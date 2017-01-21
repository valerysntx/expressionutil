using System.Collections;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil
{
  public interface IHashCodeCombiner
  {
    int CombinedHash { get; }
    void AddEnumerable( IEnumerable e );
    void AddFingerprint( IExpressionFingerprint fingerprint );
    void AddInt32( int i );
    void AddObject( object o );
  }
}