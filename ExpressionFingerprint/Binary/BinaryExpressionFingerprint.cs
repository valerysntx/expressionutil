using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Binary
{
  /// <summary>
  /// 
  /// </summary>
  public class BinaryExpressionFingerprint : AbstractExpressionFingerprint, IEquatable<BinaryExpressionFingerprint>
  {
    public MethodInfo Method { get; }

    public BinaryExpressionFingerprint( ExpressionType nodeType, Type type, MethodInfo method ) : base(nodeType, type)
    {
      Method = method;
    }

    public override bool Equals( object obj )
    {
      var binaryExpressionFingerprint = obj as BinaryExpressionFingerprint;
      if (binaryExpressionFingerprint == null || !object.Equals(Method, binaryExpressionFingerprint.Method))
      {
        return false;
      }
      return base.Equals(binaryExpressionFingerprint);
    }

    public bool Equals(BinaryExpressionFingerprint other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return base.Equals(other) && Equals(Method, other.Method);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (base.GetHashCode()*397) ^ ((Method != default(MethodInfo)) ? Method.GetHashCode() : 0);
      }
    }

    public static bool operator ==(BinaryExpressionFingerprint left, BinaryExpressionFingerprint right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(BinaryExpressionFingerprint left, BinaryExpressionFingerprint right)
    {
      return !Equals(left, right);
    }
  }

}