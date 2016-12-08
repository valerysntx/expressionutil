using System.Linq.Expressions;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Default
{
      public partial class DefaultExpressionFingerprint : AbstractExpressionFingerprint
      {
        public DefaultExpressionFingerprint(ExpressionType nodeType, Type type) : base(nodeType, type)
        {
        }
      }
}