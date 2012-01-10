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
    private static object _InitializedLock = new object();
    private static bool _initialized = false;
    private static bool _Initialized
    {
      get
      {
        lock (_InitializedLock)
        {
          return _initialized;
        }
      }
      set
      {
        lock (_InitializedLock)
        {
          _initialized = value;
        }
      }
    }

    private static bool _InitializedSuccessfully = false;
    public static bool InitializedSuccessfully
    {
      get
      {
        return _InitializedSuccessfully;
      }
      private set
      {
        _InitializedSuccessfully = value;
      }
    }

    public static void Initialize()
    {
      _Initialized = true;
      
      using (LearnLanguagesContext context = new LearnLanguagesContext())
      {

        if (context.DatabaseExists() && bool.Parse(EfResources.DeleteAllExistingDataAndStartNewSeedData))
          context.DeleteDatabase();

        if (!context.DatabaseExists())
        {
          context.CreateDatabase();
          context.Connection.Open();
          SeedContext(context);
          context.SaveChanges();
        }
      }

      InitializedSuccessfully = true;
    }
    private static void SeedContext(LearnLanguagesContext context)
    {
      foreach (var langDto in SeedData.Languages)
      {
        var data = EfHelper.ToData(langDto);
        context.LanguageDatas.AddObject(data);
        context.SaveChanges();
        //update seeddata phrases with new language id

        var affectedPhrases = (from phraseDto in SeedData.Phrases
                               where phraseDto.LanguageId == langDto.Id
                               select phraseDto).ToList();
        foreach (var phraseDto in affectedPhrases)
        {
          phraseDto.LanguageId = data.Id;//new Id
        }

        langDto.Id = data.Id;
      }

      foreach (var phraseDto in SeedData.Phrases)
      {
        var data = EfHelper.ToData(phraseDto);
        context.PhraseDatas.AddObject(data);
        context.SaveChanges();
        phraseDto.Id = data.Id;
      }
    }

    public static ObjectContextManager<LearnLanguagesContext> GetManager()
    {
      //hack: Check for Initialize LLContextManager in GetManager() method.  I had this in static constructor, but it was not getting called on the first call to this static class _every_ time the app started!?!  It would work, and then it seemed to start glitching and would completely miss the ctor with absolutely zero change in code.  Compiler optimization maybe?  I dunno.  So, now I have it check _every_ time we're getting a manager, which shouldn't be how it is.  I'll try to troubleshoot the static constructor problem later, hopefully it won't repro and I'll just put it back in the static ctor.
      if (!InitializedSuccessfully)
        Initialize();

      return ObjectContextManager<LearnLanguagesContext>.GetManager(EfResources.LearnLanguagesConnectionStringKey);
    }
  }
}
