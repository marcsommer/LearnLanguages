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
  [Tag("phrase_belief")]
  public class PhraseBeliefTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    //private LanguageEdit _ServerEnglishLang;
    //private LanguageEdit _ServerSpanishLang;
    private PhraseEdit _TestPhraseEdit;

    [ClassInitialize]
    [Asynchronous]
    public void InitializePhraseBeliefTests()
    {
      var isLoaded = false;
      PhraseList.GetAll((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          _TestPhraseEdit = r.Object.First();
          isLoaded = true;
        });

      EnqueueConditional(() => isLoaded);
      EnqueueCallback(() => { Assert.IsNotNull(_TestPhraseEdit); });
      EnqueueTestComplete();
    }

    //  //WE NEED TO UPDATE THE CLIENT SEEDDATA.INSTANCE IDS.  
    //  var phraseBeliefsCorrected = false;
    //  Exception error = new Exception();
    //  Exception errorPhraseBeliefList = new Exception();
    //  LanguageList allLanguages = null;
    //  PhraseBeliefList allPhraseBeliefs = null;

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

    //    PhraseBeliefList.GetAll((s2, r2) =>
    //      {
    //        errorPhraseBeliefList = r2.Error;
    //        if (errorPhraseBeliefList != null)
    //          throw errorPhraseBeliefList;

    //        allPhraseBeliefs = r2.Object;

    //        var serverHelloPhraseBeliefQuery = (from phraseBelief in allPhraseBeliefs
    //                                 where phraseBelief.Text == SeedData.Instance.HelloText && 
    //                                       phraseBelief.Language.Text == SeedData.Instance.EnglishText
    //                                 select phraseBelief);
    //        PhraseBeliefEdit serverHelloPhraseBelief = null;
    //        if (serverHelloPhraseBeliefQuery.Count() == 0) //we don't have the hello phraseBelief in the db, so put it there
    //        {
    //          var phraseBelief = allPhraseBeliefs[0];
    //          phraseBelief.BeginEdit();
    //          phraseBelief.Text = SeedData.Instance.HelloText;
    //          phraseBelief.Language = _ServerEnglishLang;
    //          phraseBelief.ApplyEdit();
    //          serverHelloPhraseBelief = phraseBelief;
    //        }
    //        else
    //          serverHelloPhraseBelief = serverHelloPhraseBeliefQuery.First();


    //        var serverHolaPhraseBeliefQuery = (from phraseBelief in allPhraseBeliefs
    //                                      where phraseBelief.Text == SeedData.Instance.HolaText &&
    //                                            phraseBelief.Language.Text == SeedData.Instance.EnglishText
    //                                      select phraseBelief);
    //        PhraseBeliefEdit serverHolaPhraseBelief = null;
    //        if (serverHolaPhraseBeliefQuery.Count() == 0) //we don't have the Hola phraseBelief in the db, so put it there
    //        {
    //          var phraseBelief = allPhraseBeliefs[1];
    //          phraseBelief.BeginEdit();
    //          phraseBelief.Text = SeedData.Instance.HolaText;
    //          phraseBelief.Language = _ServerSpanishLang;
    //          phraseBelief.ApplyEdit();
    //          serverHolaPhraseBelief = phraseBelief;
    //        }
    //        else
    //          serverHolaPhraseBelief = serverHolaPhraseBeliefQuery.First();

    //        var serverLongPhraseBeliefQuery = (from phraseBelief in allPhraseBeliefs
    //                                     where phraseBelief.Text == SeedData.Instance.LongText &&
    //                                           phraseBelief.Language.Text == SeedData.Instance.EnglishText
    //                                     select phraseBelief);
    //        PhraseBeliefEdit serverLongPhraseBelief = null;
    //        if (serverLongPhraseBeliefQuery.Count() == 0) //we don't have the Long phraseBelief in the db, so put it there
    //        {
    //          var phraseBelief = allPhraseBeliefs[2];
    //          phraseBelief.BeginEdit();
    //          phraseBelief.Text = SeedData.Instance.LongText;
    //          phraseBelief.Language = _ServerEnglishLang;
    //          phraseBelief.ApplyEdit();
    //          serverLongPhraseBelief = phraseBelief;
    //        }
    //        else
    //          serverLongPhraseBelief = serverLongPhraseBeliefQuery.First();


    //        var serverDogPhraseBeliefQuery = (from phraseBelief in allPhraseBeliefs
    //                                     where phraseBelief.Text == SeedData.Instance.DogText &&
    //                                           phraseBelief.Language.Text == SeedData.Instance.EnglishText
    //                                     select phraseBelief);
    //        PhraseBeliefEdit serverDogPhraseBelief = null;
    //        if (serverDogPhraseBeliefQuery.Count() == 0) //we don't have the Dog phraseBelief in the db, so put it there
    //        {
    //          var phraseBelief = allPhraseBeliefs[3];
    //          phraseBelief.BeginEdit();
    //          phraseBelief.Text = SeedData.Instance.DogText;
    //          phraseBelief.Language = _ServerSpanishLang;
    //          phraseBelief.ApplyEdit();
    //          serverDogPhraseBelief = phraseBelief;
    //        }
    //        else
    //          serverDogPhraseBelief = serverDogPhraseBeliefQuery.First();

    //        var validUserId = serverHelloPhraseBelief.UserId;
    //        SeedData.Instance.GetTestValidUserDto().Id = validUserId;

    //        SeedData.Instance.HelloPhraseBeliefDto.Id = serverHelloPhraseBelief.Id;
    //        SeedData.Instance.HolaPhraseBeliefDto.Id = serverHolaPhraseBelief.Id;
    //        SeedData.Instance.LongPhraseBeliefDto.Id = serverLongPhraseBelief.Id;
    //        SeedData.Instance.DogPhraseBeliefDto.Id = serverDogPhraseBelief.Id;

    //        SeedData.Instance.HelloPhraseBeliefDto.UserId = serverHelloPhraseBelief.UserId;
    //        SeedData.Instance.HolaPhraseBeliefDto.UserId = serverHolaPhraseBelief.UserId;
    //        SeedData.Instance.LongPhraseBeliefDto.UserId = serverLongPhraseBelief.UserId;
    //        SeedData.Instance.DogPhraseBeliefDto.UserId = serverDogPhraseBelief.UserId;

    //        phraseBeliefsCorrected = true;
    //      });
    //  });

    //  EnqueueConditional(() => isLoaded);
    //  EnqueueConditional(() => phraseBeliefsCorrected);
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
      PhraseBeliefEdit newPhraseBeliefEdit = null;
      Exception newError = new Exception();
      
      PhraseBeliefEdit.NewPhraseBeliefEdit( (s,r) =>
        {
          newError = r.Error;
          if (newError != null)
            throw newError;

          newPhraseBeliefEdit = r.Object;
          isCreated = true;
        });
      EnqueueConditional(() => isCreated);
      EnqueueCallback(
                      () => { Assert.IsNotNull(newPhraseBeliefEdit); },
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
    //  PhraseBeliefEdit PhraseBeliefEdit = null;
    //  PhraseBeliefEdit.NewPhraseBeliefEdit(id, (s,r) =>
    //    {
    //      if (r.Error != null)
    //        throw r.Error;

    //      PhraseBeliefEdit = r.Object;
    //      isCreated = true;
    //    });
    //  EnqueueConditional(() => isCreated);
    //  EnqueueCallback(() => { Assert.IsNotNull(PhraseBeliefEdit); },
    //                  () => { Assert.IsNull(null); },
    //                  () => { Assert.AreEqual(id, PhraseBeliefEdit.Id); });
    //  EnqueueTestComplete();
    //}

    [TestMethod]
    [Asynchronous]
    public void GET()
    {
      Guid testId = Guid.Empty;
      var allLoaded = false;
      var isLoaded = false;
      Exception getAllError = new Exception();
      Exception error = new Exception();
      PhraseBeliefEdit PhraseBeliefEdit = null;

      PhraseBeliefList.GetAll((s1, r1) =>
        {
          getAllError = r1.Error;
          if (getAllError != null)
            throw r1.Error;
          testId = r1.Object.First().Id;
          allLoaded = true;
          PhraseBeliefEdit.GetPhraseBeliefEdit(testId, (s, r) =>
          {
            error = r.Error;
            PhraseBeliefEdit = r.Object;
            isLoaded = true;
          });
        });


      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => allLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNull(getAllError); },
                      () => { Assert.IsNotNull(PhraseBeliefEdit); },
                      () => { Assert.AreEqual(testId, PhraseBeliefEdit.Id); });
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    [Tag("current")]
    public void NEW_EDIT_BEGINSAVE_GET()
    {
      //INITIALIZE ERRORS TO EXCEPTION, BECAUSE WE TEST IF THEY ARE NULL LATER
      Exception newError = new Exception();
      Exception savedError = new Exception();
      Exception gottenError = new Exception();

      PhraseBeliefEdit newPhraseBeliefEdit = null;
      PhraseBeliefEdit savedPhraseBeliefEdit = null;
      PhraseBeliefEdit gottenPhraseBeliefEdit = null;    
  
      var isNewed = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;
      
      //NEW
      PhraseBeliefEdit.NewPhraseBeliefEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newPhraseBeliefEdit = r.Object;
        isNewed = true;

        //EDIT
        newPhraseBeliefEdit.Date = DateTime.Now - TimeSpan.FromDays(1);
        newPhraseBeliefEdit.Date = DateTime.Now - TimeSpan.FromDays(1);
        newPhraseBeliefEdit.Text = "TestPhraseBelief.Text edited in NEW_EDIT_BEGINSAVE_GET test";
        newPhraseBeliefEdit.Strength = 2.0d;
        newPhraseBeliefEdit.Phrase = _TestPhraseEdit;
        //newPhraseBeliefEdit.Phrase.Language = _ServerEnglishLang;
        //newPhraseBeliefEdit.PhraseBeliefNumber = 0;
        //Assert.AreEqual(SeedData.Instance.TestValidUsername, newPhraseBeliefEdit.Username);

        //newPhraseBeliefEdit.UserId = SeedData.Instance.GetTestValidUserDto().Id;
        //newPhraseBeliefEdit.Username = SeedData.Instance.TestValidUsername;
        isEdited = true;

        //SAVE
        newPhraseBeliefEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          
          savedPhraseBeliefEdit = (PhraseBeliefEdit)r2.NewObject;
          isSaved = true;

          //GET (CONFIRM SAVE)
          PhraseBeliefEdit.GetPhraseBeliefEdit(savedPhraseBeliefEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;

            gottenPhraseBeliefEdit = r3.Object;
            isGotten = true;
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
                      () => { Assert.IsNotNull(newPhraseBeliefEdit); },
                      () => { Assert.AreNotEqual(Guid.Empty, newPhraseBeliefEdit.PhraseId); },
                      () => { Assert.IsNotNull(savedPhraseBeliefEdit); },
                      () => { Assert.IsNotNull(gottenPhraseBeliefEdit); },
                      () => { Assert.AreEqual(savedPhraseBeliefEdit.Id, gottenPhraseBeliefEdit.Id); }
                     );

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    [ExpectedException(typeof(ExpectedException))]
    public void NEW_EDIT_BEGINSAVE_GET_DELETE_GET()
    {
      //INITIALIZE ERRORS TO EXCEPTION, BECAUSE EXPECT THEM TO BE NULL LATER
      Exception newError = new Exception();
      Exception savedError = new Exception();
      Exception gottenError = new Exception();
      Exception deletedError = new Exception();

      //INITIALIZE CONFIRM TO NULL, BECAUSE WE EXPECT THIS TO BE AN ERROR LATER
      Exception deleteConfirmedError = null;

      PhraseBeliefEdit newPhraseBeliefEdit = null;
      PhraseBeliefEdit savedPhraseBeliefEdit = null;
      PhraseBeliefEdit gottenPhraseBeliefEdit = null;
      PhraseBeliefEdit deletedPhraseBeliefEdit = null;

      //INITIALIZE TO EMPTY PhraseBelief EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
      PhraseBeliefEdit deleteConfirmedPhraseBeliefEdit = new PhraseBeliefEdit();

      var isNewed = false;
      var isSaved = false;
      var isGotten = false;
      var isDeleted = false;
      var isDeleteConfirmed = false;

      //NEW
      PhraseBeliefEdit.NewPhraseBeliefEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newPhraseBeliefEdit = r.Object;
        isNewed = true;

        //EDIT
        newPhraseBeliefEdit.Date = DateTime.Now - TimeSpan.FromDays(7);
        newPhraseBeliefEdit.Text = "TestPhraseBelief.Text edited in NEW_EDIT_BEGINSAVE_GET_DELETE_GET test";
        newPhraseBeliefEdit.Strength = 3.0d;
        newPhraseBeliefEdit.Phrase = _TestPhraseEdit;
        Assert.AreEqual(SeedData.Instance.TestValidUsername, newPhraseBeliefEdit.Username);

        //SAVE
        newPhraseBeliefEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;

          savedPhraseBeliefEdit = (PhraseBeliefEdit)r2.NewObject;
          isSaved = true;

          //GET (CONFIRM SAVE)
          PhraseBeliefEdit.GetPhraseBeliefEdit(savedPhraseBeliefEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;

            gottenPhraseBeliefEdit = r3.Object;
            isGotten = true;

            //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
            gottenPhraseBeliefEdit.Delete();
            gottenPhraseBeliefEdit.BeginSave((s4, r4) =>
            {
              deletedError = r4.Error;
              if (deletedError != null)
                throw deletedError;

              deletedPhraseBeliefEdit = (PhraseBeliefEdit)r4.NewObject;

              PhraseBeliefEdit.GetPhraseBeliefEdit(deletedPhraseBeliefEdit.Id, (s5, r5) =>
              {
                var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
                var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
                deleteConfirmedError = r5.Error;
                if (deleteConfirmedError != null)
                {
                  isDeleteConfirmed = true;
                  throw new ExpectedException(deleteConfirmedError);
                }
                deleteConfirmedPhraseBeliefEdit = r5.Object;
              });
            });
          });
        });
      });

      EnqueueConditional(() => isNewed);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueConditional(() => isDeleted);
      EnqueueConditional(() => isDeleteConfirmed);
      EnqueueCallback(
                      () => { Assert.IsNull(newError); },
                      () => { Assert.IsNull(savedError); },
                      () => { Assert.IsNull(gottenError); },
                      () => { Assert.IsNull(deletedError); },
                      //WE EXPECT AN ERROR, AS WE TRIED A GET ON AN ID THAT SHOULD HAVE BEEN DELETED
                      () => { Assert.IsNotNull(deleteConfirmedError); },

                      () => { Assert.IsNotNull(newPhraseBeliefEdit); },
                      () => { Assert.IsNotNull(savedPhraseBeliefEdit); },
                      () => { Assert.IsNotNull(gottenPhraseBeliefEdit); },
                      () => { Assert.IsNotNull(deletedPhraseBeliefEdit); },
                      () => { Assert.IsNull(deleteConfirmedPhraseBeliefEdit); });

      EnqueueTestComplete();
    }
  }
}