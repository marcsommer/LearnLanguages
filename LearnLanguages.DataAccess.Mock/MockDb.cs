using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.Mock
{
  public static class MockDb
  {
    public static Guid DefaultLanguageId = Guid.Parse(DalResources.DefaultLanguageId);

    public static Guid EnglishId = Guid.Parse(DalResources.DefaultEnglishLanguageId);
    public static string EnglishText = DalResources.DefaultEnglishLanguageText;

    public static Guid SpanishId = new Guid("DA5AA804-E59F-4608-988E-59C7923BE383");
    public static string SpanishText = "Spanish";

    public static Guid IdHello = new Guid("00D626ED-3EAC-4729-B276-5B9368DA26AD");
    public static Guid IdLongPhrase = new Guid("32D6CB9A-CBFF-47AE-91BB-396FAA7A3506");
    public static Guid IdHola = new Guid("CBC5F6DE-3A46-4010-A67D-1E712EBFD3AB");
    public static Guid IdDog = new Guid("B45AC4A3-BAB3-406C-BD03-20D9E55E9740");

    public static List<PhraseDto> Phrases { get; private set; }
    public static List<LanguageDto> Languages { get; private set; }

    static MockDb()
    {
      InitializePhrases();
      InitializeLanguages();
    }

    private static void InitializeLanguages()
    {
      Languages = new List<LanguageDto>()
      {
        new LanguageDto()
        {
          Id = EnglishId,
          Text = EnglishText
        },

        new LanguageDto()
        {
          Id = SpanishId,
          Text = SpanishText
        }
      };
    }

    private static void InitializePhrases()
    {
      Phrases = new List<PhraseDto>()
      {
        new PhraseDto() 
        { 
          Id = IdHello,
          LanguageId = EnglishId,
          Text = "Hello!"
        },
          
        new PhraseDto() 
        { 
          Id = IdLongPhrase,
          LanguageId = EnglishId,
          Text = "Why this is a very long phrase indeed.  It is in fact several sentences.  I think it might just be TOO long!"
        },
        
        new PhraseDto() 
        { 
          Id = IdHola,
          LanguageId = SpanishId,
          Text = "Hola!"
        },

        new PhraseDto()
        {
          Id = IdDog,
          LanguageId = EnglishId,
          Text = "dog"
        }
      };
    }

    public static bool ContainsLanguageId(Guid id)
    {
      var results = from l in Languages
                    where l.Id == id
                    select l;

      return results.Count() == 1;
    }
  }
}
