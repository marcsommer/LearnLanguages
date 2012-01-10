using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public static class SeedData
  
  {
    static SeedData()
    {
      //INITIALIZE LANGUAGES FIRST!!
      InitializeLanguages();
      InitializePhrases();
    }

    public static Guid DefaultLanguageId = Guid.Parse(DalResources.DefaultLanguageId);
    public static Guid EnglishId
    {
      get
      {
        return (from lang in SeedData.Languages
                where lang.Text == EnglishText
                select lang.Id).First();
      }
    }
    public static Guid SpanishId
    {
      get
      {
        return (from lang in SeedData.Languages
                where lang.Text == SpanishText
                select lang.Id).First();
      }
    }
    public static string EnglishText = DalResources.DefaultEnglishLanguageText;
    public static string SpanishText = "Spanish";

    public static string HelloText = "Hello!";
    public static string LongPhraseText = "Why this is a very long phrase indeed.  It is in fact several sentences.  I think it might just be TOO long!";
    public static string HolaText = "Hola!!!";
    public static string DogText = "dog";

    public static Guid IdHello
    {
      get { return GetPhraseId(HelloText); }
    }
    public static Guid IdLongPhrase
    {
      get { return GetPhraseId(LongPhraseText); }
    }
    public static Guid IdHola
    {
      get { return GetPhraseId(HolaText); }
    }
    public static Guid IdDog
    {
      get { return GetPhraseId(DogText); }
    }
    

    public static List<PhraseDto> Phrases { get; private set; }
    public static List<LanguageDto> Languages { get; private set; }

    
    private static void InitializeLanguages()
    {
      Languages = new List<LanguageDto>()
      {
        new LanguageDto()
        {
          Id = Guid.Parse(DalResources.DefaultEnglishLanguageId),
          Text = EnglishText
        },

        new LanguageDto()
        {
          Id = new Guid("DA5AA804-E59F-4608-988E-59C7923BE383"),
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
          Id = new Guid("00D626ED-3EAC-4729-B276-5B9368DA26AD"),
          LanguageId = EnglishId,
          Text = HelloText
        },
          
        new PhraseDto() 
        { 
          Id = new Guid("32D6CB9A-CBFF-47AE-91BB-396FAA7A3506"),
          LanguageId = EnglishId,
          Text = LongPhraseText
        },
        
        new PhraseDto() 
        { 
          Id = new Guid("CBC5F6DE-3A46-4010-A67D-1E712EBFD3AB"),
          LanguageId = SpanishId,
          Text = HolaText
        },

        new PhraseDto()
        {
          Id = new Guid("B45AC4A3-BAB3-406C-BD03-20D9E55E9740"),
          LanguageId = EnglishId,
          Text = DogText
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
    public static Guid GetPhraseId(string phraseText)
    {
        return (from phrase in SeedData.Phrases
                where phrase.Text == phraseText
                select phrase.Id).First();
    }
  }
}
