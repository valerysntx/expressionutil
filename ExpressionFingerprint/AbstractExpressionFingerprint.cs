using System.Linq.Expressions;

namespace System.Web.Mvc.ExpressionUtil.ExpressionFingerprint
{
  namespace ExpressionFingerprint
  {
    public abstract partial class AbstractExpressionFingerprint : IExpressionFingerprint
    {
      public ExpressionType NodeType { get; }

      public Type Type { get; }

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
      public virtual void AddToHashCodeCombiner(IHashCodeCombiner combiner)
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
        // ReSharper disable once CheckForReferenceEqualityInstead.2
        return Equals(Type, other.Type);
      }

      public override bool Equals(object obj)
      {
        return Equals(obj as AbstractExpressionFingerprint);
      }

      public override int GetHashCode()
      {
        var hashCodeCombiner = new HashCodeCombiner();
        AddToHashCodeCombiner(hashCodeCombiner);
        return hashCodeCombiner.CombinedHash;
      }

    }
  }
}