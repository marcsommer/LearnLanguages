using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class UpdateFailedException : Exception
  {
    public UpdateFailedException()
      : base(DalResources.Dal_UpdateFailedErrorMessage)
    {

    }

    public UpdateFailedException(string errorMsg)
      : base(errorMsg)
    {

    }
  }
}
