using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.EFCodeFirst
{
  [Serializable]
  public class DbContextManagerException : Exception
  {
    public DbContextManagerException()
      :base()
    {

    }

    public DbContextManagerException(string msg)
      : base(msg)
    {

    }
  }
}
