using System.Linq.Expressions;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Lambda
{
  #pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class LambdaExpressionFingerprint : AbstractExpressionFingerprint
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
}