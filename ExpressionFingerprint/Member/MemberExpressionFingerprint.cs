using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Member
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class MemberExpressionFingerprint : AbstractExpressionFingerprint
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public MemberInfo Member
    {
      get;
      private set;
    }

    public MemberExpressionFingerprint( ExpressionType nodeType, Type type, MemberInfo member ) : base(nodeType, type)
    {
      Member = member;
    }

#pragma warning disable 659
    public override bool Equals( object obj )
#pragma warning restore 659
    {
      MemberExpressionFingerprint memberExpressionFingerprint = obj as MemberExpressionFingerprint;
      if (memberExpressionFingerprint == null || !object.Equals(Member, memberExpressionFingerprint.Member))
      {
        return false;
      }
      return base.Equals(memberExpressionFingerprint);
    }
  }
}