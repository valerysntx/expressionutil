using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil
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

		internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
		{
			combiner.AddObject(TypeOperand);
			base.AddToHashCodeCombiner(combiner);
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