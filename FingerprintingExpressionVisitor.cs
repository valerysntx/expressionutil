using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Binary;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Conditional;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Constant.Constant;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Default;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Index;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Lambda;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Member;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.MethodCall;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Parameter;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.TypeBinary;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Unary;

namespace System.Web.Mvc.ExpressionUtil
{
  /// <summary>
  /// WeakReference object chanin fingerprinting
  /// </summary>
  class FingerprintingExpressionVisitor : ExpressionVisitor
  {
    private readonly List<WeakReference> _seenConstants = new List<WeakReference>();

    private readonly List<ParameterExpression> _seenParameters = new List<ParameterExpression>();

    private readonly ExpressionFingerprintChain _currentChain = new ExpressionFingerprintChain();

    private bool _gaveUp;

    private FingerprintingExpressionVisitor()
    {
    }

    public static ExpressionFingerprintChain GetFingerprintChain( Expression expr, out List<WeakReference> capturedConstants )
    {
      FingerprintingExpressionVisitor fingerprintingExpressionVisitor = new FingerprintingExpressionVisitor();
      fingerprintingExpressionVisitor.Visit(expr);
      if (fingerprintingExpressionVisitor._gaveUp)
      {
        capturedConstants = null;
        return null;
      }
      capturedConstants = fingerprintingExpressionVisitor._seenConstants;
      return fingerprintingExpressionVisitor._currentChain;
    }

    private T GiveUp<T>( T node )
    {
      _gaveUp = true;
      return node;
    }

    public override Expression Visit( Expression node )
    {
      if (node != null)
      {
        return base.Visit(node);
      }
      _currentChain.Elements.Add(null);
      return null;
    }

    protected override Expression VisitBinary( BinaryExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new BinaryExpressionFingerprint(node.NodeType, node.Type, node.Method));
      return base.VisitBinary(node);
    }

    protected override Expression VisitBlock( BlockExpression node )
    {
      return GiveUp(node);
    }

    protected override CatchBlock VisitCatchBlock( CatchBlock node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitConditional( ConditionalExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new ConditionalExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitConditional(node);
    }

    protected override Expression VisitConstant( ConstantExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _seenConstants.Add(new WeakReference(node.Value));
      _currentChain.Elements.Add(new ConstantExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitConstant(node);
    }

    protected override Expression VisitDebugInfo( DebugInfoExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitDefault( DefaultExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new DefaultExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitDefault(node);
    }

    internal static ExpressionFingerprintChain GetFingerprintChain<TIn, TOut>( Expression<Func<TIn, TOut>> expr, out ICollection<WeakReference> objs )
    {
      throw new NotImplementedException();
    }

    protected override Expression VisitDynamic( DynamicExpression node )
    {
      return GiveUp(node);
    }

    protected override ElementInit VisitElementInit( ElementInit node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitExtension( Expression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitGoto( GotoExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitIndex( IndexExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new IndexExpressionFingerprint(node.NodeType, node.Type, node.Indexer));
      return base.VisitIndex(node);
    }

    protected override Expression VisitInvocation( InvocationExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitLabel( LabelExpression node )
    {
      return GiveUp(node);
    }

    protected override LabelTarget VisitLabelTarget( LabelTarget node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitLambda<T>( Expression<T> node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new LambdaExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitLambda(node);
    }

    protected override Expression VisitListInit( ListInitExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitLoop( LoopExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitMember( MemberExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new MemberExpressionFingerprint(node.NodeType, node.Type, node.Member));
      return base.VisitMember(node);
    }

    protected override MemberAssignment VisitMemberAssignment( MemberAssignment node )
    {
      return GiveUp(node);
    }

    protected override MemberBinding VisitMemberBinding( MemberBinding node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitMemberInit( MemberInitExpression node )
    {
      return GiveUp(node);
    }

    protected override MemberListBinding VisitMemberListBinding( MemberListBinding node )
    {
      return GiveUp(node);
    }

    protected override MemberMemberBinding VisitMemberMemberBinding( MemberMemberBinding node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitMethodCall( MethodCallExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new MethodCallExpressionFingerprint(node.NodeType, node.Type, node.Method));
      return base.VisitMethodCall(node);
    }

    protected override Expression VisitNew( NewExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitNewArray( NewArrayExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitParameter( ParameterExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      int count = _seenParameters.IndexOf(node);
      if (count < 0)
      {
        count = _seenParameters.Count;
        _seenParameters.Add(node);
      }
      _currentChain.Elements.Add(new ParameterExpressionFingerprint(node.NodeType, node.Type, count));
      return base.VisitParameter(node);
    }

    protected override Expression VisitRuntimeVariables( RuntimeVariablesExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitSwitch( SwitchExpression node )
    {
      return GiveUp(node);
    }

    protected override SwitchCase VisitSwitchCase( SwitchCase node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitTry( TryExpression node )
    {
      return GiveUp(node);
    }

    protected override Expression VisitTypeBinary( TypeBinaryExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new TypeBinaryExpressionFingerprint(node.NodeType, node.Type, node.TypeOperand));
      return base.VisitTypeBinary(node);
    }

    protected override Expression VisitUnary( UnaryExpression node )
    {
      if (_gaveUp)
      {
        return node;
      }
      _currentChain.Elements.Add(new UnaryExpressionFingerprint(node.NodeType, node.Type, node.Method));
      return base.VisitUnary(node);
    }
  }
}