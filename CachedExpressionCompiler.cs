using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Web.Mvc.ExpressionUtil
{
  //  Expression Compiler w. Caching


  /// <summary>
  /// Cached Expression Compiler
  /// </summary>
  public static class CachedExpressionCompiler
	{
		public static Func<TModel, TValue> Process<TModel, TValue>(Expression<Func<TModel, TValue>> lambdaExpression)
		{
			return Compiler<TModel, TValue>.Compile(lambdaExpression);
		}

    /// <summary>
    /// Compiler
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
		public static class Compiler<TIn, TOut>
		{
			private static Func<TIn, TOut> _identityFunc;

			private readonly static ConcurrentDictionary<MemberInfo, Func<TIn, TOut>> _simpleMemberAccessDict;
			private readonly static ConcurrentDictionary<MemberInfo, Func<object, TOut>> _constMemberAccessDict;
			private readonly static ConcurrentDictionary<ExpressionFingerprintChain, Hoisted<TIn, TOut>> _fingerprintedCache;

			static Compiler()
			{
        _simpleMemberAccessDict = new ConcurrentDictionary<MemberInfo, Func<TIn, TOut>>();
        _constMemberAccessDict = new ConcurrentDictionary<MemberInfo, Func<object, TOut>>();
        _fingerprintedCache = new ConcurrentDictionary<ExpressionFingerprintChain, Hoisted<TIn, TOut>>();
			}

      #region Main  

      /// <summary>
      ///   Compile (will it boost perf 10x?)
      /// </summary>
      /// <param name="expr"></param>
      /// <remarks>
      ///   http://stackoverflow.com/questions/19678074/caching-compiled-expression-tree
      /// </remarks>
      /// <example>
      /// <code> 
      ///       
      ///      CompileSlow is Expression.Compile() default implementation
      /// 
      /// </code>
      /// </example>
      /// <returns></returns>
      public static Func<TIn, TOut> Compile(Expression<Func<TIn, TOut>> expr)
			{

#if PERF_ENABLED
          
#endif
        return CompileFromIdentityFunc(expr) ?? (
                  CompileFromConstLookup(expr) ?? (
                      CompileFromMemberAccess(expr) ?? ( 
                           CompileFromFingerprint(expr) ?? CompileSlow(expr)
                          )
                       )
                  );
			}

      #endregion

      public static Func<TIn, TOut> CompileFromConstLookup(Expression<Func<TIn, TOut>> expr)
			{
				ConstantExpression body = expr.Body as ConstantExpression;
				if (body == null)
				{
					return null;
				}
				TOut value = (TOut)body.Value;
				return (TIn _) => value;
			}

      public static Func<TIn, TOut> CompileFromFingerprint(Expression<Func<TIn, TOut>> expr)
			{
				ICollection<WeakReference> objs;
				ExpressionFingerprintChain fingerprintChain = FingerprintingExpressionVisitor.GetFingerprintChain(expr, out objs);
				if (fingerprintChain == null)
				{
					return null;
				}
				Hoisted<TIn, TOut> orAdd = _fingerprintedCache.GetOrAdd(fingerprintChain, (ExpressionFingerprintChain _) => HoistingExpressionVisitor<TIn, TOut>.Hoist(expr).Compile());
				return (TIn model) => orAdd(model, objs);
			}

			public static Func<TIn, TOut> CompileFromIdentityFunc(Expression<Func<TIn, TOut>> expr)
			{
				if (expr.Body != expr.Parameters[0])
				{
					return null;
				}
				if (_identityFunc == null)
				{
					_identityFunc = expr.Compile();
				}
				return _identityFunc;
			}

			public static Func<TIn, TOut> CompileFromMemberAccess(Expression<Func<TIn, TOut>> expr)
			{
				var body = expr.Body as MemberExpression;
				if (body != null)
				{
					if (body.Expression == expr.Parameters[0] || body.Expression == null)
					{
						return _simpleMemberAccessDict.GetOrAdd(body.Member, (MemberInfo _) => expr.Compile());
					}
					ConstantExpression expression = body.Expression as ConstantExpression;
					if (expression != null)
					{
						Func<object, TOut> orAdd = _constMemberAccessDict.GetOrAdd(body.Member, (MemberInfo _) => {
							ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "capturedLocal");
							UnaryExpression unaryExpression = Expression.Convert(parameterExpression, body.Member.DeclaringType);
							return Expression.Lambda<Func<object, TOut>>(body.Update(unaryExpression), new ParameterExpression[] { parameterExpression }).Compile();
						});
						object value = expression.Value;
						return (TIn _) => orAdd(value);
					}
				}
				return null;
			}

			public static Func<TIn, TOut> CompileSlow(Expression<Func<TIn, TOut>> expr)
			{
				return expr.Compile();
			}
		}
	}
}