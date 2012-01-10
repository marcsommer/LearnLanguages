using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class GetAllFailedException : Exception
  {
    public GetAllFailedException()
      : base(DalResources.ErrorMsgGetAllFailed)
    {
      
    }

    public GetAllFailedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public GetAllFailedException(Exception innerException)
      : base(DalResources.ErrorMsgGetAllFailed, innerException)
    {

    }
  }
}
