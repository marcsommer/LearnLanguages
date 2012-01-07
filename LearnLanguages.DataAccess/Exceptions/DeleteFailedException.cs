using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class DeleteFailedException : Exception
  {
    public DeleteFailedException()
      : base(DalResources.Dal_DeleteFailedErrorMessage)
    {

    }

    public DeleteFailedException(string errorMsg)
      : base(errorMsg)
    {

    }
  }
}
