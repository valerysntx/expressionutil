using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Binary;

namespace System.Web.Mvc.ExpressionUtil
{ 
  using ExpressionFingerprint;
  using Constant;

  //  Expression Compiler w. Caching
  public delegate TValue Hoisted<TModel, TValue>( TModel model, ICollection<WeakReference> capturedConstants );

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class DefaultExpressionFingerprint : AbstractExpressionFingerprint
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public DefaultExpressionFingerprint( ExpressionType nodeType, Type type ) : base(nodeType, type)
    {
    }

    public override bool Equals( object obj )
    {
      DefaultExpressionFingerprint defaultExpressionFingerprint = obj as DefaultExpressionFingerprint;
      if (defaultExpressionFingerprint == null)
      {
        return false;
      }
      return base.Equals(defaultExpressionFingerprint);
    }
  }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public sealed class IndexExpressionFingerprint : AbstractExpressionFingerprint
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public PropertyInfo Indexer
    {
      get;
      private set;
    }

    public IndexExpressionFingerprint( ExpressionType nodeType, Type type, PropertyInfo indexer ) : base(nodeType, type)
    {
      Indexer = indexer;
    }

    internal override void AddToHashCodeCombiner( HashCodeCombiner combiner )
    {
      combiner.AddObject(Indexer);
      base.AddToHashCodeCombiner(combiner);
    }

    public override bool Equals( object obj )
    {
      IndexExpressionFingerprint indexExpressionFingerprint = obj as IndexExpressionFingerprint;
      if (indexExpressionFingerprint == null || !object.Equals(Indexer, indexExpressionFingerprint.Indexer))
      {
        return false;
      }
      return base.Equals(indexExpressionFingerprint);
    }
  }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
   public class LambdaExpressionFingerprint : AbstractExpressionFingerprint
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public LambdaExpressionFingerprint( ExpressionType nodeType, Type type ) : base(nodeType, type)
    {
    }

    public override bool Equals( object obj )
    {
      LambdaExpressionFingerprint lambdaExpressionFingerprint = obj as LambdaExpressionFingerprint;
      if (lambdaExpressionFingerprint == null)
      {
        return false;
      }
      return base.Equals(lambdaExpressionFingerprint);
    }
  }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class MemberExpressionFingerprint : AbstractExpressionFingerprint
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public MemberInfo Member
    {
      get;
      private set;
    }

    public MemberExpressionFingerprint( ExpressionType nodeType, Type type, MemberInfo member ) : base(nodeType, type)
    {
      Member = member;
    }

    internal override void AddToHashCodeCombiner( HashCodeCombiner combiner )
    {
      combiner.AddObject(Member);
      base.AddToHashCodeCombiner(combiner);
    }

    public override bool Equals( object obj )
    {
      MemberExpressionFingerprint memberExpressionFingerprint = obj as MemberExpressionFingerprint;
      if (memberExpressionFingerprint == null || !object.Equals(Member, memberExpressionFingerprint.Member))
      {
        return false;
      }
      return base.Equals(memberExpressionFingerprint);
    }
  }


  /***************************/
  public class HoistingExpressionVisitor<TIn, TOut> : ExpressionVisitor
  {
    private readonly static ParameterExpression _hoistedConstantsParamExpr;

    private int _numConstantsProcessed;

    static HoistingExpressionVisitor()
    {
      _hoistedConstantsParamExpr = Expression.Parameter(typeof(List<WeakReference>), "hoistedConstants");
    }

    private HoistingExpressionVisitor()
    {
    }

    public static Expression<Hoisted<TIn, TOut>> Hoist( Expression<Func<TIn, TOut>> expr )
    {
      Expression expression = (new HoistingExpressionVisitor<TIn, TOut>()).Visit(expr.Body);
      ParameterExpression[] item = new ParameterExpression[] { expr.Parameters[0], _hoistedConstantsParamExpr };
      return Expression.Lambda<Hoisted<TIn, TOut>>(expression, item );
    }

    protected override Expression VisitConstant( ConstantExpression node )
    {
      ParameterExpression parameterExpression = _hoistedConstantsParamExpr;
      Expression[] expressionArray = new Expression[1];
      HoistingExpressionVisitor<TIn, TOut> hoistingExpressionVisitor = this;
      int num = hoistingExpressionVisitor._numConstantsProcessed;
      int num1 = num;
      hoistingExpressionVisitor._numConstantsProcessed = num + 1;
      expressionArray[0] = Expression.Constant(num1);
      return Expression.Convert(Expression.Property(parameterExpression, "Item", expressionArray), node.Type);
    }
  }

  /// <summary>
  /// WeakReference object chanin fingerprinting
  /// </summary>
  class FingerprintingExpressionVisitor : ExpressionVisitor
  {
    private readonly List<WeakReference> _seenConstants = new List<WeakReference>();

    private readonly List<ParameterExpression> _seenParameters = new List<ParameterExpression>();

    private readonly ExpressionFingerprintChain _currentChain = new ExpressionFingerprintChain();

    private bool _gaveUp;

    private FingerprintingExpressionVisitor()
    {
    }

    public static ExpressionFingerprintChain GetFingerprintChain( Expression expr, out List<WeakReference> capturedConstants )
    {
      FingerprintingExpressionVisitor fingerprintingExpressionVisitor = new FingerprintingExpressionVisitor();
      fingerprintingExpressionVisitor.Visit(expr);
      if (fingerprintingExpressionVisitor._gaveUp)
      {
        capturedConstants = null;
        return null;
      }
      capturedConstants = fingerprintingExpressionVisitor._seenConstants;
      return fingerprintingExpressionVisitor._currentChain;
    }

    private T GiveUp<T>( T node )
    {
      _gaveUp = true;
      return node;
    }

    public override Expression Visit( Expression node )
    {
      if (node != null)
      {
        return base.Visit(node);
      }
      _currentChain.Elements.Add(null);
      return null;
    }

    protected override Expression VisitBinary( BinaryExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new BinaryExpressionFingerprint(node.NodeType, node.Type, node.Method));
      return base.VisitBinary(node);
    }

    protected override Expression VisitBlock( BlockExpression node )
    {
      return GiveUp(node);
    }

    protected override CatchBlock VisitCatchBlock( CatchBlock node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitConditional( ConditionalExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new ConditionalExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitConditional(node);
    }

    protected override Expression VisitConstant( ConstantExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _seenConstants.Add(new WeakReference(node.Value));
      _currentChain.Elements.Add(new ConstantExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitConstant(node);
    }

    protected override Expression VisitDebugInfo( DebugInfoExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitDefault( DefaultExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new DefaultExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitDefault(node);
    }

    internal static ExpressionFingerprintChain GetFingerprintChain<TIn, TOut>( Expression<Func<TIn, TOut>> expr, out ICollection<WeakReference> objs )
    {
      throw new NotImplementedException();
    }

    protected override Expression VisitDynamic( DynamicExpression node )
    {
      return GiveUp(node);
    }

    protected override ElementInit VisitElementInit( ElementInit node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitExtension( Expression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitGoto( GotoExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitIndex( IndexExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new IndexExpressionFingerprint(node.NodeType, node.Type, node.Indexer));
      return base.VisitIndex(node);
    }

    protected override Expression VisitInvocation( InvocationExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitLabel( LabelExpression node )
    {
      return GiveUp(node);
    }

    protected override LabelTarget VisitLabelTarget( LabelTarget node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitLambda<T>( Expression<T> node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new LambdaExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitLambda(node);
    }

    protected override Expression VisitListInit( ListInitExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitLoop( LoopExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitMember( MemberExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new MemberExpressionFingerprint(node.NodeType, node.Type, node.Member));
      return base.VisitMember(node);
    }

    protected override MemberAssignment VisitMemberAssignment( MemberAssignment node )
    {
      return GiveUp(node);
    }

    protected override MemberBinding VisitMemberBinding( MemberBinding node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitMemberInit( MemberInitExpression node )
    {
      return GiveUp(node);
    }

    protected override MemberListBinding VisitMemberListBinding( MemberListBinding node )
    {
      return GiveUp(node);
    }

    protected override MemberMemberBinding VisitMemberMemberBinding( MemberMemberBinding node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitMethodCall( MethodCallExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new MethodCallExpressionFingerprint(node.NodeType, node.Type, node.Method));
      return base.VisitMethodCall(node);
    }

    protected override Expression VisitNew( NewExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitNewArray( NewArrayExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitParameter( ParameterExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      int count = _seenParameters.IndexOf(node);
      if (count < 0)
      {
        count = _seenParameters.Count;
        _seenParameters.Add(node);
      }
      _currentChain.Elements.Add(new ParameterExpressionFingerprint(node.NodeType, node.Type, count));
      return base.VisitParameter(node);
    }

    protected override Expression VisitRuntimeVariables( RuntimeVariablesExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitSwitch( SwitchExpression node )
    {
      return GiveUp(node);
    }

    protected override SwitchCase VisitSwitchCase( SwitchCase node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitTry( TryExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitTypeBinary( TypeBinaryExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new TypeBinaryExpressionFingerprint(node.NodeType, node.Type, node.TypeOperand));
      return base.VisitTypeBinary(node);
    }

    protected override Expression VisitUnary( UnaryExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new UnaryExpressionFingerprint(node.NodeType, node.Type, node.Method));
      return base.VisitUnary(node);
    }
  }

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
				MemberExpression body = expr.Body as MemberExpression;
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