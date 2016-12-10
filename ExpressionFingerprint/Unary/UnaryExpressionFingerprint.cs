using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Unary
{
    public class UnaryExpressionFingerprint : AbstractExpressionFingerprint
	{
		public MethodInfo Method
		{
			get;
			private set;
		}

		public UnaryExpressionFingerprint(ExpressionType nodeType, Type type, MethodInfo method) : base(nodeType, type)
		{
      		Method = method;
		}

		public override int GetHashCode(){
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			UnaryExpressionFingerprint unaryExpressionFingerprint = obj as UnaryExpressionFingerprint;
			if (unaryExpressionFingerprint == null || !object.Equals(Method, unaryExpressionFingerprint.Method))
			{
				return false;
			}
			return base.Equals(unaryExpressionFingerprint);
		}
	}
}