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
  [Tag("translation")]
  public class TranslationTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    private LanguageEdit _ServerEnglishLang;
    private LanguageEdit _ServerSpanishLang;

    //[ClassInitialize]
    //[Asynchronous]
    //public void InitializeTranslationTests()
    //{

    //  //WE NEED TO UPDATE THE CLIENT SEEDDATA.INSTANCE IDS.  
    //  var isLoaded = false;
    //  var translationsCorrected = false;
    //  Exception error = new Exception();
    //  Exception errorTranslationList = new Exception();
    //  LanguageList allLanguages = null;
    //  TranslationList allTranslations = null;

    //  LanguageList.GetAll((s, r) =>
    //  {
    //    #region Initialize Language Data
    //    error = r.Error;
    //    if (error != null)
    //      throw error;

    //    allLanguages = r.Object;
    //    _ServerEnglishLang = (from language in allLanguages
    //                          where language.Text == SeedData.Instance.EnglishText
    //                          select language).First();

    //    SeedData.Instance.EnglishLanguageDto.Id = _ServerEnglishLang.Id;

    //    _ServerSpanishLang = (from language in allLanguages
    //                          where language.Text == SeedData.Instance.SpanishText
    //                          select language).First();

    //    SeedData.Instance.SpanishLanguageDto.Id = _ServerSpanishLang.Id;

    //    #endregion

    //    isLoaded = true;

    //    TranslationList.GetAll((s2, r2) =>
    //      {
    //        errorTranslationList = r2.Error;
    //        if (errorTranslationList != null)
    //          throw errorTranslationList;

    //        allTranslations = r2.Object;

    //        var serverHelloTranslationQuery = (from translation in allTranslations
    //                                           where translation.Text == SeedData.Instance.HelloText &&
    //                                                 translation.Language.Text == SeedData.Instance.EnglishText
    //                                           select translation);
    //        TranslationEdit serverHelloTranslation = null;
    //        if (serverHelloTranslationQuery.Count() == 0) //we don't have the hello translation in the db, so put it there
    //        {
    //          var translation = allTranslations[0];
    //          translation.BeginEdit();
    //          translation.Text = SeedData.Instance.HelloText;
    //          translation.Language = _ServerEnglishLang;
    //          translation.ApplyEdit();
    //          serverHelloTranslation = translation;
    //        }
    //        else
    //          serverHelloTranslation = serverHelloTranslationQuery.First();


    //        var serverHolaTranslationQuery = (from translation in allTranslations
    //                                          where translation.Text == SeedData.Instance.HolaText &&
    //                                                translation.Language.Text == SeedData.Instance.EnglishText
    //                                          select translation);
    //        TranslationEdit serverHolaTranslation = null;
    //        if (serverHolaTranslationQuery.Count() == 0) //we don't have the Hola translation in the db, so put it there
    //        {
    //          var translation = allTranslations[1];
    //          translation.BeginEdit();
    //          translation.Text = SeedData.Instance.HolaText;
    //          translation.Language = _ServerSpanishLang;
    //          translation.ApplyEdit();
    //          serverHolaTranslation = translation;
    //        }
    //        else
    //          serverHolaTranslation = serverHolaTranslationQuery.First();

    //        var serverLongTranslationQuery = (from translation in allTranslations
    //                                          where translation.Text == SeedData.Instance.LongText &&
    //                                                translation.Language.Text == SeedData.Instance.EnglishText
    //                                          select translation);
    //        TranslationEdit serverLongTranslation = null;
    //        if (serverLongTranslationQuery.Count() == 0) //we don't have the Long translation in the db, so put it there
    //        {
    //          var translation = allTranslations[2];
    //          translation.BeginEdit();
    //          translation.Text = SeedData.Instance.LongText;
    //          translation.Language = _ServerEnglishLang;
    //          translation.ApplyEdit();
    //          serverLongTranslation = translation;
    //        }
    //        else
    //          serverLongTranslation = serverLongTranslationQuery.First();


    //        var serverDogTranslationQuery = (from translation in allTranslations
    //                                         where translation.Text == SeedData.Instance.DogText &&
    //                                               translation.Language.Text == SeedData.Instance.EnglishText
    //                                         select translation);
    //        TranslationEdit serverDogTranslation = null;
    //        if (serverDogTranslationQuery.Count() == 0) //we don't have the Dog translation in the db, so put it there
    //        {
    //          var translation = allTranslations[3];
    //          translation.BeginEdit();
    //          translation.Text = SeedData.Instance.DogText;
    //          translation.Language = _ServerSpanishLang;
    //          translation.ApplyEdit();
    //          serverDogTranslation = translation;
    //        }
    //        else
    //          serverDogTranslation = serverDogTranslationQuery.First();

    //        var validUserId = serverHelloTranslation.UserId;
    //        SeedData.Instance.GetTestValidUserDto().Id = validUserId;

    //        SeedData.Instance.HelloTranslationDto.Id = serverHelloTranslation.Id;
    //        SeedData.Instance.HolaTranslationDto.Id = serverHolaTranslation.Id;
    //        SeedData.Instance.LongTranslationDto.Id = serverLongTranslation.Id;
    //        SeedData.Instance.DogTranslationDto.Id = serverDogTranslation.Id;

    //        SeedData.Instance.HelloTranslationDto.UserId = serverHelloTranslation.UserId;
    //        SeedData.Instance.HolaTranslationDto.UserId = serverHolaTranslation.UserId;
    //        SeedData.Instance.LongTranslationDto.UserId = serverLongTranslation.UserId;
    //        SeedData.Instance.DogTranslationDto.UserId = serverDogTranslation.UserId;

    //        translationsCorrected = true;
    //      });
    //  });

    //  EnqueueConditional(() => isLoaded);
    //  EnqueueConditional(() => translationsCorrected);
    //  EnqueueCallback(() => { Assert.IsNull(error); },
    //                  () => { Assert.IsNotNull(allLanguages); },
    //                  () => { Assert.AreNotEqual(Guid.Empty, SeedData.Instance.EnglishId); },
    //                  () => { Assert.AreNotEqual(Guid.Empty, SeedData.Instance.SpanishId); },
    //                  () => { Assert.IsTrue(allLanguages.Count > 0); });
    //  EnqueueTestComplete();
    //}

    [TestMethod]
    [Asynchronous]
    public void CREATE_NEW()
    {
      var isCreated = false;
      TranslationEdit newTranslationEdit = null;
      Exception newError = new Exception();

      TranslationEdit.NewTranslationEdit((s, r) =>
        {
          newError = r.Error;
          if (newError != null)
            throw newError;

          newTranslationEdit = r.Object;
          isCreated = true;
        });
      EnqueueConditional(() => isCreated);
      EnqueueCallback(
                      () => { Assert.IsNotNull(newTranslationEdit); },
                      () => { Assert.IsNull(newError); }
                      );
      EnqueueTestComplete();
    }

    //[TestMethod]
    //[Asynchronous]
    //public void CREATE_NEW_WITH_ID()
    //{
    //  Guid id = new Guid("BDEF87AC-21FA-4BAE-A155-91CDDA52C9CD");

    //  var isCreated = false;
    //  TranslationEdit TranslationEdit = null;
    //  TranslationEdit.NewTranslationEdit(id, (s,r) =>
    //    {
    //      if (r.Error != null)
    //        throw r.Error;

    //      TranslationEdit = r.Object;
    //      isCreated = true;
    //    });
    //  EnqueueConditional(() => isCreated);
    //  EnqueueCallback(() => { Assert.IsNotNull(TranslationEdit); },
    //                  () => { Assert.IsNull(null); },
    //                  () => { Assert.AreEqual(id, TranslationEdit.Id); });
    //  EnqueueTestComplete();
    //}

    [TestMethod]
    [Asynchronous]
    [Tag("tget")]
    public void GET()
    {
      Guid testId = Guid.Empty;
      var allLoaded = false;
      var isLoaded = false;
      Exception getAllError = new Exception();
      Exception error = new Exception();
      TranslationEdit translationEdit = null;

      TranslationList.GetAll((s1, r1) =>
        {
          getAllError = r1.Error;
          if (getAllError != null)
            throw r1.Error;
          testId = r1.Object.First().Id;
          allLoaded = true;
          TranslationEdit.GetTranslationEdit(testId, (s, r) =>
          {
            error = r.Error;
            translationEdit = r.Object;
            isLoaded = true;
          });
        });


      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => allLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNull(getAllError); },
                      () => { Assert.IsNotNull(translationEdit); },
                      () => { Assert.IsTrue(translationEdit.PhraseIds.Count >= 2); },
                      () => { Assert.IsTrue(translationEdit.Phrases.Count == translationEdit.PhraseIds.Count); },
                      () => { Assert.AreEqual(testId, translationEdit.Id); });
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void NEW_EDIT_BEGINSAVE_GET()
    {
      //INITIALIZE ERRORS TO EXCEPTION, BECAUSE WE TEST IF THEY ARE NULL LATER
      Exception newError = new Exception();
      Exception savedError = new Exception();
      Exception gottenError = new Exception();

      TranslationEdit newTranslationEdit = null;
      TranslationEdit savedTranslationEdit = null;
      TranslationEdit gottenTranslationEdit = null;

      PhraseEdit phraseA = null;
      PhraseEdit phraseB = null;

      var isNewed = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;

      //NEW
      TranslationEdit.NewTranslationEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newTranslationEdit = r.Object;
        isNewed = true;

        //EDIT
        newTranslationEdit.UserId = SeedData.Instance.GetTestValidUserDto().Id;
        newTranslationEdit.Username = SeedData.Instance.TestValidUsername;
        PhraseEdit.NewPhraseEdit((s4, r4) => 
          {
            if (r4.Error != null)
              throw r4.Error;

            phraseA = r4.Object;
            phraseA.Text = "Test Phrase A Text.  This is phrase A in English.";
            phraseA.LanguageId = SeedData.Instance.EnglishId;
            newTranslationEdit.AddPhrase(phraseA);

            PhraseEdit.NewPhraseEdit((s5, r5) =>
              {
                if (r5.Error != null)
                  throw r5.Error;
                
                phraseB = r5.Object;
                phraseB.Text = "Test Phrase B Text.  This is phrase B in Spanish.";
                phraseB.LanguageId = SeedData.Instance.SpanishId;
                newTranslationEdit.AddPhrase(phraseB);

                isEdited = true;

                //SAVE
                newTranslationEdit.BeginSave((s2, r2) =>
                {
                  savedError = r2.Error;
                  if (savedError != null)
                    throw savedError;
                  savedTranslationEdit = (TranslationEdit)r2.NewObject;

                  isSaved = true;
                  
                  //GET (CONFIRM SAVE)
                  TranslationEdit.GetTranslationEdit(savedTranslationEdit.Id, (s3, r3) =>
                  {
                    gottenError = r3.Error;
                    if (gottenError != null)
                      throw gottenError;
                    gottenTranslationEdit = r3.Object;
                    isGotten = true;
                  });
                });
              });
          });
      });

      EnqueueConditional(() => isNewed);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueCallback(
                      () => { Assert.IsNull(newError); },
                      () => { Assert.IsNull(savedError); },
                      () => { Assert.IsNull(gottenError); },
                      () => { Assert.IsNotNull(newTranslationEdit); },
                      () => { Assert.IsNotNull(savedTranslationEdit); },
                      () => { Assert.IsNotNull(gottenTranslationEdit); },
                      () => { Assert.IsNotNull(phraseA); },
                      () => { Assert.IsNotNull(phraseB); },
                      () => { Assert.AreEqual(gottenTranslationEdit.Phrases.Count, gottenTranslationEdit.PhraseIds.Count); },
                      () => { Assert.AreEqual(savedTranslationEdit.Id, gottenTranslationEdit.Id); }
                     );

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    [ExpectedException(typeof(ExpectedException))]
    public void NEW_EDIT_BEGINSAVE_GET_DELETE_GET()
    {
      TranslationEdit newTranslationEdit = null;
      TranslationEdit savedTranslationEdit = null;
      TranslationEdit gottenTranslationEdit = null;
      TranslationEdit deletedTranslationEdit = null;

      PhraseEdit phraseA = null;
      PhraseEdit phraseB = null;

      //INITIALIZE TO EMPTY TRANSLATIONEDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
      TranslationEdit deleteConfirmedTranslationEdit = new TranslationEdit();

      var isNewed = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;
      var isDeleted = false;
      var isDeleteConfirmed = false;

      //NEW
      TranslationEdit.NewTranslationEdit((sNew, rNew) =>
      {
        if (rNew.Error != null)
          throw rNew.Error;
        newTranslationEdit = rNew.Object;
        isNewed = true;

        //EDIT
        newTranslationEdit.UserId = SeedData.Instance.GetTestValidUserDto().Id;
        newTranslationEdit.Username = SeedData.Instance.TestValidUsername;
        PhraseEdit.NewPhraseEdit((sNewPhraseA, rNewPhraseA) =>
        {
          if (rNewPhraseA.Error != null)
            throw rNewPhraseA.Error;

          phraseA = rNewPhraseA.Object;
          phraseA.Text = "Test Phrase A Text.  This is phrase A in English.";
          phraseA.LanguageId = SeedData.Instance.EnglishId;
          newTranslationEdit.AddPhrase(phraseA);

          PhraseEdit.NewPhraseEdit((sNewPhraseB, rNewPhraseB) =>
          {
            if (rNewPhraseB.Error != null)
              throw rNewPhraseB.Error;

            phraseB = rNewPhraseB.Object;
            phraseB.Text = "Test Phrase B Text.  This is phrase B in Spanish.";
            phraseB.LanguageId = SeedData.Instance.SpanishId;
            newTranslationEdit.AddPhrase(phraseB);

            isEdited = true;

            //SAVE
            newTranslationEdit.BeginSave((sSave, rSave) =>
            {
              if (rSave.Error!= null)
                throw rSave.Error;
              savedTranslationEdit = (TranslationEdit)rSave.NewObject;

              isSaved = true;

              //GET (CONFIRM SAVE)
              TranslationEdit.GetTranslationEdit(savedTranslationEdit.Id, (sGet, rGet) =>
              {
                if (rGet.Error!= null)
                  throw rGet.Error;

                gottenTranslationEdit = rGet.Object;
                isGotten = true;

                //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
                gottenTranslationEdit.Delete();
                gottenTranslationEdit.BeginSave((sSaveGotten, rSaveGotten) =>
                {
                  if (rSaveGotten.Error != null)
                    throw rSaveGotten.Error;

                  deletedTranslationEdit = (TranslationEdit)rSaveGotten.NewObject;
                  //TODO: Figure out why TranslationEditTests final callback gets thrown twice.  When server throws an exception (expected because we are trying to fetch a deleted translation that shouldn't exist), the callback is executed twice.

                  TranslationEdit.GetTranslationEdit(deletedTranslationEdit.Id, (sGetDeleted, rGetDeleted) =>
                  {
                    var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
                    var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
                    if (rGetDeleted.Error != null)
                    {
                      isDeleteConfirmed = true;
                      throw new ExpectedException(rGetDeleted.Error);
                    }
                    deleteConfirmedTranslationEdit = rGetDeleted.Object;
                  });
                });
              });
            });
          });
        });
      });

      EnqueueConditional(() => isNewed);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueConditional(() => isDeleted);
      EnqueueConditional(() => isDeleteConfirmed);
      EnqueueCallback(
                      () => { Assert.IsNotNull(newTranslationEdit); },
                      () => { Assert.IsNotNull(savedTranslationEdit); },
                      () => { Assert.IsNotNull(gottenTranslationEdit); },
                      () => { Assert.IsNotNull(deletedTranslationEdit); },
                      () => { Assert.IsNull(deleteConfirmedTranslationEdit); });

      EnqueueTestComplete();
    }

  }
}