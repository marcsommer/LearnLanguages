using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class UpdateFailedException : Exception
  {
    public UpdateFailedException()
      : base()
    {

    }

    public UpdateFailedException(string errorMsg)
      : base(errorMsg)
    {

    }
  }
}
