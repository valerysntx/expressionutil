using System.Reflection;

namespace System.Web.Mvc.ExpressionUtil
{
  public interface IMethodCallExpressionFingerprint
  {
    MethodInfo Method { get; }

    bool Equals(object obj);
  }
}