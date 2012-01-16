using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess
{
  [Serializable]
  public class PhraseDto
  {
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid LanguageId  { get; set; } //primary key
    public Guid UserId { get; set; }      //composite key
    public string Username { get; set; }  //composite key
  }
}
