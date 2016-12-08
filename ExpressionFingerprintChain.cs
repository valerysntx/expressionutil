using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil
{
  public class ExpressionFingerprintChain : IEquatable<ExpressionFingerprintChain>
  {
    public readonly List<AbstractExpressionFingerprint> Elements = new List<AbstractExpressionFingerprint>();

    private ExpressionFingerprintChain(IEnumerable<IExpressionFingerprint> elements)
    {
      Elements = new List<AbstractExpressionFingerprint>((IEnumerable<AbstractExpressionFingerprint>) elements);   
    }

    public ExpressionFingerprintChain()
    {
    }

    public bool Equals( ExpressionFingerprintChain other )
    {
      if (Elements.Count != other?.Elements.Count)
      {
        return false;
      }
      return !Elements
          .Where(( fingerprint, i ) => !Equals(fingerprint, other.Elements[i]))
          .Any();
    }

    public override bool Equals( object obj )
    {
      return Equals(obj as ExpressionFingerprintChain);
    }

    public override int GetHashCode()
    {
      HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
      Elements.ForEach(new Action<AbstractExpressionFingerprint>(fingerprint => hashCodeCombiner.AddFingerprint(fingerprint)));
      return hashCodeCombiner.CombinedHash;
    }
  }
}