using System.Linq.Expressions;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil
{
  public class ConditionalExpressionFingerprint : AbstractExpressionFingerprint, IEquatable<ConditionalExpressionFingerprint>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public ConditionalExpressionFingerprint( ExpressionType nodeType, Type type ) : base(nodeType, type)
    {
    }

    internal override void AddToHashCodeCombiner(HashCodeCombiner combiner) => base.AddToHashCodeCombiner(combiner);

    public override bool Equals( object obj )
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      var other = obj as ConditionalExpressionFingerprint;
      return other != null && Equals(other);
    }

    public bool Equals(ConditionalExpressionFingerprint other)
    {
      throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
      throw new NotImplementedException();
    }

    public static bool operator ==(ConditionalExpressionFingerprint left, ConditionalExpressionFingerprint right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(ConditionalExpressionFingerprint left, ConditionalExpressionFingerprint right)
    {
      return !Equals(left, right);
    }
  }
}