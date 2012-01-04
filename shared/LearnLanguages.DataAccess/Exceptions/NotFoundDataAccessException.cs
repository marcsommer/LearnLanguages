using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class NotFoundDataAccessException : Exception
  {
    public NotFoundDataAccessException()
      : base()
    {

    }

    public NotFoundDataAccessException(string errorMsg)
      : base(errorMsg)
    {

    }
  }
}
