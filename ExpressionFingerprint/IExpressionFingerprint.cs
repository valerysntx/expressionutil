namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint
{
  using System.Linq.Expressions;

  /// <summary>
  /// IExpressionFingerprint will contain the 'cached' compiled expression
  /// </summary>
  public interface IExpressionFingerprint
  {
    ExpressionType NodeType { get; }
    Type Type { get; }

    bool Equals(object obj);
    int GetHashCode();
  }
}