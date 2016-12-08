using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Web.Mvc.ExpressionUtil
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
	internal sealed class UnaryExpressionFingerprint : ExpressionFingerprint
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
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

		internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
		{
			combiner.AddObject(Method);
			base.AddToHashCodeCombiner(combiner);
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