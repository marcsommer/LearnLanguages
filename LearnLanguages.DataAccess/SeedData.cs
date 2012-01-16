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
      InitializeUsers();
      InitializeLanguages();
      InitializePhrases();

    }

    #region Language Data
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
    #endregion

    #region Phrase Data
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
    #endregion

    #region User Data
    public static Guid TestValidUserId = new Guid("D719AED8-A7E2-4A74-ABFD-4D78B328F491");
    public static string TestValidUsername = "user";
    public static string TestValidPassword = "password";
    public static string TestSaltedHashedPassword = @"瞌訖ꎚ壿喐ຯ缟㕧";
    public static int TestSalt = -54623530;
    public static string TestInvalidUsername = "ImNotAValidUser";
    public static string TestInvalidPassword = "ImNotAValidPassword";
    #endregion

    #region Role Data
    public static Guid TestRoleId = new Guid("4E7DACEC-2EE7-4201-8657-694D51AA0487");
    public static RoleDto TestRole = new RoleDto()
    {
      Id = TestRoleId,
      Text = DalResources.RoleAdmin
    };
    #endregion

    public static List<PhraseDto> Phrases { get; private set; }
    public static List<LanguageDto> Languages { get; private set; }
    public static List<UserDto> Users { get; private set; }
    public static List<RoleDto> Roles { get; private set; }

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
    private static void InitializeUsers()
    {
      Users = new List<UserDto>()
      {
        new UserDto() 
        {
          Id = TestValidUserId,
          Username = TestValidUsername,
          Salt = TestSalt,
          SaltedHashedPasswordValue = TestSaltedHashedPassword
        }
      };
    }
    private static void InitializeRoles()
    {
      Roles = new List<RoleDto>()
      {
        TestRole
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
