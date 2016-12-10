using System.Linq.Expressions;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Conditional
{
  using ExpressionFingerprint;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class ConditionalExpressionFingerprint : AbstractExpressionFingerprint
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public ConditionalExpressionFingerprint( ExpressionType nodeType, Type type ) : base(nodeType, type)
    {
    }

    public override bool Equals( object obj )
    {
      ConditionalExpressionFingerprint conditionalExpressionFingerprint = obj as ConditionalExpressionFingerprint;
      if (conditionalExpressionFingerprint == null)
      {
        return false;
      }
      return base.Equals(conditionalExpressionFingerprint);
    }
  }
}