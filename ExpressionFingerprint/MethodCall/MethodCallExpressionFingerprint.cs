using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.MethodCall
{
  public sealed class MethodCallExpressionFingerprint : AbstractExpressionFingerprint, IMethodCallExpressionFingerprint
  {
    public MethodCallExpressionFingerprint(ExpressionType nodeType, Type type, MethodInfo method) : base(nodeType, type)
    {
      Method = method;
    }

    public MethodInfo Method { get; }

    public override bool Equals(object obj)
    {
      var methodCallExpressionFingerprint = obj as MethodCallExpressionFingerprint;
      if (methodCallExpressionFingerprint == null || !Equals(Method, methodCallExpressionFingerprint.Method))
      {
        return false;
      }
      return Equals(methodCallExpressionFingerprint);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}