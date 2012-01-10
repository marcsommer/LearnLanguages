using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class UpdateFailedException : Exception
  {
    public UpdateFailedException()  
      : base(DalResources.ErrorMsgUpdateFailed)
    {

    }

    public UpdateFailedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public UpdateFailedException(Exception innerException)
      : base(DalResources.ErrorMsgUpdateFailed, innerException)
    {

    }
  }
}
