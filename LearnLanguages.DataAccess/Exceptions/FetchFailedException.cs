using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class FetchFailedException : Exception
  {
    public FetchFailedException()
      : base(DalResources.ErrorMsgFetchFailed)
    {
      
    }

    public FetchFailedException(string errorMsg)
      : base(errorMsg)
    {

    }
  }
}
