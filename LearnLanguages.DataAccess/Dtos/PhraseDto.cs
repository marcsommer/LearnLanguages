using System;

namespace LearnLanguages.DataAccess
{
  public class PhraseDto
  {
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid LanguageId  { get; set; }
  }
}
