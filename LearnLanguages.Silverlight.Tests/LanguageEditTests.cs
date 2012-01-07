using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;
using LearnLanguages.DataAccess.Mock;
using LearnLanguages.DataAccess.Exceptions;

namespace LearnLanguages.Client.Tests
{
  [TestClass]
  public class LanguageEditTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    [TestMethod]
    public void CREATE_NEW()
    {
      LanguageEdit.NewLanguageEdit( (s,r) =>
        {
          Assert.IsNull(r.Error);
          LanguageEdit newLanguageEdit = r.Object;
          Assert.IsNotNull(newLanguageEdit);
        });
    }

    [TestMethod]
    public void CREATE_NEW_WITH_ID()
    {
      Guid id = new Guid("BDEF87AC-21FA-4BAE-A155-91CDDA52C9CD");
      LanguageEdit.NewLanguageEdit(id, (s, r) =>
      {
        Assert.IsNull(r.Error);
        LanguageEdit newLanguageEdit = r.Object;
        Assert.IsNotNull(newLanguageEdit);
        Assert.AreEqual(id, newLanguageEdit.Id);
      });
    }

    [TestMethod]
    [Asynchronous]
    public void GET()
    {
      Guid testId = MockDb.EnglishId;
      var isLoaded = false;
      Exception error = null;
      LanguageEdit languageEdit = null;

      LanguageEdit.GetLanguageEdit(testId, (s, r) =>
      {
        error = r.Error;
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
        languageEdit = r.Object;
        isNewed = true;

        //EDIT
        languageEdit.Text = "TestLanguage";

        //SAVE
        languageEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          savedLanguageEdit = r2.NewObject as LanguageEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          LanguageEdit.GetLanguageEdit(savedLanguageEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
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
    [ExpectedException(typeof(Csla.DataPortalException))]
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
        languageEdit = r.Object;
        isNewed = true;

        //EDIT
        languageEdit.Text = "TestLanguage";

        //SAVE
        languageEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          savedLanguageEdit = r2.NewObject as LanguageEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          LanguageEdit.GetLanguageEdit(savedLanguageEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            gottenLanguageEdit = r3.Object;
            isGotten = true;

            //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
            gottenLanguageEdit.Delete();
            gottenLanguageEdit.BeginSave((s4, r4) =>
            {
              deletedError = r4.Error;
              deletedLanguageEdit = r4.NewObject as LanguageEdit;

              try
              {
                LanguageEdit.GetLanguageEdit(deletedLanguageEdit.Id, (s5, r5) =>
                {
                  try
                  {
                    deleteConfirmedError = r5.Error;
                    if (deleteConfirmedError != null)
                      throw deleteConfirmedError;
                  }
                  catch (Csla.DataPortalException dpex)
                  {
                    var dummy = 5;
                  }
                  //deleteConfirmedLanguageEdit = r5.Object;
                  isDeleteConfirmed = true;
                });
              }
              catch (Csla.DataPortalException dpex)
              {
                var dummy = 5;
              }
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

  }
}