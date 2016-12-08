using System;
using System.Collections;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil
{
  public class HashCodeCombiner : IHashCodeCombiner
  {
    private long _combinedHash64 = 5381;

    public int CombinedHash => _combinedHash64.GetHashCode();

    public HashCodeCombiner()
    {
    }

    public void AddEnumerable( IEnumerable e )
    {
      if (e == null)
      {
        AddInt32(0);
        return;
      }
      int num = 0;
      foreach (object obj in e)
      {
        AddObject(obj);
        num++;
      }
      AddInt32(num);
    }

    public void AddFingerprint(IExpressionFingerprint fingerprint)
    {
      if (fingerprint != null)
      {
        fingerprint.AddToHashCodeCombiner(this);
        return;
      }
      AddInt32(0);
    }

    public void AddInt32( int i )
    {
      _combinedHash64 = (_combinedHash64 << 5) + _combinedHash64 ^ (long) i;
    }

    public void AddObject( object o )
    {
      AddInt32((o != null ? o.GetHashCode() : 0));
    }
  }
}