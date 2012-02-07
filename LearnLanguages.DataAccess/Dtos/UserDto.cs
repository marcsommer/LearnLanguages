using System;
using Csla.Serialization;
using System.Collections.Generic;

namespace LearnLanguages.DataAccess
{
  [Serializable]
  public class UserDto
  {
    public Guid Id { get; set; }
    public string Username { get; set; }
    public int Salt { get; set; }
    public string NativeLanguageText { get; set; }
    public string SaltedHashedPasswordValue { get; set; }
    public ICollection<Guid> TranslationIds { get; set; }
    public ICollection<Guid> PhraseIds { get; set; }
    public ICollection<Guid> LineIds { get; set; }
    public ICollection<Guid> RoleIds { get; set; }
  }
}
