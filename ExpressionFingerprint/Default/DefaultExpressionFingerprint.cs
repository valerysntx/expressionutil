using System.Linq.Expressions;

namespace System.Web.Mvc.ExpressionUtil
{
	namespace ExpressionFingerprint
  {
    namespace Default
    {
      public partial class DefaultExpressionFingerprint : AbstractExpressionFingerprint
      {
        public DefaultExpressionFingerprint( ExpressionType nodeType, Type type ) : base(nodeType, type)
        {
        }
      }
    }
  }
}