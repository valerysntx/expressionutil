using System.Collections.Generic;

namespace System.Web.Mvc.ExpressionUtil
{
  public delegate TValue Hoisted<TModel, TValue>( TModel model, ICollection<WeakReference> capturedConstants );
}