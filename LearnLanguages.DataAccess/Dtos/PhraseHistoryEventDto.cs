using System;
using Csla.Serialization;
using System.Collections.Generic;

namespace LearnLanguages.DataAccess
{
  [Serializable]
  public class PhraseHistoryEventDto
  {
    public Guid Id { get; set; }
    public Guid PhraseId { get; set; }
    public DateTime TimeStamp { get; set; }
    public TimeSpan Duration { get; set; }
    public string Text { get; set; }
    public Guid UserId { get; set; }      
    public string Username { get; set; }
  }
}
