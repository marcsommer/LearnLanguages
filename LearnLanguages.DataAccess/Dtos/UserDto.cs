using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess
{
  [Serializable]
  public class UserDto
  {
    public Guid Id { get; set; }
    public string Username { get; set; }
    public int Salt { get; set; }
    public string SaltedHashedPasswordValue { get; set; }
  }
}
