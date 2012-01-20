using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class UserNotAuthenticatedException : Exception
  {
    public UserNotAuthenticatedException()
      : base(DalResources.ErrorMsgUserNotAuthenticatedException)
    {

    }

    public UserNotAuthenticatedException(string errorMsg)
      : base(errorMsg)
    {

    }

    public UserNotAuthenticatedException(Exception innerException)
      : base(DalResources.ErrorMsgUserNotAuthenticatedException, innerException)
    {

    }
  }
}
