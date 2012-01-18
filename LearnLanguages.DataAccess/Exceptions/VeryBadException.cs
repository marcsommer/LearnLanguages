using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class VeryBadException : Exception
  {
    public VeryBadException() 
      : base()
    {
    }

    public VeryBadException(string errorMsg)
      : base(errorMsg)
    {

    }

    public VeryBadException(Exception innerException)
      : base(DalResources.ErrorMsgVeryBadException, innerException)
    {

    }

  }
}
