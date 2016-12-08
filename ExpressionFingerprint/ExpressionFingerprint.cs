namespace System.Web.Mvc.ExpressionUtil
{
  using System.Linq.Expressions;

  namespace ExpressionFingerprint
  {
    using System.Web.Mvc.ExpressionUtil;

    public abstract partial class AbstractExpressionFingerprint : IExpressionFingerprint
    {
      public ExpressionType NodeType { get; private set; }

      public Type Type { get; private set; }

      /// <summary>
      /// Restrict cast 
      /// </summary>
      private AbstractExpressionFingerprint()
      {
        if (GetType().IsAbstract)
        {
          throw new InvalidOperationException("please implement inheritor from abstract expression fingerprint");
        }
      }

      protected AbstractExpressionFingerprint(ExpressionType nodeType, Type type)
      {
        NodeType = nodeType;
        Type = type;
      }

      /// <summary>
      /// instance hashing  
      /// //TODO: refactor
      /// </summary>
      /// <param name="combiner"></param>
      internal virtual void AddToHashCodeCombiner(HashCodeCombiner combiner)
      {
        combiner.AddInt32((int) NodeType);
        combiner.AddObject(Type);
      }

      public bool Equals(AbstractExpressionFingerprint other)
      {
        if (other == null || NodeType != other.NodeType)
        {
          return false;
        }
        return Equals(Type, other.Type);
      }

      public override bool Equals(object obj)
      {
        return Equals(obj as AbstractExpressionFingerprint);
      }

      public override int GetHashCode()
      {
        HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
        AddToHashCodeCombiner(hashCodeCombiner);
        return hashCodeCombiner.CombinedHash;
      }
    }
  }
}