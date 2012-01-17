using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Csla.Data;

namespace LearnLanguages.DataAccess.Ef
{
  public static class LearnLanguagesContextManager
  {
    static LearnLanguagesContextManager()
    {
      Initialized = false;
      Initializing = false;
      Initialize();
    }

    public static bool Initialized
    {
      get
      {
        return (bool)Csla.ApplicationContext.GlobalContext[EfResources.Key_ContextManagerInitialized];
      }
      private set
      {
        Csla.ApplicationContext.GlobalContext[EfResources.Key_ContextManagerInitialized] = value;
      }
    }

    public static bool Initializing
    {
      get
      {
        return (bool)Csla.ApplicationContext.LocalContext[EfResources.Key_ContextManagerInitializing];
      }
      private set
      {
        Csla.ApplicationContext.LocalContext[EfResources.Key_ContextManagerInitializing] = value;
      }
    }

    public static void Initialize()
    {
      var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
      var isBackground = System.Threading.Thread.CurrentThread.IsBackground;
      var isPool = System.Threading.Thread.CurrentThread.IsThreadPoolThread;
      if (Initialized || Initializing)
        return;
      Initializing = true;
      try
      {
        using (LearnLanguagesContext context = new LearnLanguagesContext())
        {
          if (context.DatabaseExists() && bool.Parse(EfResources.DeleteAllExistingDataAndStartNewSeedData))
            context.DeleteDatabase();

          if (!context.DatabaseExists())
          {
            context.CreateDatabase();
            context.Connection.Open();
            try
            {
              SeedContext(context);
              context.SaveChanges();
            }
            finally
            {
              context.Connection.Close();
            }
          }
        }
        Initialized = true;
      }
      finally
      {
        Initializing = false;
      }
    }

    private static void SeedContext(LearnLanguagesContext context)
    {
      SeedData.InitializeData();

      //LANGUAGES
      foreach (var langDto in SeedData.Languages)
      {
        var data = EfHelper.ToData(langDto);
        context.LanguageDatas.AddObject(data);
        context.SaveChanges();

        //UPDATE SEED DATA PHRASES WITH NEW LANGUAGE ID
        var affectedPhrases = (from phraseDto in SeedData.Phrases
                               where phraseDto.LanguageId == langDto.Id
                               select phraseDto).ToList();
        foreach (var phraseDto in affectedPhrases)
        {
          phraseDto.LanguageId = data.Id;//new Id
        }
        langDto.Id = data.Id;
      }

      //PHRASES
      foreach (var phraseDto in SeedData.Phrases)
      {
        var data = EfHelper.ToData(phraseDto);
        context.PhraseDatas.AddObject(data);
        context.SaveChanges();
        phraseDto.Id = data.Id;
      }

      //USERS
      foreach (var userDto in SeedData.Users)
      {
        var data = EfHelper.ToData(userDto);
      }
    }

    public static ObjectContextManager<LearnLanguagesContext> GetManager()
    {
      //hack: Check for Initialize LLContextManager in GetManager() method.  I had this in static constructor, but it was not getting called on the first call to this static class _every_ time the app started!?!  It would work, and then it seemed to start glitching and would completely miss the ctor with absolutely zero change in code.  Compiler optimization maybe?  I dunno.  So, now I have it check _every_ time we're getting a manager, which shouldn't be how it is.  I'll try to troubleshoot the static constructor problem later, hopefully it won't repro and I'll just put it back in the static ctor.
      if (!Initialized)
        Initialize();

      return ObjectContextManager<LearnLanguagesContext>.GetManager(EfResources.LearnLanguagesConnectionStringKey);
    }

    public static void Dispose()
    {
      throw new NotImplementedException();
    }
  }
}
