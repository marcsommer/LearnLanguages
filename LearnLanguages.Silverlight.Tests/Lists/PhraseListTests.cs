using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;
using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Linq;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("plist")]
  public class PhraseListTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    private LanguageEdit _ServerEnglishLang;
    private LanguageEdit _ServerSpanishLang;
    
    [TestInitialize]
    [Asynchronous]
    public void InitializePhraseTests()
    {

      //WE NEED TO UPDATE THE CLIENT SEEDDATA.INSTANCE IDS.  
      var isLoaded = false;
      var phrasesCorrected = false;
      Exception error = new Exception();
      Exception errorPhraseList = new Exception();
      LanguageList allLanguages = null;
      PhraseList allPhrases = null;

      LanguageList.GetAll((s, r) =>
      {
        #region Initialize Language Data
        error = r.Error;
        if (error != null)
          throw error;

        allLanguages = r.Object;
        _ServerEnglishLang = (from language in allLanguages
                              where language.Text == SeedData.Instance.EnglishText
                              select language).First();

        SeedData.Instance.EnglishLanguageDto.Id = _ServerEnglishLang.Id;

        _ServerSpanishLang = (from language in allLanguages
                              where language.Text == SeedData.Instance.SpanishText
                              select language).First();

        SeedData.Instance.SpanishLanguageDto.Id = _ServerSpanishLang.Id;

        #endregion

        isLoaded = true;

        PhraseList.GetAll((s2, r2) =>
        {
          errorPhraseList = r2.Error;
          if (errorPhraseList != null)
            throw errorPhraseList;

          allPhrases = r2.Object;

          var serverHelloPhraseQuery = (from phrase in allPhrases
                                        where phrase.Text == SeedData.Instance.HelloText &&
                                              phrase.Language.Text == SeedData.Instance.EnglishText
                                        select phrase);
          PhraseEdit serverHelloPhrase = null;
          if (serverHelloPhraseQuery.Count() == 0) //we don't have the hello phrase in the db, so put it there
          {
            var phrase = allPhrases[0];
            phrase.BeginEdit();
            phrase.Text = SeedData.Instance.HelloText;
            phrase.Language = _ServerEnglishLang;
            phrase.ApplyEdit();
            serverHelloPhrase = phrase;
          }
          else
            serverHelloPhrase = serverHelloPhraseQuery.First();


          var serverHolaPhraseQuery = (from phrase in allPhrases
                                       where phrase.Text == SeedData.Instance.HolaText &&
                                             phrase.Language.Text == SeedData.Instance.EnglishText
                                       select phrase);
          PhraseEdit serverHolaPhrase = null;
          if (serverHolaPhraseQuery.Count() == 0) //we don't have the Hola phrase in the db, so put it there
          {
            var phrase = allPhrases[1];
            phrase.BeginEdit();
            phrase.Text = SeedData.Instance.HolaText;
            phrase.Language = _ServerSpanishLang;
            phrase.ApplyEdit();
            serverHolaPhrase = phrase;
          }
          else
            serverHolaPhrase = serverHolaPhraseQuery.First();

          var serverLongPhraseQuery = (from phrase in allPhrases
                                       where phrase.Text == SeedData.Instance.LongText &&
                                             phrase.Language.Text == SeedData.Instance.EnglishText
                                       select phrase);
          PhraseEdit serverLongPhrase = null;
          if (serverLongPhraseQuery.Count() == 0) //we don't have the Long phrase in the db, so put it there
          {
            var phrase = allPhrases[2];
            phrase.BeginEdit();
            phrase.Text = SeedData.Instance.LongText;
            phrase.Language = _ServerEnglishLang;
            phrase.ApplyEdit();
            serverLongPhrase = phrase;
          }
          else
            serverLongPhrase = serverLongPhraseQuery.First();


          var serverDogPhraseQuery = (from phrase in allPhrases
                                      where phrase.Text == SeedData.Instance.DogText &&
                                            phrase.Language.Text == SeedData.Instance.EnglishText
                                      select phrase);
          PhraseEdit serverDogPhrase = null;
          if (serverDogPhraseQuery.Count() == 0) //we don't have the Dog phrase in the db, so put it there
          {
            var phrase = allPhrases[3];
            phrase.BeginEdit();
            phrase.Text = SeedData.Instance.DogText;
            phrase.Language = _ServerSpanishLang;
            phrase.ApplyEdit();
            serverDogPhrase = phrase;
          }
          else
            serverDogPhrase = serverDogPhraseQuery.First();

          var validUserId = serverHelloPhrase.UserId;
          SeedData.Instance.GetTestValidUserDto().Id = validUserId;

          SeedData.Instance.HelloPhraseDto.Id = serverHelloPhrase.Id;
          SeedData.Instance.HolaPhraseDto.Id = serverHolaPhrase.Id;
          SeedData.Instance.LongPhraseDto.Id = serverLongPhrase.Id;
          SeedData.Instance.DogPhraseDto.Id = serverDogPhrase.Id;

          SeedData.Instance.HelloPhraseDto.UserId = serverHelloPhrase.UserId;
          SeedData.Instance.HolaPhraseDto.UserId = serverHolaPhrase.UserId;
          SeedData.Instance.LongPhraseDto.UserId = serverLongPhrase.UserId;
          SeedData.Instance.DogPhraseDto.UserId = serverDogPhrase.UserId;

          phrasesCorrected = true;
        });
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => phrasesCorrected);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNotNull(allLanguages); },
                      () => { Assert.AreNotEqual(Guid.Empty, SeedData.Instance.EnglishId); },
                      () => { Assert.AreNotEqual(Guid.Empty, SeedData.Instance.SpanishId); },
                      () => { Assert.IsTrue(allLanguages.Count > 0); });
      EnqueueTestComplete();
    }
    
    [TestMethod]
    [Asynchronous]
    public void GET_ALL()
    {
      var isLoaded = false;
      Exception error = null;
      PhraseList allPhrases = null;
      PhraseList.GetAll((s, r) =>
      {
        error = r.Error;
        if (error != null)
          throw error;

        allPhrases = r.Object;
        isLoaded = true;
      });

      EnqueueConditional(() => isLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNotNull(allPhrases); },
                      () => { Assert.IsTrue(allPhrases.Count > 0); });
      EnqueueTestComplete();
    }
    
    [TestMethod]
    [Asynchronous]
    public void GET_ALL_EDIT_SAVE()
    {
      var isLoaded = false;
      var isEdited = false;
      var isSaved = false;

      var loadError = new Exception();
      var saveError = new Exception();

      var allPhrasesCount = -1;
      bool phrasesCountStaysTheSame = false;
      var text0 = "This is edited Text00000._save";
      var text1 = "This is edited Text11111._save";
      var text2 = "This is edited Text22222222222222222222222._save";
      var containsText0 = false;
      var containsText1 = false;
      var containsText2 = false;  

      PhraseList allPhrases = null;
      PhraseList savedPhrases = null;
      PhraseList.GetAll((s, r) =>
      {
        loadError = r.Error;
        if (loadError != null)
          throw loadError;

        allPhrases = r.Object;
        isLoaded = true;
        allPhrases[0].Text = text0;
        allPhrases[1].Text = text1;
        allPhrases[2].Text = text2;

        allPhrasesCount = allPhrases.Count;

        isEdited = true;
        allPhrases.BeginSave((s2, r2) =>
          {
            saveError = r2.Error;
            if (saveError != null)
              throw saveError;

            savedPhrases = (PhraseList)r2.NewObject;
            phrasesCountStaysTheSame = allPhrasesCount == savedPhrases.Count;

            containsText0 = (from phrase in savedPhrases
                             where phrase.Text == text0
                             select phrase).Count() == 1;
            containsText1 = (from phrase in savedPhrases
                             where phrase.Text == text1
                             select phrase).Count() == 1;
            containsText2 = (from phrase in savedPhrases
                             where phrase.Text == text2
                             select phrase).Count() == 1;
            isSaved = true;
          });
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueCallback(
                      () => { Assert.IsNull(loadError); },
                      () => { Assert.IsNull(saveError); },
                      () => { Assert.IsNotNull(allPhrases); },
                      () => { Assert.IsNotNull(savedPhrases); },
                      () => { Assert.IsTrue(containsText0); },
                      () => { Assert.IsTrue(containsText1); },
                      () => { Assert.IsTrue(containsText2); },
                      () => { Assert.IsTrue(phrasesCountStaysTheSame); },
                      () => { Assert.IsTrue(allPhrases.Count > 0); });
      EnqueueTestComplete();

    }

    [TestMethod]
    [Asynchronous]
    public void GET_ALL_EDIT_CANCELEDIT()
    {
      var isLoaded = false;
      var isEdited = false;
      var isCanceled = false;

      var loadError = new Exception();

      var text0 = "This is edited Text00000._canceledit";
      var text1 = "This is edited Text11111._canceledit";
      var text2 = "This is edited Text22222222222222222222222._canceledit";
      var containsText0 = false;
      var containsText1 = false;
      var containsText2 = false;

      PhraseList allPhrases = null;
      PhraseList canceledEditPhrases = null;
      PhraseList.GetAll((s, r) =>
      {
        loadError = r.Error;
        if (loadError != null)
          throw loadError;

        allPhrases = r.Object;
        isLoaded = true;
        //allPhrases.BeginEdit();
        //allPhrases.CancelEdit();
        allPhrases.BeginEdit();
        allPhrases[0].Text = text0;
        allPhrases[1].Text = text1;
        allPhrases[2].Text = text2;
        allPhrases.CancelEdit();

        isEdited = true;
        
        canceledEditPhrases = allPhrases;

        containsText0 = (from phrase in canceledEditPhrases
                         where phrase.Text == text0
                         select phrase).Count() == 1;
        containsText1 = (from phrase in canceledEditPhrases
                         where phrase.Text == text1
                         select phrase).Count() == 1;
        containsText2 = (from phrase in canceledEditPhrases
                         where phrase.Text == text2
                         select phrase).Count() == 1;
        isCanceled = true;
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isCanceled);
      EnqueueCallback(
                      () => { Assert.IsNull(loadError); },
                      () => { Assert.IsNotNull(allPhrases); },
                      () => { Assert.IsNotNull(canceledEditPhrases); },
                      () => { Assert.IsFalse(containsText0); },
                      () => { Assert.IsFalse(containsText1); },
                      () => { Assert.IsFalse(containsText2); },
                      () => { Assert.IsTrue(allPhrases.Count > 0); });
      EnqueueTestComplete();

    }

    [TestMethod]
    [Asynchronous]
    [Tag("plang")]
    public void GET_ALL_SET_NEW_LANGUAGE_SAVE()
    {
      var isLoaded = false;
      var isEdited = false;
      var isSaved = false;

      var loadError = new Exception();
      var saveError = new Exception();

      var allPhrasesCount = -1;
      bool phrasesCountStaysTheSame = false;
      LanguageEdit initLang0 = null;
      LanguageEdit initLang1 = null;
      LanguageEdit initLang2 = null;

      PhraseList allPhrases = null;
      PhraseList savedPhrases = null;
      PhraseList.GetAll((s, r) =>
      {
        loadError = r.Error;
        if (loadError != null)
          throw loadError;

        allPhrases = r.Object;
        isLoaded = true;

        allPhrasesCount = allPhrases.Count;
        initLang0 = allPhrases[0].Language;
        initLang1 = allPhrases[1].Language;
        initLang2 = allPhrases[2].Language;

        //this is fine
        allPhrases.BeginEdit();
        allPhrases.ApplyEdit();

        //just checking to see if removing from phraselist and apply edit deletes phrase from db.  it does.
        //PhraseEdit.GetPhraseEdit(SeedData.Instance.IdHello, (s3, r3) =>
        //{
        //  if (r3.Error != null)
        //    throw r3.Error;
        //});
        //allPhrases.BeginEdit(); //this does delete the item.
        //allPhrases.RemoveAt(3);
        //allPhrases.ApplyEdit();

        allPhrases[0].BeginEdit();
        allPhrases[0].Language = _ServerSpanishLang;
        allPhrases[0].ApplyEdit();

        allPhrases[1].BeginEdit();
        allPhrases[1].Language = _ServerSpanishLang;
        allPhrases[1].ApplyEdit();

        allPhrases[2].BeginEdit();
        allPhrases[2].Language = _ServerSpanishLang;
        allPhrases[2].ApplyEdit();

        isEdited = true;
        //allPhrases.ApplyEdit(); //just because we are 
        allPhrases.BeginSave((s2, r2) =>
        {
          saveError = r2.Error;
          if (saveError != null)
            throw saveError;

          //just checking to see if removing and applyingedit to allPhrases list deletes the item from db.  it looks like it does.
          //PhraseEdit.GetPhraseEdit(SeedData.Instance.IdHello, (s3, r3) =>
          //{
          //  if (r3.Error != null)
          //    throw r3.Error;
          //});
          savedPhrases = (PhraseList)r2.NewObject;
          phrasesCountStaysTheSame = allPhrasesCount == savedPhrases.Count;

          isSaved = true;
        });
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueCallback(
                      () => { Assert.IsNull(loadError); },
                      () => { Assert.IsNull(saveError); },
                      () => { Assert.AreEqual(_ServerSpanishLang.Id, savedPhrases[0].LanguageId); },
                      () => { Assert.AreEqual(_ServerSpanishLang.Id, savedPhrases[1].LanguageId); },
                      () => { Assert.AreEqual(_ServerSpanishLang.Id, savedPhrases[2].LanguageId); },
                      () => { Assert.IsNotNull(allPhrases); },
                      () => { Assert.IsNotNull(savedPhrases); },
                      () => { Assert.IsTrue(phrasesCountStaysTheSame); },
                      () => { Assert.IsTrue(allPhrases.Count > 0); });
      EnqueueTestComplete();

    }

    [TestMethod]
    [Asynchronous]
    [Tag("plang")]
    public void GET_ALL_SET_NEW_LANGUAGE_CANCELEDIT()
    {
      #region Init vars
      var isLoaded = false;
      var isEdited = false;
      var isCanceled = false;

      var loadError = new Exception();

      var allPhrasesCount = -1;
      bool phrasesCountStaysTheSame = false;
      LanguageEdit initLang0 = null;
      LanguageEdit initLang1 = null;
      LanguageEdit initLang2 = null;

      PhraseList allPhrases = null;
      PhraseList canceledEditPhrases = null;
      #endregion

      PhraseList.GetAll((s, r) =>
      {
        loadError = r.Error;
        if (loadError != null)
          throw loadError;

        allPhrases = r.Object;
        
        isLoaded = true;

        allPhrasesCount = allPhrases.Count;
        initLang0 = allPhrases[0].Language;
        initLang1 = allPhrases[1].Language;
        initLang2 = allPhrases[2].Language;

        //allPhrases.BeginEdit();
        allPhrases[0].BeginEdit();
        allPhrases[0].Language = _ServerSpanishLang;
        //allPhrases[0].ApplyEdit();
        //allPhrases[0].ApplyEdit();
        allPhrases[0].CancelEdit();
        //allPhrases.CancelEdit();
        //allPhrases[2].BeginEdit();
        //allPhrases[2].Language = _ServerSpanishLang;
        //allPhrases[2].ApplyEdit();
        //allPhrases[3].BeginEdit();
        //allPhrases[3].Language = _ServerSpanishLang;
        //allPhrases[3].ApplyEdit();
        isEdited = true;

        //allPhrases.CancelEdit();
        canceledEditPhrases = allPhrases;
        phrasesCountStaysTheSame = allPhrasesCount == canceledEditPhrases.Count;
        isCanceled = true;

        //allPhrases.BeginSave((s2, r2) =>
        //{
        //  saveError = r2.Error;
        //  if (saveError != null)
        //    throw saveError;

        //  savedPhrases = (PhraseList)r2.NewObject;
        //  phrasesCountStaysTheSame = allPhrasesCount == savedPhrases.Count;

        //  isSaved = true;
        //});
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isCanceled);
      EnqueueCallback(
                      () => { Assert.IsNull(loadError); },
                      () => { Assert.AreEqual(initLang0.Id, canceledEditPhrases[0].LanguageId); },
                      () => { Assert.AreEqual(initLang1.Id, canceledEditPhrases[1].LanguageId); },
                      () => { Assert.AreEqual(initLang2.Id, canceledEditPhrases[2].LanguageId); },
                      () => { Assert.IsNotNull(allPhrases); },
                      () => { Assert.IsNotNull(canceledEditPhrases); },
                      () => { Assert.IsTrue(phrasesCountStaysTheSame); },
                      () => { Assert.IsTrue(allPhrases.Count > 0); });
      EnqueueTestComplete();

    }

    [TestMethod]
    [Asynchronous]
    [Tag("plang")]
    public void GET_ALL_SET_NEW_LANGUAGE_SAVE_GET_CONFIRMNEWLANGUAGE()
    {
      #region Init vars
      var isLoaded = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;
      var isConfirmed = false;

      var allPhrasesCount = -1;
      PhraseEdit testPhrase = null;
      PhraseEdit savedPhrase = null;
      PhraseEdit gottenPhrase = null;

      PhraseList allPhrases = null;
      PhraseList savedAllPhrases = null;
      PhraseList confirmAllPhrases = null;
      #endregion

      //LOAD--------------
      PhraseList.GetAll((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        allPhrases = r.Object;
        isLoaded = true;
        allPhrasesCount = allPhrases.Count;

        testPhrase = (from phrase in allPhrases
                      where phrase.Language.Text == _ServerEnglishLang.Text
                      select phrase).First();

        //testPhrase is english

        //EDIT---------
        testPhrase.BeginEdit();
        testPhrase.Language = _ServerSpanishLang;
        testPhrase.ApplyEdit();
        isEdited = true;


        //SAVE-------------
        allPhrases.BeginSave((s2, r2) =>
          {
            if (r2.Error != null)
              throw r2.Error;

            savedAllPhrases = (PhraseList)r2.NewObject;

            savedPhrase = (from phrase in savedAllPhrases
                           where phrase.Text == testPhrase.Text
                           select phrase).First();

            Assert.AreEqual(_ServerSpanishLang.Text, savedPhrase.Language.Text);
            Assert.AreEqual(_ServerSpanishLang.Id, savedPhrase.Language.Id);
            Assert.AreEqual(_ServerSpanishLang.Id, savedPhrase.LanguageId);
            isSaved = true;

            //GET-----------
            PhraseEdit.GetPhraseEdit(savedPhrase.Id, (s3, r3) =>
              {
                if (r3.Error != null)
                  throw r3.Error;

                gottenPhrase = r3.Object;
                isGotten = true;


                //CONFIRM----------
                //confirm language swap worked.
                Assert.AreEqual(_ServerSpanishLang.Text, gottenPhrase.Language.Text);
                Assert.AreEqual(_ServerSpanishLang.Id, gottenPhrase.Language.Id);
                Assert.AreEqual(_ServerSpanishLang.Id, gottenPhrase.LanguageId);
                //confirm we haven't glitched our data and magically doubled the size of our allPhrases.
                PhraseList.GetAll((s4, r4) =>
                  {
                    if (r4.Error != null)
                      throw r4.Error;

                    confirmAllPhrases = r4.Object;
                    Assert.AreEqual(allPhrasesCount, confirmAllPhrases.Count);
                    isConfirmed = true;
                  });
              });
          });
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueConditional(() => isConfirmed);
      EnqueueCallback(() => { Assert.IsNotNull(allPhrases); },
                      () => { Assert.IsNotNull(confirmAllPhrases); });
      EnqueueTestComplete();

    }

    [TestMethod]
    [Asynchronous]
    public void GET_ALL_CHECK_GLITCH()
    {
      var isLoaded = false;
      var isSaved = false;
      Exception error = null;
      PhraseList allPhrases = null;
      int allPhrasesCount = -1;
      PhraseList savedAllPhrases = null;
      int savedAllPhrasesCount = -2;
      PhraseList.GetAll((s, r) =>
      {
        error = r.Error;
        if (error != null)
          throw error;

        allPhrases = r.Object;
        allPhrasesCount = allPhrases.Count;
        isLoaded = true;

        allPhrases.BeginSave((s2, r2) =>
          {
            if (r2.Error != null)
              throw r2.Error;

            savedAllPhrases = (PhraseList)r2.NewObject;
            savedAllPhrasesCount = savedAllPhrases.Count;
            Assert.AreEqual(allPhrasesCount, savedAllPhrasesCount);
            isSaved = true;
          });
      });

      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => isSaved);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNotNull(allPhrases); },
                      () => { Assert.IsTrue(allPhrases.Count > 0); });
      EnqueueTestComplete();
    }
  }
}