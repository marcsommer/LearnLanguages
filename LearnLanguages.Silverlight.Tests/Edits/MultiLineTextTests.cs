using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;
using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Linq;
using Csla;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("mlt")]
  public class MultiLineTextTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    private LanguageEdit _ServerEnglishLang;
    private LanguageEdit _ServerSpanishLang;

    //[ClassInitialize]
    //[Asynchronous]
    //public void InitializeMultiLineTextTests()
    //{

    //  //WE NEED TO UPDATE THE CLIENT SeedData.Ton IDS.  
    //  var isLoaded = false;
    //  var multiLineTextsCorrected = false;
    //  Exception error = new Exception();
    //  Exception errorMultiLineTextList = new Exception();
    //  LanguageList allLanguages = null;
    //  MultiLineTextList allMultiLineTexts = null;

    //  LanguageList.GetAll((s, r) =>
    //  {
    //    #region Initialize Language Data
    //    error = r.Error;
    //    if (error != null)
    //      throw error;

    //    allLanguages = r.Object;
    //    _ServerEnglishLang = (from language in allLanguages
    //                          where language.Text == SeedData.Ton.EnglishText
    //                          select language).First();

    //    SeedData.Ton.EnglishLanguageDto.Id = _ServerEnglishLang.Id;

    //    _ServerSpanishLang = (from language in allLanguages
    //                          where language.Text == SeedData.Ton.SpanishText
    //                          select language).First();

    //    SeedData.Ton.SpanishLanguageDto.Id = _ServerSpanishLang.Id;

    //    #endregion

    //    isLoaded = true;

    //    MultiLineTextList.GetAll((s2, r2) =>
    //      {
    //        errorMultiLineTextList = r2.Error;
    //        if (errorMultiLineTextList != null)
    //          throw errorMultiLineTextList;

    //        allMultiLineTexts = r2.Object;

    //        var serverHelloMultiLineTextQuery = (from multiLineText in allMultiLineTexts
    //                                           where multiLineText.Text == SeedData.Ton.HelloText &&
    //                                                 multiLineText.Language.Text == SeedData.Ton.EnglishText
    //                                           select multiLineText);
    //        MultiLineTextEdit serverHelloMultiLineText = null;
    //        if (serverHelloMultiLineTextQuery.Count() == 0) //we don't have the hello multiLineText in the db, so put it there
    //        {
    //          var multiLineText = allMultiLineTexts[0];
    //          multiLineText.BeginEdit();
    //          multiLineText.Text = SeedData.Ton.HelloText;
    //          multiLineText.Language = _ServerEnglishLang;
    //          multiLineText.ApplyEdit();
    //          serverHelloMultiLineText = multiLineText;
    //        }
    //        else
    //          serverHelloMultiLineText = serverHelloMultiLineTextQuery.First();


    //        var serverHolaMultiLineTextQuery = (from multiLineText in allMultiLineTexts
    //                                          where multiLineText.Text == SeedData.Ton.HolaText &&
    //                                                multiLineText.Language.Text == SeedData.Ton.EnglishText
    //                                          select multiLineText);
    //        MultiLineTextEdit serverHolaMultiLineText = null;
    //        if (serverHolaMultiLineTextQuery.Count() == 0) //we don't have the Hola multiLineText in the db, so put it there
    //        {
    //          var multiLineText = allMultiLineTexts[1];
    //          multiLineText.BeginEdit();
    //          multiLineText.Text = SeedData.Ton.HolaText;
    //          multiLineText.Language = _ServerSpanishLang;
    //          multiLineText.ApplyEdit();
    //          serverHolaMultiLineText = multiLineText;
    //        }
    //        else
    //          serverHolaMultiLineText = serverHolaMultiLineTextQuery.First();

    //        var serverLongMultiLineTextQuery = (from multiLineText in allMultiLineTexts
    //                                          where multiLineText.Text == SeedData.Ton.LongText &&
    //                                                multiLineText.Language.Text == SeedData.Ton.EnglishText
    //                                          select multiLineText);
    //        MultiLineTextEdit serverLongMultiLineText = null;
    //        if (serverLongMultiLineTextQuery.Count() == 0) //we don't have the Long multiLineText in the db, so put it there
    //        {
    //          var multiLineText = allMultiLineTexts[2];
    //          multiLineText.BeginEdit();
    //          multiLineText.Text = SeedData.Ton.LongText;
    //          multiLineText.Language = _ServerEnglishLang;
    //          multiLineText.ApplyEdit();
    //          serverLongMultiLineText = multiLineText;
    //        }
    //        else
    //          serverLongMultiLineText = serverLongMultiLineTextQuery.First();


    //        var serverDogMultiLineTextQuery = (from multiLineText in allMultiLineTexts
    //                                         where multiLineText.Text == SeedData.Ton.DogText &&
    //                                               multiLineText.Language.Text == SeedData.Ton.EnglishText
    //                                         select multiLineText);
    //        MultiLineTextEdit serverDogMultiLineText = null;
    //        if (serverDogMultiLineTextQuery.Count() == 0) //we don't have the Dog multiLineText in the db, so put it there
    //        {
    //          var multiLineText = allMultiLineTexts[3];
    //          multiLineText.BeginEdit();
    //          multiLineText.Text = SeedData.Ton.DogText;
    //          multiLineText.Language = _ServerSpanishLang;
    //          multiLineText.ApplyEdit();
    //          serverDogMultiLineText = multiLineText;
    //        }
    //        else
    //          serverDogMultiLineText = serverDogMultiLineTextQuery.First();

    //        var validUserId = serverHelloMultiLineText.UserId;
    //        SeedData.Ton.GetTestValidUserDto().Id = validUserId;

    //        SeedData.Ton.HelloMultiLineTextDto.Id = serverHelloMultiLineText.Id;
    //        SeedData.Ton.HolaMultiLineTextDto.Id = serverHolaMultiLineText.Id;
    //        SeedData.Ton.LongMultiLineTextDto.Id = serverLongMultiLineText.Id;
    //        SeedData.Ton.DogMultiLineTextDto.Id = serverDogMultiLineText.Id;

    //        SeedData.Ton.HelloMultiLineTextDto.UserId = serverHelloMultiLineText.UserId;
    //        SeedData.Ton.HolaMultiLineTextDto.UserId = serverHolaMultiLineText.UserId;
    //        SeedData.Ton.LongMultiLineTextDto.UserId = serverLongMultiLineText.UserId;
    //        SeedData.Ton.DogMultiLineTextDto.UserId = serverDogMultiLineText.UserId;

    //        multiLineTextsCorrected = true;
    //      });
    //  });

    //  EnqueueConditional(() => isLoaded);
    //  EnqueueConditional(() => multiLineTextsCorrected);
    //  EnqueueCallback(() => { Assert.IsNull(error); },
    //                  () => { Assert.IsNotNull(allLanguages); },
    //                  () => { Assert.AreNotEqual(Guid.Empty, SeedData.Ton.EnglishId); },
    //                  () => { Assert.AreNotEqual(Guid.Empty, SeedData.Ton.SpanishId); },
    //                  () => { Assert.IsTrue(allLanguages.Count > 0); });
    //  EnqueueTestComplete();
    //}

    [TestMethod]
    [Asynchronous]
    public void CREATE_NEW()
    {
      var isCreated = false;
      MultiLineTextEdit newMultiLineTextEdit = null;
      Exception newError = new Exception();

      MultiLineTextEdit.NewMultiLineTextEdit((s, r) =>
        {
          newError = r.Error;
          if (newError != null)
            throw newError;

          newMultiLineTextEdit = r.Object;
          Assert.IsTrue(newMultiLineTextEdit.User.IsAuthenticated);
          Assert.AreEqual(Csla.ApplicationContext.User.Identity.Name, newMultiLineTextEdit.Username);
          isCreated = true;
        });
      EnqueueConditional(() => isCreated);
      EnqueueCallback(
                      () => { Assert.IsNotNull(newMultiLineTextEdit); },
                      () => { Assert.IsNull(newError); }
                      );
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void GET()
    {
      Guid testId = Guid.Empty;
      var allLoaded = false;
      var isLoaded = false;
      Exception getAllError = new Exception();
      Exception error = new Exception();
      MultiLineTextEdit multiLineTextEdit = null;

      MultiLineTextList.GetAll((s1, r1) =>
        {
          getAllError = r1.Error;
          if (getAllError != null)
            throw r1.Error;

          testId = r1.Object.First().Id;
          allLoaded = true;
          MultiLineTextEdit.GetMultiLineTextEdit(testId, (s, r) =>
          {
            error = r.Error;
            multiLineTextEdit = r.Object;
            isLoaded = true;
          });
        });


      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => allLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNull(getAllError); },
                      () => { Assert.IsNotNull(multiLineTextEdit); },
                      () => { Assert.IsTrue(multiLineTextEdit.Lines.Count >= 2); },
        //() => { Assert.IsTrue(multiLineTextEdit.Lines.Count == multiLineTextEdit.LineIds.Count); },
                      () => { Assert.AreEqual(testId, multiLineTextEdit.Id); });
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void NEW_EDIT_BEGINSAVE_GET()
    {
      MultiLineTextEdit newMultiLineTextEdit = null;
      MultiLineTextEdit savedMultiLineTextEdit = null;
      MultiLineTextEdit gottenMultiLineTextEdit = null;

      var isNewed = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;

      string lineAText = "MultiLineTextTests.neweditbeginsaveget.Test Line A Text.  This is line A in English.";
      string lineBText = "MultiLineTextTests.neweditbeginsaveget.Test Line BBBB Text.  This is line B in Spanish.";
      LineEdit lineA = null;
      LineEdit lineB = null;
      
      //NEW
      MultiLineTextEdit.NewMultiLineTextEdit((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;
          newMultiLineTextEdit = r.Object;
          isNewed = true;

          //EDIT
          //newMultiLineTextEdit.Lines.add

          newMultiLineTextEdit.Lines.AddedNew += (s5, r5) =>
            {
              if (lineA == null)
              {
                lineA = r5.NewObject;
                lineA.Id = Guid.NewGuid();
                lineA.Phrase.Text = lineAText;
                lineA.Phrase.LanguageId = SeedData.Ton.EnglishId;
                //lineA.Username = Csla.ApplicationContext.User.Identity.Name;
                //lineA.UserId = SeedData.Ton.GetTestValidUserDto().Id;
              }
              else
              {
                lineB = r5.NewObject;
                lineB.Id = Guid.NewGuid();
                lineB.Phrase.Text = lineBText;
                lineB.Phrase.LanguageId = SeedData.Ton.SpanishId;
                //lineB.Username = Csla.ApplicationContext.User.Identity.Name;
                //lineB.UserId = SeedData.Ton.GetTestValidUserDto().Id;
              }
            };

          newMultiLineTextEdit.Lines.AddNew();
          newMultiLineTextEdit.Lines.AddNew();
          isEdited = true;

          //newMultiLineTextEdit.AddLine(lineA);
          //newMultiLineTextEdit.AddLine(lineB);

          //SAVE
          newMultiLineTextEdit.BeginSave((s2, r2) =>
            {
              if (r2.Error != null)
                throw r2.Error;

              savedMultiLineTextEdit = (MultiLineTextEdit)r2.NewObject;
              isSaved = true;

              //GET (CONFIRM SAVE)
              MultiLineTextEdit.GetMultiLineTextEdit(savedMultiLineTextEdit.Id, (s3, r3) =>
                {
                  if (r3.Error != null)
                    throw r3.Error;

                  gottenMultiLineTextEdit = r3.Object;
                  isGotten = true;
                });
            });
        });

      EnqueueConditional(() => isNewed);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueCallback(
                      () => { Assert.IsNotNull(newMultiLineTextEdit); },
                      () => { Assert.IsNotNull(savedMultiLineTextEdit); },
                      () => { Assert.IsNotNull(gottenMultiLineTextEdit); },
                      () => { Assert.IsNotNull(lineA); },
                      () => { Assert.IsNotNull(lineB); },
                      () => { Assert.IsTrue(gottenMultiLineTextEdit.Lines.Count >= 2); },
                      () => { Assert.AreEqual(savedMultiLineTextEdit.Id, gottenMultiLineTextEdit.Id); }
                     );

      EnqueueTestComplete();
    }

    //[TestMethod]
    //[Asynchronous]
    //[ExpectedException(typeof(ExpectedException))]
    //public void NEW_EDIT_BEGINSAVE_GET_DELETE_GET()
    //{
    //  MultiLineTextEdit newMultiLineTextEdit = null;
    //  MultiLineTextEdit savedMultiLineTextEdit = null;
    //  MultiLineTextEdit gottenMultiLineTextEdit = null;
    //  MultiLineTextEdit deletedMultiLineTextEdit = null;

    //  LineEdit lineA = null;
    //  LineEdit lineB = null;

    //  //INITIALIZE TO EMPTY TRANSLATIONEDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
    //  MultiLineTextEdit deleteConfirmedMultiLineTextEdit = new MultiLineTextEdit();

    //  var isNewed = false;
    //  var isEdited = false;
    //  var isSaved = false;
    //  var isGotten = false;
    //  var isDeleted = false;
    //  var isDeleteConfirmed = false;

    //  //NEW
    //  MultiLineTextEdit.NewMultiLineTextEdit((sNew, rNew) =>
    //  {
    //    if (rNew.Error != null)
    //      throw rNew.Error;
    //    newMultiLineTextEdit = rNew.Object;
    //    isNewed = true;

    //    //EDIT
    //    newMultiLineTextEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
    //    newMultiLineTextEdit.Username = SeedData.Ton.TestValidUsername;
    //    LineEdit.NewLineEdit((sNewLineA, rNewLineA) =>
    //    {
    //      if (rNewLineA.Error != null)
    //        throw rNewLineA.Error;

    //      lineA = rNewLineA.Object;
    //      lineA.Text = "Test Line A Text.  This is line A in English.";
    //      lineA.LanguageId = SeedData.Ton.EnglishId;
    //      newMultiLineTextEdit.AddLine(lineA);

    //      LineEdit.NewLineEdit((sNewLineB, rNewLineB) =>
    //      {
    //        if (rNewLineB.Error != null)
    //          throw rNewLineB.Error;

    //        lineB = rNewLineB.Object;
    //        lineB.Text = "Test Line B Text.  This is line B in Spanish.";
    //        lineB.LanguageId = SeedData.Ton.SpanishId;
    //        newMultiLineTextEdit.AddLine(lineB);

    //        isEdited = true;

    //        //SAVE
    //        newMultiLineTextEdit.BeginSave((sSave, rSave) =>
    //        {
    //          if (rSave.Error!= null)
    //            throw rSave.Error;
    //          savedMultiLineTextEdit = (MultiLineTextEdit)rSave.NewObject;

    //          isSaved = true;

    //          //GET (CONFIRM SAVE)
    //          MultiLineTextEdit.GetMultiLineTextEdit(savedMultiLineTextEdit.Id, (sGet, rGet) =>
    //          {
    //            if (rGet.Error!= null)
    //              throw rGet.Error;

    //            gottenMultiLineTextEdit = rGet.Object;
    //            isGotten = true;

    //            //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
    //            gottenMultiLineTextEdit.Delete();
    //            gottenMultiLineTextEdit.BeginSave((sSaveGotten, rSaveGotten) =>
    //            {
    //              if (rSaveGotten.Error != null)
    //                throw rSaveGotten.Error;

    //              deletedMultiLineTextEdit = (MultiLineTextEdit)rSaveGotten.NewObject;
    //              //TODO: Figure out why MultiLineTextEditTests final callback gets thrown twice.  When server throws an exception (expected because we are trying to fetch a deleted multiLineText that shouldn't exist), the callback is executed twice.

    //              MultiLineTextEdit.GetMultiLineTextEdit(deletedMultiLineTextEdit.Id, (sGetDeleted, rGetDeleted) =>
    //              {
    //                var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
    //                var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
    //                if (rGetDeleted.Error != null)
    //                {
    //                  isDeleteConfirmed = true;
    //                  throw new ExpectedException(rGetDeleted.Error);
    //                }
    //                deleteConfirmedMultiLineTextEdit = rGetDeleted.Object;
    //              });
    //            });
    //          });
    //        });
    //      });
    //    });
    //  });

    //  EnqueueConditional(() => isNewed);
    //  EnqueueConditional(() => isEdited);
    //  EnqueueConditional(() => isSaved);
    //  EnqueueConditional(() => isGotten);
    //  EnqueueConditional(() => isDeleted);
    //  EnqueueConditional(() => isDeleteConfirmed);
    //  EnqueueCallback(
    //                  () => { Assert.IsNotNull(newMultiLineTextEdit); },
    //                  () => { Assert.IsNotNull(savedMultiLineTextEdit); },
    //                  () => { Assert.IsNotNull(gottenMultiLineTextEdit); },
    //                  () => { Assert.IsNotNull(deletedMultiLineTextEdit); },
    //                  () => { Assert.IsNull(deleteConfirmedMultiLineTextEdit); });

    //  EnqueueTestComplete();
    //}

  }
}