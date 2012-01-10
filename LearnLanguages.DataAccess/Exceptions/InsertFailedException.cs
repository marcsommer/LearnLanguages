using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class InsertFailedException : Exception
  {
    public InsertFailedException()
      : base(DalResources.ErrorMsgInsertFailed)
    {

    }

    public InsertFailedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public InsertFailedException(Exception innerException)
      : base(DalResources.ErrorMsgInsertFailed, innerException)
    {

    }
  }
}
