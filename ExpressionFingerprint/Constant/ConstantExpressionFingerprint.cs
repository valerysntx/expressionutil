using System.Linq.Expressions;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Constant
{
  namespace Constant
  {
    public class ConstantExpressionFingerprint : AbstractExpressionFingerprint
    {
      public ConstantExpressionFingerprint(ExpressionType nodeType, Type type) : base(nodeType, type)
      {
      }

      public override bool Equals(object obj)
      {
        var constantExpressionFingerprint = obj as ConstantExpressionFingerprint;
        if (constantExpressionFingerprint == null)
        {
          return false;
        }
        return Equals(constantExpressionFingerprint);
      }

      public override int GetHashCode()
      {
        return base.GetHashCode();
      }
    }
  }
}