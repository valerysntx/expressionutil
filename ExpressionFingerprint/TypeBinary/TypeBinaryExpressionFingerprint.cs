using System.Linq.Expressions;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.TypeBinary
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
	internal sealed class TypeBinaryExpressionFingerprint : AbstractExpressionFingerprint
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
	{
		public Type TypeOperand
		{
			get;
			private set;
		}

		public TypeBinaryExpressionFingerprint(ExpressionType nodeType, Type type, Type typeOperand ) : base(nodeType, type)
		{
      TypeOperand = typeOperand;
		}

		public override bool Equals(object obj)
		{
			TypeBinaryExpressionFingerprint typeBinaryExpressionFingerprint = obj as TypeBinaryExpressionFingerprint;
			if (typeBinaryExpressionFingerprint == null || !object.Equals(TypeOperand, typeBinaryExpressionFingerprint.TypeOperand))
			{
				return false;
			}
			return base.Equals(typeBinaryExpressionFingerprint);
		}
	}
}