using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;
#if USE_MOCK
using LearnLanguages.DataAccess.Mock;
#endif
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Linq;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("phrase")]
  public class PhraseTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    private Guid _EnglishId { get; set; }
    private Guid _SpanishId { get; set; }

    [ClassInitialize]
    [Asynchronous]
    public void InitializePhraseTests()
    {
      var isLoaded = false;
      Exception error = null;
      LanguageList allLanguages = null;
      LanguageList.GetAll((s, r) =>
      {
        error = r.Error;
        if (error != null)
          throw error;

        allLanguages = r.Object;
        _EnglishId = (from language in allLanguages
                      where language.Text == SeedData.EnglishText
                      select language.Id).First();

        _SpanishId = (from language in allLanguages
                      where language.Text == SeedData.SpanishText
                      select language.Id).First();

        isLoaded = true;
      });

      EnqueueConditional(() => isLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNotNull(allLanguages); },
                      () => { Assert.AreNotEqual(Guid.Empty, _EnglishId); },
                      () => { Assert.AreNotEqual(Guid.Empty, _SpanishId); },
                      () => { Assert.IsTrue(allLanguages.Count > 0); });
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void CREATE_NEW()
    {
      var isCreated = false;
      PhraseEdit newPhraseEdit = null;
      Exception newError = new Exception();

      PhraseEdit.NewPhraseEdit( (s,r) =>
        {
          newError = r.Error;
          if (newError != null)
            throw newError;

          newPhraseEdit = r.Object;
          isCreated = true;
        });
      EnqueueConditional(() => isCreated);
      EnqueueCallback(
                      () => { Assert.IsNotNull(newPhraseEdit); },
                      () => { Assert.IsNull(newError); },
                      () => { Assert.AreEqual(Guid.Parse(DalResources.DefaultLanguageId), newPhraseEdit.LanguageId); },
                      () => { Assert.IsNotNull(newPhraseEdit.Language); }
                      );
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void CREATE_NEW_WITH_ID()
    {
      Guid id = new Guid("BDEF87AC-21FA-4BAE-A155-91CDDA52C9CD");
    
      var isCreated = false;
      PhraseEdit PhraseEdit = null;
      PhraseEdit.NewPhraseEdit(id, (s,r) =>
        {
          if (r.Error != null)
            throw r.Error;

          PhraseEdit = r.Object;
          isCreated = true;
        });
      EnqueueConditional(() => isCreated);
      EnqueueCallback(() => { Assert.IsNotNull(PhraseEdit); },
                      () => { Assert.IsNull(null); },
                      () => { Assert.AreEqual(id, PhraseEdit.Id); });
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void GET()
    {
      Guid testId = SeedData.IdHello;
      var isLoaded = false;
      Exception error = null;
      PhraseEdit PhraseEdit = null;

      PhraseEdit.GetPhraseEdit(testId, (s, r) =>
      {
        error = r.Error;
        PhraseEdit = r.Object;
        isLoaded = true;
      });

      EnqueueConditional(() => isLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNotNull(PhraseEdit); },
                      () => { Assert.AreEqual(testId, PhraseEdit.Id); });
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

      PhraseEdit newPhraseEdit = null;
      PhraseEdit savedPhraseEdit = null;
      PhraseEdit gottenPhraseEdit = null;    
  
      var isNewed = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;
      
      //NEW
      PhraseEdit.NewPhraseEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newPhraseEdit = r.Object;
        isNewed = true;

        //EDIT
        newPhraseEdit.Text = "TestPhrase NEW_EDIT_BEGINSAVE_GET";
        isEdited = true;

        //SAVE
        newPhraseEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          savedPhraseEdit = r2.NewObject as PhraseEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          PhraseEdit.GetPhraseEdit(savedPhraseEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;
            gottenPhraseEdit = r3.Object;
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
                      () => { Assert.IsNotNull(newPhraseEdit); },
                      () => { Assert.IsNotNull(savedPhraseEdit); },
                      () => { Assert.IsNotNull(gottenPhraseEdit); },
                      () => { Assert.AreEqual(savedPhraseEdit.Id, gottenPhraseEdit.Id); },
                      () => { Assert.AreEqual(savedPhraseEdit.Text, gottenPhraseEdit.Text); });

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

      PhraseEdit PhraseEdit = null;
      PhraseEdit savedPhraseEdit = null;
      PhraseEdit gottenPhraseEdit = null;
      PhraseEdit deletedPhraseEdit = null;

      //INITIALIZE TO EMPTY Phrase EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
      PhraseEdit deleteConfirmedPhraseEdit = new PhraseEdit();

      var isNewed = false;
      var isSaved = false;
      var isGotten = false;
      var isDeleted = false;
      var isDeleteConfirmed = false;

      //NEW
      PhraseEdit.NewPhraseEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        PhraseEdit = r.Object;
        isNewed = true;

        //EDIT
        PhraseEdit.Text = "TestPhrase NEW_EDIT_BEGINSAVE_GET_DELETE_GET";

        //SAVE
        PhraseEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          savedPhraseEdit = r2.NewObject as PhraseEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          PhraseEdit.GetPhraseEdit(savedPhraseEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;
            gottenPhraseEdit = r3.Object;
            isGotten = true;

            //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
            gottenPhraseEdit.Delete();
            gottenPhraseEdit.BeginSave((s4, r4) =>
            {
              deletedError = r4.Error;
              if (deletedError != null)
                throw deletedError;

              deletedPhraseEdit = r4.NewObject as PhraseEdit;
              //TODO: Figure out why PhraseEditTests final callback gets thrown twice.  The server throws expected exception, but callback is executed twice.
              PhraseEdit.GetPhraseEdit(deletedPhraseEdit.Id, (s5, r5) =>
              {
                deleteConfirmedError = r5.Error;
                if (deleteConfirmedError != null)
                {
                  isDeleteConfirmed = true;
                  throw new ExpectedException(deleteConfirmedError);
                }
                deleteConfirmedPhraseEdit = r5.Object;
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

                      () => { Assert.IsNotNull(PhraseEdit); },
                      () => { Assert.IsNotNull(savedPhraseEdit); },
                      () => { Assert.IsNotNull(gottenPhraseEdit); },
                      () => { Assert.IsNotNull(deletedPhraseEdit); },
                      () => { Assert.IsNull(deleteConfirmedPhraseEdit); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void GET_ALL()
    {
      {
        var isLoaded = false;
        Exception error = null;
        PhraseList allLanguages = null;
        PhraseList.GetAll((s, r) =>
        {
          error = r.Error;
          if (error != null)
            throw error;

          allLanguages = r.Object;
          isLoaded = true;
        });

        EnqueueConditional(() => isLoaded);
        EnqueueCallback(() => { Assert.IsNull(error); },
                        () => { Assert.IsNotNull(allLanguages); },
                        () => { Assert.IsTrue(allLanguages.Count > 0); });
        EnqueueTestComplete();
      }
    }
  }
}