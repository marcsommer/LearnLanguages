using System;
using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;
using System.ComponentModel;
using LearnLanguages.Business.Security;

namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseList : Common.CslaBases.BusinessListBase<PhraseList, PhraseEdit, PhraseDto>
  {
    #region Factory Methods

    public static void GetAll(EventHandler<DataPortalResult<PhraseList>> callback)
    {
      DataPortal.BeginFetch<PhraseList>(callback);
    }

    public static void NewPhraseList(ICollection<Guid> phraseIds, EventHandler<DataPortalResult<PhraseList>> callback)
    {
      DataPortal.BeginFetch<PhraseList>(phraseIds, callback);
    }

    public static void NewPhraseList(Criteria.PhraseTextsCriteria phraseTextsCriteria,
      EventHandler<DataPortalResult<PhraseList>> callback)
    {
      DataPortal.BeginCreate<PhraseList>(phraseTextsCriteria, callback);
    }

    public static PhraseList NewPhraseList()
    {
      return new PhraseList();
    }

#if SILVERLIGHT
    /// <summary>
    /// Runs locally.
    /// </summary>
    /// <param name="callback"></param>
    public static void NewPhraseList(EventHandler<DataPortalResult<PhraseList>> callback)
    {
      DataPortal.BeginCreate<PhraseList>(callback, DataPortal.ProxyModes.LocalOnly);
    }
#else
    /// <summary>
    /// Runs locally.
    /// </summary>
    [RunLocal]
    public static void NewPhraseList(EventHandler<DataPortalResult<PhraseList>> callback)
    {
      DataPortal.BeginCreate<PhraseList>(callback);
    }

    public static PhraseList GetAll()
    {
      return DataPortal.Fetch<PhraseList>();
    }
#endif

    #endregion

    #region Data Portal methods (including child)

#if !SILVERLIGHT
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Create(Criteria.PhraseTextsCriteria phraseTextsCriteria)
    {
      if (phraseTextsCriteria.PhraseTexts.Count == 0)
        throw new ArgumentException("phraseTextsCriteria");
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageText = phraseTextsCriteria.LanguageText;
        //var languageDal = dalManager.GetProvider<ILanguageDal>();
        //var languageResult = languageDal.Fetch(languageText);
        var language = DataPortal.FetchChild<LanguageEdit>(languageText);
        //LanguageEdit language = null;
        //if (!languageResult.IsSuccess)
        //{
        //  var exception = languageResult.GetExceptionFromInfo();
        //  if (exception == null)
        //    throw new LanguageTextNotFoundException(languageText);
        //  else if (exception.Message.Contains(LanguageTextNotFoundException.GetDefaultErrorMessage(languageText)))
        //  {
        //    //IF THE LANGUAGE TEXT DOES NOT EXIST, CREATE IT
        //    //CAN'T CHECK FOR DATA EXCEPTION TYPE BECAUSE IT WILL BE A GENERAL CSLA DATAPORTAL EXCEPTION
        //    //language = LanguageEdit.NewLanguageEdit();
        //    language = DataPortal.CreateChild<LanguageEdit>();
        //    language.Text = languageText;
        //    //language = language.Save();
        //  }
        //  else
        //    throw new CreateFailedException(DataAccess.DalResources.ErrorMsgLanguageTextProblemWhileCreatingPhraseTexts);
        //}

        //var languageDto = languageResult.Obj;
        //language = LanguageEdit.NewLanguageEdit(languageDto);
        //language.LoadFromDtoBypassPropertyChecks(languageDto);

        //WE NOW HAVE OUR LANGUAGEEDIT THAT WILL BE USED FOR ALL PHRASE TEXTS.
        var PhraseDal = dalManager.GetProvider<IPhraseDal>();

        //PhraseList newPhraseList = PhraseList.NewPhraseList();
        foreach (var phraseText in phraseTextsCriteria.PhraseTexts)
        {
          if (string.IsNullOrEmpty(phraseText))
            continue;
          PhraseEdit phraseEdit = DataPortal.CreateChild<PhraseEdit>();
          phraseEdit.Language = language;
          phraseEdit.Text = phraseText;
          Add(phraseEdit);
        }
      }
    }

    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(ICollection<Guid> phraseIds)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var PhraseDal = dalManager.GetProvider<IPhraseDal>();

        Result<ICollection<PhraseDto>> result = PhraseDal.Fetch(phraseIds);
        if (!result.IsSuccess || result.IsError)
        {
          if (result.Info != null)
          {
            var ex = result.GetExceptionFromInfo();
            if (ex != null)
              throw new FetchFailedException(ex.Message);
            else
              throw new FetchFailedException();
          }
          else
            throw new FetchFailedException();
        }

        //RESULT WAS SUCCESSFUL
        var fetchedPhraseDtos = result.Obj;
        foreach (var phraseDto in fetchedPhraseDtos)
        {
          //var PhraseEdit = DataPortal.CreateChild<PhraseEdit>(PhraseDto);
          var phraseEdit = DataPortal.FetchChild<PhraseEdit>(phraseDto);
          this.Add(phraseEdit);
        }
      }
    }

    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var PhraseDal = dalManager.GetProvider<IPhraseDal>();

        Result<ICollection<PhraseDto>> result = PhraseDal.GetAll();
        if (!result.IsSuccess || result.IsError)
        {
          if (result.Info != null)
          {
            var ex = result.GetExceptionFromInfo();
            if (ex != null)
              throw new FetchFailedException(ex.Message);
            else
              throw new FetchFailedException();
          }
          else
            throw new FetchFailedException();
        }

        //RESULT WAS SUCCESSFUL
        var allPhraseDtos = result.Obj;
        foreach (var PhraseDto in allPhraseDtos)
        {
          //var PhraseEdit = DataPortal.CreateChild<PhraseEdit>(PhraseDto);
          var PhraseEdit = DataPortal.FetchChild<PhraseEdit>(PhraseDto);
          this.Add(PhraseEdit);
        }
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        base.Child_Update();
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Child_Fetch(ICollection<Guid> phraseIds)
    {
      Items.Clear();
      foreach (var id in phraseIds)
      {
        var phraseEdit = DataPortal.FetchChild<PhraseEdit>(id);
        Items.Add(phraseEdit);
      }
    }
#endif

    #endregion

    #region AddNewCore

#if SILVERLIGHT
    protected override void AddNewCore()
    {
      AddedNew += PhraseList_AddedNew; 
      base.AddNewCore();
      AddedNew -= PhraseList_AddedNew;
    }

    private void PhraseList_AddedNew(object sender, Csla.Core.AddedNewEventArgs<PhraseEdit> e)
    {
      //CustomIdentity.CheckAuthentication();
      var phraseEdit = e.NewObject;
      phraseEdit.LoadCurrentUser();
      //var identity = (CustomIdentity)Csla.ApplicationContext.User.Identity;
      //phraseEdit.UserId = identity.UserId;
      //phraseEdit.Username = identity.Name;
    }
#else
    protected override PhraseEdit AddNewCore()
    {
      //SERVER
      var phraseEdit = base.AddNewCore();
      phraseEdit.LoadCurrentUser();
      return phraseEdit;
    }
#endif

    #endregion

    protected override void OnChildChanged(Csla.Core.ChildChangedEventArgs e)
    {
      base.OnChildChanged(e);
      //if (e.ChildObject != null)
      //  (Csla.Core.BusinessBase)e.ChildObject.BusinessRules.CheckRules();
    }
  }
}
