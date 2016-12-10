using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.ExpressionFingerprint;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint.Index
{
  public sealed class IndexExpressionFingerprint : AbstractExpressionFingerprint
  {
    public PropertyInfo Indexer
    {
      get;
      private set;
    }

    public IndexExpressionFingerprint( ExpressionType nodeType, Type type, PropertyInfo indexer ) : base(nodeType, type)
    {
      Indexer = indexer;
    }

   

    public override bool Equals( object obj )
    {
      IndexExpressionFingerprint indexExpressionFingerprint = obj as IndexExpressionFingerprint;
      if (indexExpressionFingerprint == null || !object.Equals(Indexer, indexExpressionFingerprint.Indexer))
      {
        return false;
      }
      return base.Equals(indexExpressionFingerprint);
    }
  }
}