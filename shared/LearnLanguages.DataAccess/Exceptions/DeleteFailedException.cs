using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class DeleteFailedException : Exception
  {
    public DeleteFailedException()
      : base()
    {

    }

    public DeleteFailedException(string errorMsg)
      : base(errorMsg)
    {

    }
  }
}
