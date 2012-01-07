using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class CreateFailedException : Exception
  {
    public CreateFailedException()
      : base(DalResources.Dal_CreateFailedErrorMessage)
    {

    }

    public CreateFailedException(string errorMsg)
      : base(errorMsg)
    {

    }
  }
}
