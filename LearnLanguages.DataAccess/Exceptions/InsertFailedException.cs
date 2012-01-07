using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class InsertFailedException : Exception
  {
    public InsertFailedException()
      : base(DalResources.Dal_InsertFailedErrorMessage)
    {

    }

    public InsertFailedException(string errorMsg)
      : base(errorMsg)
    {

    }
  }
}
