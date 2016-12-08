namespace System.Web.Mvc.ExpressionUtil
{
  public interface IParameterExpressionFingerprint
  {
    int ParameterIndex { get; }

    bool Equals(object obj);
  }
}