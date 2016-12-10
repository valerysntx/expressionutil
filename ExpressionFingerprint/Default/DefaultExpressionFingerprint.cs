using System.Linq.Expressions;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Default
{
  public class DefaultExpressionFingerprint : AbstractExpressionFingerprint
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

}