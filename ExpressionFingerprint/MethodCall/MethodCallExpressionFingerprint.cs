
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil
{
  public sealed class MethodCallExpressionFingerprint : AbstractExpressionFingerprint, IMethodCallExpressionFingerprint
  {
		public MethodInfo Method
		{
			get;
			private set;
		}

		public MethodCallExpressionFingerprint(ExpressionType nodeType, Type type, MethodInfo method) : base(nodeType, type)
		{
      Method = method;
		}

		internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
		{
			combiner.AddObject(Method);
			base.AddToHashCodeCombiner(combiner);
		}

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override bool Equals(object obj)
		{
			MethodCallExpressionFingerprint methodCallExpressionFingerprint = obj as MethodCallExpressionFingerprint;
			if (methodCallExpressionFingerprint == null || !object.Equals(Method, methodCallExpressionFingerprint.Method))
			{
				return false;
			}
			return base.Equals(methodCallExpressionFingerprint);
		}
	}
}