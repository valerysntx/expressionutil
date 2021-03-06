using System.Linq.Expressions;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Parameter
{
  using ExpressionFingerprint;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public sealed class ParameterExpressionFingerprint : AbstractExpressionFingerprint, IParameterExpressionFingerprint
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public int ParameterIndex { get; private set; }

    public ParameterExpressionFingerprint(ExpressionType nodeType, Type type, int parameterIndex) : base(nodeType, type)
    {
      ParameterIndex = parameterIndex;
    }

    public override bool Equals(object obj)
    {
      ParameterExpressionFingerprint parameterExpressionFingerprint = obj as ParameterExpressionFingerprint;
      if (parameterExpressionFingerprint == null || ParameterIndex != parameterExpressionFingerprint.ParameterIndex)
      {
        return false;
      }
      return Equals(parameterExpressionFingerprint);
    }
  }
}