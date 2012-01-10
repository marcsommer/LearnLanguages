using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;
using LearnLanguages.DataAccess.Mock;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;
using System.Linq;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("language")]
  public class LanguageTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    private Guid _EnglishId { get; set; }
    private Guid _SpanishId { get; set; }

    [ClassInitialize]
    [Asynchronous]
    public void InitializeLanguageTests()
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
      LanguageEdit languageEdit = null;
      LanguageEdit.NewLanguageEdit( (s,r) =>
        {
          if (r.Error != null)
            throw r.Error;

          languageEdit = r.Object;
          isCreated = true;
        });
      EnqueueConditional(() => isCreated);
      EnqueueCallback(() => { Assert.IsNotNull(languageEdit); },
                      () => { Assert.IsNull(null); });
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void CREATE_NEW_WITH_ID()
    {
      Guid id = new Guid("BDEF87AC-21FA-4BAE-A155-91CDDA52C9CD");
    
      var isCreated = false;
      LanguageEdit languageEdit = null;
      LanguageEdit.NewLanguageEdit(id, (s,r) =>
        {
          if (r.Error != null)
            throw r.Error;

          languageEdit = r.Object;
          isCreated = true;
        });
      EnqueueConditional(() => isCreated);
      EnqueueCallback(() => { Assert.IsNotNull(languageEdit); },
                      () => { Assert.IsNull(null); },
                      () => { Assert.AreEqual(id, languageEdit.Id); });
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    [Tag("lget")]
    public void GET()
    {
      Guid testId = _EnglishId;
      var isLoaded = false;
      Exception error = null;
      LanguageEdit languageEdit = null;

      LanguageEdit.GetLanguageEdit(testId, (s, r) =>
      {
        error = r.Error;
        if (error != null)
          throw error;
        languageEdit = r.Object;
        isLoaded = true;
      });

      EnqueueConditional(() => isLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNotNull(languageEdit); },
                      () => { Assert.AreEqual(testId, languageEdit.Id); });
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

      LanguageEdit languageEdit = null;
      LanguageEdit savedLanguageEdit = null;
      LanguageEdit gottenLanguageEdit = null;    
  
      var isNewed = false;
      var isSaved = false;
      var isGotten = false;
      
      //NEW
      LanguageEdit.NewLanguageEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        languageEdit = r.Object;
        isNewed = true;

        //EDIT
        languageEdit.Text = "TestLanguage";

        //SAVE
        languageEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          savedLanguageEdit = r2.NewObject as LanguageEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          LanguageEdit.GetLanguageEdit(savedLanguageEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;
            gottenLanguageEdit = r3.Object;
            isGotten = true;
          });
        });

      });

      EnqueueConditional(() => isNewed);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueCallback(
                      () => { Assert.IsNull(newError); },
                      () => { Assert.IsNull(savedError); },
                      () => { Assert.IsNull(gottenError); },
                      () => { Assert.IsNotNull(languageEdit); },
                      () => { Assert.IsNotNull(savedLanguageEdit); },
                      () => { Assert.IsNotNull(gottenLanguageEdit); },
                      () => { Assert.AreEqual(savedLanguageEdit.Id, gottenLanguageEdit.Id); },
                      () => { Assert.AreEqual(savedLanguageEdit.Text, gottenLanguageEdit.Text); });

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

      LanguageEdit languageEdit = null;
      LanguageEdit savedLanguageEdit = null;
      LanguageEdit gottenLanguageEdit = null;
      LanguageEdit deletedLanguageEdit = null;

      //INITIALIZE TO EMPTY LANGUAGE EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
      LanguageEdit deleteConfirmedLanguageEdit = new LanguageEdit();

      var isNewed = false;
      var isSaved = false;
      var isGotten = false;
      var isDeleted = false;
      var isDeleteConfirmed = false;

      //NEW
      LanguageEdit.NewLanguageEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        languageEdit = r.Object;
        isNewed = true;

        //EDIT
        languageEdit.Text = "TestLanguage";

        //SAVE
        languageEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          savedLanguageEdit = r2.NewObject as LanguageEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          LanguageEdit.GetLanguageEdit(savedLanguageEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;
            gottenLanguageEdit = r3.Object;
            isGotten = true;

            //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
            gottenLanguageEdit.Delete();
            gottenLanguageEdit.BeginSave((s4, r4) =>
            {
              deletedError = r4.Error;
              if (deletedError != null)
                throw deletedError;

              deletedLanguageEdit = r4.NewObject as LanguageEdit;
              //TODO: Figure out why LanguageEditTests final callback gets thrown twice.  The server throws expected exception, but callback is executed twice.
              LanguageEdit.GetLanguageEdit(deletedLanguageEdit.Id, (s5, r5) =>
              {
                deleteConfirmedError = r5.Error;
                if (deleteConfirmedError != null)
                {
                  isDeleteConfirmed = true;
                  throw new ExpectedException(deleteConfirmedError);
                }
                deleteConfirmedLanguageEdit = r5.Object;
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

                      () => { Assert.IsNotNull(languageEdit); },
                      () => { Assert.IsNotNull(savedLanguageEdit); },
                      () => { Assert.IsNotNull(gottenLanguageEdit); },
                      () => { Assert.IsNotNull(deletedLanguageEdit); },
                      () => { Assert.IsNull(deleteConfirmedLanguageEdit); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    [Tag("lgetall")]
    public void GET_ALL()
    {
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