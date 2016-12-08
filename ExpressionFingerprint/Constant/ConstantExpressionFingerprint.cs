using System;
using System.Linq.Expressions;

namespace System.Web.Mvc.ExpressionUtil
{
  using ExpressionFingerprint;

  namespace Constant {

    public class ConstantExpressionFingerprint : AbstractExpressionFingerprint
    {

      public ConstantExpressionFingerprint( ExpressionType nodeType, Type type ) : base(nodeType, type)
      {
      }

      public override bool Equals( object obj )
      {
        ConstantExpressionFingerprint constantExpressionFingerprint = obj as ConstantExpressionFingerprint;
        if (constantExpressionFingerprint == null)
        {
          return false;
        }
        return base.Equals(constantExpressionFingerprint);
      }

      internal override void AddToHashCodeCombiner( HashCodeCombiner combiner )
      {
        base.AddToHashCodeCombiner(combiner);
      }

      public override int GetHashCode()
      {
        return base.GetHashCode();
      }
    }
  }
}