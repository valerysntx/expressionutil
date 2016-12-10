using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Web.Mvc.ExpressionUtil
{
  public class HoistingExpressionVisitor<TIn, TOut> : ExpressionVisitor
  {
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once StaticMemberInGenericType
    private readonly static ParameterExpression _hoistedConstantsParamExpr;

    private int _numConstantsProcessed;

    static HoistingExpressionVisitor()
    {
      _hoistedConstantsParamExpr = Expression.Parameter(typeof(List<WeakReference>), "hoistedConstants");
    }

    private HoistingExpressionVisitor()
    {
    }

    public static Expression<Hoisted<TIn, TOut>> Hoist( Expression<Func<TIn, TOut>> expr )
    {
      Expression expression = (new HoistingExpressionVisitor<TIn, TOut>()).Visit(expr.Body);
      ParameterExpression[] item = new ParameterExpression[] { expr.Parameters[0], _hoistedConstantsParamExpr };
      return Expression.Lambda<Hoisted<TIn, TOut>>(expression, item );
    }

    protected override Expression VisitConstant( ConstantExpression node )
    {
      ParameterExpression parameterExpression = _hoistedConstantsParamExpr;
      Expression[] expressionArray = new Expression[1];
      HoistingExpressionVisitor<TIn, TOut> hoistingExpressionVisitor = this;
      int num = hoistingExpressionVisitor._numConstantsProcessed;
      int num1 = num;
      hoistingExpressionVisitor._numConstantsProcessed = num + 1;
      expressionArray[0] = Expression.Constant(num1);
      return Expression.Convert(Expression.Property(parameterExpression, "Item", expressionArray), node.Type);
    }
  }
}