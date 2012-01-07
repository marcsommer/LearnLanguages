using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.MockProvider
{
  public static class MockDb
  {
    private static Guid _EnglishId = new Guid("C10D85B4-4DE4-40CB-8ABD-9458AD9B7FD9");
    private static Guid _SpanishId = new Guid("DA5AA804-E59F-4608-988E-59C7923BE383");

    public static List<PhraseDto> Phrases { get; private set; }
    public static List<LanguageDto> Languages { get; private set; }

    static MockDb()
    {
      InitializePhrases(_EnglishId, _SpanishId);
    }

    private static void InitializePhrases(Guid englishId, Guid spanishId)
    {
      Phrases = new List<PhraseDto>()
      {
        new PhraseDto() 
        { 
          Id = new Guid("00D626ED-3EAC-4729-B276-5B9368DA26AD"),
          LanguageId = englishId,
          Text = "Hello!"
        },
          
        new PhraseDto() 
        { 
          Id = new Guid("32D6CB9A-CBFF-47AE-91BB-396FAA7A3506"),
          LanguageId = englishId,
          Text = "Why this is a very long phrase indeed.  It is in fact several sentences.  I think it might just be TOO long!"
        },
        
        new PhraseDto() 
        { 
          Id = new Guid("CBC5F6DE-3A46-4010-A67D-1E712EBFD3AB"),
          LanguageId = spanishId,
          Text = "Hola!"
        },

        new PhraseDto()
        {
          Id = new Guid("B45AC4A3-BAB3-406C-BD03-20D9E55E9740"),
          LanguageId = englishId,
          Text = "dog"
        }
      };
    }
  }
}
