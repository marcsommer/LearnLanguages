using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public sealed class SeedData
  {
    private SeedData()
    {
      InitializeData();
    }

    #region Singleton Pattern Members
    private static volatile SeedData _Instance;
    private static object _Lock = new object();
    public static SeedData Instance
    {
      get
      {
        if (_Instance == null)
        {
          lock (_Lock)
          {
            if (_Instance == null)
              _Instance = new SeedData();
          }
        }

        return _Instance;
      }
    }
    #endregion
    
    private void InitializeData()
    {
      AdminRole = new RoleDto()
      {
        Id = AdminRoleId,
        Text = AdminRoleText
      };

      UserRole = new RoleDto()
      {
        Id = UserRoleId,
        Text = UserRoleText
      };

      InitializeLanguages();
      InitializePhrases();
      InitializeLines();
      InitializeTranslations();
      InitializeUsers();
      InitializeRoles();
      InitializeStudyDatas();
    }


    #region Language Data
    public Guid DefaultLanguageId = Guid.Parse(DalResources.DefaultLanguageId);
    public LanguageDto EnglishLanguageDto
    {
      get
      {
        return (from language in Languages
                where language.Text == EnglishText
                select language).First();
      }
    }
    public Guid EnglishId
    {
      get
      {
        return (from lang in Languages
                where lang.Text == EnglishText
                select lang.Id).First();
      }
    }
    public LanguageDto SpanishLanguageDto
    {
      get
      {
        return (from language in Languages
                where language.Text == SpanishText
                select language).First();
      }
    }
    public Guid SpanishId
    {
      get
      {
        return (from lang in Languages
                where lang.Text == SpanishText
                select lang.Id).First();
      }
    }
    public string EnglishText = DalResources.DefaultEnglishLanguageText;
    public string SpanishText = "Spanish";
    #endregion
    #region Phrase Data
    public PhraseDto HelloPhraseDto
    {
      get
      {
        return (from phrase in Phrases
                where phrase.Text == HelloText
                select phrase).First();
      }
    }
    public PhraseDto HolaPhraseDto
    {
      get
      {
        return (from phrase in Phrases
                where phrase.Text == HolaText
                select phrase).First();
      }
    }
    public PhraseDto LongPhraseDto
    {
      get
      {
        return (from phrase in Phrases
                where phrase.Text == LongText
                select phrase).First();
      }
    }
    public PhraseDto DogPhraseDto
    {
      get
      {
        return (from phrase in Phrases
                where phrase.Text == DogText
                select phrase).First();
      }
    }

    public string HelloText = "Hello!";
    public string LongText = "Why this is a very long phrase indeed.  It is in fact several sentences.  I think it might just be TOO long!";
    public string HolaText = "Hola!!!";
    public string DogText = "dog";

    public Guid IdHello
    {
      get { return GetPhraseId(HelloText); }
    }
    public Guid IdLongPhrase
    {
      get { return GetPhraseId(LongText); }
    }
    public Guid IdHola
    {
      get { return GetPhraseId(HolaText); }
    }
    public Guid IdDog
    {
      get { return GetPhraseId(DogText); }
    }
    #endregion
    #region Translation Data
    public TranslationDto HelloTranslationDto
    {
      get 
      {
        return new TranslationDto()
        {
          Id = IdHelloTranslation,
          PhraseIds = new List<Guid>()
          {
            IdHello, 
            IdHola
          },
          UserId = GetTestValidUserDto().Id,
          Username = TestValidUsername
        };
      }
    }
    public Guid IdHelloTranslation
    {
      get
      {
        return (from t in Translations
                where t.PhraseIds.Contains(IdHello)
                select t).First().Id;
      }
    }
    #endregion
    #region User Data
    public Guid DefaultTestValidUserId = new Guid("D719AED8-A7E2-4A74-ABFD-4D78B328F491");

    public UserDto GetTestValidUserDto()
    {
      return (from userDto in Users
              where userDto.Username == TestValidUsername
              select userDto).First();
    }
    public string TestValidUsername = "user";
    public string TestValidPassword = "password";
    public string TestSaltedHashedPassword = @"瞌訖ꎚ壿喐ຯ缟㕧";
    public int TestSalt = -54623530;
    public string TestInvalidUsername = "ImNotAValidUser";
    public string TestInvalidPassword = "ImNotAValidPassword";
    #endregion
    #region Role Data
    public Guid AdminRoleId = new Guid("4E7DACEC-2EE7-4201-8657-694D51AA0487");
    public string AdminRoleText = DalResources.RoleAdmin;
    public RoleDto AdminRole;

    public Guid UserRoleId = new Guid("BD8C1940-59FD-412A-B710-91BECCF7D469");
    public string UserRoleText = DalResources.RoleUser;
    public RoleDto UserRole;
    #endregion

    public List<LanguageDto> Languages { get; private set; }
    public List<PhraseDto> Phrases { get; private set; }
    public List<LineDto> Lines { get; private set; }
    public List<TranslationDto> Translations { get; private set; }
    public List<UserDto> Users { get; private set; }
    public List<RoleDto> Roles { get; private set; }
    public List<StudyDataDto> StudyDatas { get; private set; }

    private void InitializeLanguages()
    {
      Languages = new List<LanguageDto>()
      {
        new LanguageDto()
        {
          Id = Guid.Parse(DalResources.DefaultEnglishLanguageId),
          Text = EnglishText, 
          Username = TestValidUsername,
          UserId = DefaultTestValidUserId
        },

        new LanguageDto()
        {
          Id = new Guid("DA5AA804-E59F-4608-988E-59C7923BE383"),
          Text = SpanishText,
          Username = TestValidUsername,
          UserId = DefaultTestValidUserId
        }
      };
    }
    private void InitializePhrases()
    {
      Phrases = new List<PhraseDto>()
      {
        new PhraseDto() 
        { 
          Id = new Guid("00D626ED-3EAC-4729-B276-5B9368DA26AD"),
          LanguageId = EnglishId,
          Text = HelloText,
          UserId = DefaultTestValidUserId,
          Username = TestValidUsername
        },
          
        new PhraseDto() 
        { 
          Id = new Guid("32D6CB9A-CBFF-47AE-91BB-396FAA7A3506"),
          LanguageId = EnglishId,
          Text = LongText,
          UserId = DefaultTestValidUserId,
          Username = TestValidUsername
        },
        
        new PhraseDto() 
        { 
          Id = new Guid("CBC5F6DE-3A46-4010-A67D-1E712EBFD3AB"),
          LanguageId = SpanishId,
          Text = HolaText,
          UserId = DefaultTestValidUserId,
          Username = TestValidUsername
        },

        new PhraseDto()
        {
          Id = new Guid("B45AC4A3-BAB3-406C-BD03-20D9E55E9740"),
          LanguageId = EnglishId,
          Text = DogText,
          UserId = DefaultTestValidUserId,
          Username = TestValidUsername
        }
      };
    }
    private void InitializeLines()
    {
      Lines = new List<LineDto>()
      {
        new LineDto()
        {
          Id = new Guid("527A8025-25B1-4518-B448-1CD27A801ECE"),
          PhraseId = IdHello, 
          LineNumber = 0,
          UserId = DefaultTestValidUserId,
          Username = TestValidUsername
        },

        new LineDto()
        {
          Id = new Guid("40F83D40-CC12-4C14-A2DA-D82960C6DFBA"),
          PhraseId = IdLongPhrase, 
          LineNumber = 1,
          UserId = DefaultTestValidUserId,
          Username = TestValidUsername
        }
      };
    }
    private void InitializeTranslations()
    {
      Translations = new List<TranslationDto>()
      {
        new TranslationDto()
        {
          Id = new Guid("3E077DBA-8E71-4E64-9D30-68204F989242"),
          PhraseIds = new List<Guid>()
          {
            IdHello, 
            IdHola
          },
          UserId = DefaultTestValidUserId,
          Username = TestValidUsername
        }
      };
    }
    private void InitializeUsers()
    {
      Users = new List<UserDto>()
      {
        new UserDto() 
        {
          Id = DefaultTestValidUserId,
          Username = TestValidUsername,
          NativeLanguageText = EnglishText,
          Salt = TestSalt,
          SaltedHashedPasswordValue = TestSaltedHashedPassword,
          PhraseIds = new List<Guid>() { IdHello, IdDog, IdHola, IdLongPhrase },
          TranslationIds = new List<Guid>() { IdHelloTranslation },
          RoleIds = new List<Guid>() { AdminRoleId, UserRoleId }
        }
      };
    }
    private void InitializeRoles()
    {
      Roles = new List<RoleDto>()
      {
        AdminRole,
        UserRole
      };
    }
    private void InitializeStudyDatas()
    {
      var dto = new StudyDataDto()
      {
        Id = new Guid("A1F69CEE-05EC-49D2-AA58-A246A8371AC0"),
        NativeLanguageText = EnglishText,
        Username = TestValidUsername
      };
      StudyDatas = new List<StudyDataDto>()
      {
        dto
      };
    }

    public bool ContainsLanguageId(Guid id)
    {
      var results = from l in Languages
                    where l.Id == id
                    select l;

      return results.Count() == 1;
    }
    public bool ContainsUserId(Guid id)
    {
      var results = from u in Users
                    where u.Id == id
                    select u;

      return results.Count() == 1;
    }
    public Guid GetPhraseId(string phraseText)
    {
        return (from phrase in Phrases
                where phrase.Text == phraseText
                select phrase.Id).First();
    }

    public string GetUsername(Guid userId)
    {
      return (from u in Users
              where u.Id == userId
              select u.Username).FirstOrDefault();
    }


    public bool ContainsPhraseId(Guid id)
    {
      var results = from p in Phrases
                    where p.Id == id
                    select p;

      return results.Count() == 1;
    }
  }
}
