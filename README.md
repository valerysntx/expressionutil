## expressionutil
#### Lambda Expressions Compiler

```
public static Func<TIn, TOut> CompileSlow(Expression<Func<TIn, TOut>> expr) {
	return expr.Compile();
}

// we will cache the chain

CachedExpressionCompiler.Process<TModel,TValue>(Expression<Func<TModel, TValue>> lambda)
```

 * Implementation
```
   public static Func<TIn, TOut> Compile(Expression<Func<TIn, TOut>> expr)
   {
        return CompileFromIdentityFunc(expr) ?? (
                  CompileFromConstLookup(expr) ?? (
                      CompileFromMemberAccess(expr) ?? ( 
                           CompileFromFingerprint(expr) ?? CompileSlow(expr)
                          )
                       )
                  );
	}

	public static Func<TIn, TOut> CompileFromFingerprint(Expression<Func<TIn, TOut>> expr)
	{
				ICollection<WeakReference> objs;
				ExpressionFingerprintChain fingerprintChain = FingerprintingExpressionVisitor.GetFingerprintChain(expr, out objs);
				if (fingerprintChain == null)
				{
					return null;
				}
				Hoisted<TIn, TOut> orAdd =
				_fingerprintedCache.GetOrAdd(fingerprintChain, (ExpressionFingerprintChain _) => 
				HoistingExpressionVisitor<TIn, TOut>.Hoist(expr).Compile());
				return (TIn model) => orAdd(model, objs);
				}
				return null;
			}

			public static Func<TIn, TOut> CompileSlow(Expression<Func<TIn, TOut>> expr)
			{
				return expr.Compile();
			}
		}
	}
```

