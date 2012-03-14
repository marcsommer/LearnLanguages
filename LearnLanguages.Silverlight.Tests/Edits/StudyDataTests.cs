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
  [Tag("study")]
  public class StudyDataTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    
    //[ClassInitialize]
    //[Asynchronous]
    //public void InitializeStudyDataTests()
    //{

    //  //WE NEED TO UPDATE THE CLIENT SeedData.Ton IDS.  
    //  var isLoaded = false;
    //  var StudyDatasCorrected = false;
    //  Exception error = new Exception();
    //  Exception errorStudyDataList = new Exception();
    //  LanguageList allLanguages = null;
    //  StudyDataList allStudyDatas = null;

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

    //    StudyDataList.GetAll((s2, r2) =>
    //      {
    //        errorStudyDataList = r2.Error;
    //        if (errorStudyDataList != null)
    //          throw errorStudyDataList;

    //        allStudyDatas = r2.Object;

    //        var serverHelloStudyDataQuery = (from StudyData in allStudyDatas
    //                                 where StudyData.Text == SeedData.Ton.HelloText && 
    //                                       StudyData.Language.Text == SeedData.Ton.EnglishText
    //                                 select StudyData);
    //        StudyDataEdit serverHelloStudyData = null;
    //        if (serverHelloStudyDataQuery.Count() == 0) //we don't have the hello StudyData in the db, so put it there
    //        {
    //          var StudyData = allStudyDatas[0];
    //          StudyData.BeginEdit();
    //          StudyData.Text = SeedData.Ton.HelloText;
    //          StudyData.Language = _ServerEnglishLang;
    //          StudyData.ApplyEdit();
    //          serverHelloStudyData = StudyData;
    //        }
    //        else
    //          serverHelloStudyData = serverHelloStudyDataQuery.First();


    //        var serverHolaStudyDataQuery = (from StudyData in allStudyDatas
    //                                      where StudyData.Text == SeedData.Ton.HolaText &&
    //                                            StudyData.Language.Text == SeedData.Ton.EnglishText
    //                                      select StudyData);
    //        StudyDataEdit serverHolaStudyData = null;
    //        if (serverHolaStudyDataQuery.Count() == 0) //we don't have the Hola StudyData in the db, so put it there
    //        {
    //          var StudyData = allStudyDatas[1];
    //          StudyData.BeginEdit();
    //          StudyData.Text = SeedData.Ton.HolaText;
    //          StudyData.Language = _ServerSpanishLang;
    //          StudyData.ApplyEdit();
    //          serverHolaStudyData = StudyData;
    //        }
    //        else
    //          serverHolaStudyData = serverHolaStudyDataQuery.First();

    //        var serverLongStudyDataQuery = (from StudyData in allStudyDatas
    //                                     where StudyData.Text == SeedData.Ton.LongText &&
    //                                           StudyData.Language.Text == SeedData.Ton.EnglishText
    //                                     select StudyData);
    //        StudyDataEdit serverLongStudyData = null;
    //        if (serverLongStudyDataQuery.Count() == 0) //we don't have the Long StudyData in the db, so put it there
    //        {
    //          var StudyData = allStudyDatas[2];
    //          StudyData.BeginEdit();
    //          StudyData.Text = SeedData.Ton.LongText;
    //          StudyData.Language = _ServerEnglishLang;
    //          StudyData.ApplyEdit();
    //          serverLongStudyData = StudyData;
    //        }
    //        else
    //          serverLongStudyData = serverLongStudyDataQuery.First();


    //        var serverDogStudyDataQuery = (from StudyData in allStudyDatas
    //                                     where StudyData.Text == SeedData.Ton.DogText &&
    //                                           StudyData.Language.Text == SeedData.Ton.EnglishText
    //                                     select StudyData);
    //        StudyDataEdit serverDogStudyData = null;
    //        if (serverDogStudyDataQuery.Count() == 0) //we don't have the Dog StudyData in the db, so put it there
    //        {
    //          var StudyData = allStudyDatas[3];
    //          StudyData.BeginEdit();
    //          StudyData.Text = SeedData.Ton.DogText;
    //          StudyData.Language = _ServerSpanishLang;
    //          StudyData.ApplyEdit();
    //          serverDogStudyData = StudyData;
    //        }
    //        else
    //          serverDogStudyData = serverDogStudyDataQuery.First();

    //        var validUserId = serverHelloStudyData.UserId;
    //        SeedData.Ton.GetTestValidUserDto().Id = validUserId;

    //        SeedData.Ton.HelloStudyDataDto.Id = serverHelloStudyData.Id;
    //        SeedData.Ton.HolaStudyDataDto.Id = serverHolaStudyData.Id;
    //        SeedData.Ton.LongStudyDataDto.Id = serverLongStudyData.Id;
    //        SeedData.Ton.DogStudyDataDto.Id = serverDogStudyData.Id;

    //        SeedData.Ton.HelloStudyDataDto.UserId = serverHelloStudyData.UserId;
    //        SeedData.Ton.HolaStudyDataDto.UserId = serverHolaStudyData.UserId;
    //        SeedData.Ton.LongStudyDataDto.UserId = serverLongStudyData.UserId;
    //        SeedData.Ton.DogStudyDataDto.UserId = serverDogStudyData.UserId;

    //        StudyDatasCorrected = true;
    //      });
    //  });

    //  EnqueueConditional(() => isLoaded);
    //  EnqueueConditional(() => StudyDatasCorrected);
    //  EnqueueCallback(() => { Assert.IsNull(error); },
    //                  () => { Assert.IsNotNull(allLanguages); },
    //                  () => { Assert.AreNotEqual(Guid.Empty, SeedData.Ton.EnglishId); },
    //                  () => { Assert.AreNotEqual(Guid.Empty, SeedData.Ton.SpanishId); },
    //                  () => { Assert.IsTrue(allLanguages.Count > 0); });
    //  EnqueueTestComplete();
    //}

    //[TestMethod]
    //[Asynchronous]
    //public void CREATE_NEW()
    //{
    //  var isCreated = false;
    //  StudyDataEdit newStudyDataEdit = null;
    //  Exception newError = new Exception();
      
    //  StudyDataRetriever.CreateNew( (s,r) =>
    //    {
    //      newError = r.Error;
    //      if (newError != null)
    //        throw newError;

    //      newStudyDataEdit = r.Object.StudyData;
    //      isCreated = true;
    //    });
    //  EnqueueConditional(() => isCreated);
    //  EnqueueCallback(
    //                  () => { Assert.IsNotNull(newStudyDataEdit); },
    //                  () => { Assert.IsNull(newError); },
    //                  () => { Assert.IsNotNull(newStudyDataEdit.User); },
    //                  () => { Assert.IsFalse(string.IsNullOrEmpty(newStudyDataEdit.Username)); }
    //                  );
    //  EnqueueTestComplete();
    //}

    //[TestMethod]
    //[Asynchronous]
    //public void CREATE_NEW_WITH_ID()
    //{
    //  Guid id = new Guid("BDEF87AC-21FA-4BAE-A155-91CDDA52C9CD");
    
    //  var isCreated = false;
    //  StudyDataEdit StudyDataEdit = null;
    //  StudyDataEdit.NewStudyDataEdit(id, (s,r) =>
    //    {
    //      if (r.Error != null)
    //        throw r.Error;

    //      StudyDataEdit = r.Object;
    //      isCreated = true;
    //    });
    //  EnqueueConditional(() => isCreated);
    //  EnqueueCallback(() => { Assert.IsNotNull(StudyDataEdit); },
    //                  () => { Assert.IsNull(null); },
    //                  () => { Assert.AreEqual(id, StudyDataEdit.Id); });
    //  EnqueueTestComplete();
    //}

    [TestMethod]
    [Asynchronous]
    public void GET_VIA_RETRIEVER()
    {
      var isLoaded = false;
      Exception error = new Exception();
      StudyDataEdit studyDataEdit = null;
      StudyDataRetriever.CreateNew((s, r) =>
      //StudyDataEdit.GetStudyDataEditForCurrentUser(testId, (s, r) =>
      {
        error = r.Error;
        studyDataEdit = r.Object.StudyData;
        isLoaded = true;
      });

      EnqueueConditional(() => isLoaded);
      EnqueueCallback(
                      () => { Assert.IsNull(error); },
                      () => { Assert.IsNotNull(studyDataEdit); }
                     );
      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void NEW_EDIT_BEGINSAVE_GET()
    {
      //INITIALIZE ERRORS TO EXCEPTION, BECAUSE WE TEST IF THEY ARE NULL LATER
      Exception retrieveError = new Exception();
      Exception savedError = new Exception();
      Exception gottenError = new Exception();

      StudyDataEdit newStudyDataEdit = null;
      StudyDataEdit savedStudyDataEdit = null;
      StudyDataEdit gottenStudyDataEdit = null;

      var isRetrieved = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;
      var studyDataAlreadyExisted = false;

      var editedStudyDataText = "Spanish";

      //NEW
      StudyDataRetriever.CreateNew((s, r) =>
      {
        retrieveError = r.Error;
        if (retrieveError != null)
          throw retrieveError;
        newStudyDataEdit = r.Object.StudyData;
        isRetrieved = true;

        //EDIT
        newStudyDataEdit.NativeLanguageText = editedStudyDataText;
        //newStudyDataEdit.Username = SeedData.Ton.TestValidUsername;
        isEdited = true;

        //SAVE
        newStudyDataEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          savedStudyDataEdit = r2.NewObject as StudyDataEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          StudyDataRetriever.CreateNew((s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;

            gottenStudyDataEdit = r3.Object.StudyData;
            //STUDY DATA SHOULD HAVE ALREADY EXISTED AS WE JUST SAVED IT.
            studyDataAlreadyExisted = r3.Object.StudyDataAlreadyExisted;

            isGotten = true;
          });
        });



      });

      EnqueueConditional(() => isRetrieved);
      EnqueueConditional(() => isEdited);
      EnqueueConditional(() => isSaved);
      EnqueueConditional(() => isGotten);
      EnqueueCallback(
                      //NO ERRORS
                      () => { Assert.IsNull(retrieveError); },
                      () => { Assert.IsNull(savedError); },
                      () => { Assert.IsNull(gottenError); },

                      //NOT NULL
                      () => { Assert.IsNotNull(newStudyDataEdit); },
                      () => { Assert.IsNotNull(savedStudyDataEdit); },
                      () => { Assert.IsNotNull(gottenStudyDataEdit); },

                      //SAME IDS
                      () => { Assert.AreEqual(savedStudyDataEdit.Id, gottenStudyDataEdit.Id); },

                      //EDITED TEXT WAS PERSISTED AND THE GOTTEN STUDY DATA EDIT TEXT IS THE SAME
                      () => { Assert.AreEqual(editedStudyDataText, gottenStudyDataEdit.NativeLanguageText); },

                      //GOTTEN STUDY DATA CHECKS THAT THE STUDY DATA ALREADY EXISTED.  SHOULD BE TRUE AS WE SAVED IT.
                      () => { Assert.IsTrue(studyDataAlreadyExisted); },

                      //SAVED TEXT PERSISTED 
                      () =>
                      {
                        Assert.AreEqual(savedStudyDataEdit.NativeLanguageText,
                                        gottenStudyDataEdit.NativeLanguageText);
                      });

      EnqueueTestComplete();
    }

    //[TestMethod]
    //[Asynchronous]
    //[ExpectedException(typeof(ExpectedException))]
    //public void NEW_EDIT_BEGINSAVE_GET_DELETE_GET()
    //{
    //  //INITIALIZE ERRORS TO EXCEPTION, BECAUSE EXPECT THEM TO BE NULL LATER
    //  Exception newError = new Exception();
    //  Exception savedError = new Exception();
    //  Exception gottenError = new Exception();
    //  Exception deletedError = new Exception();

    //  //INITIALIZE CONFIRM TO NULL, BECAUSE WE EXPECT THIS TO BE AN ERROR LATER
    //  Exception deleteConfirmedError = null;

    //  StudyDataEdit newStudyDataEdit = null;
    //  StudyDataEdit savedStudyDataEdit = null;
    //  StudyDataEdit gottenStudyDataEdit = null;
    //  StudyDataEdit deletedStudyDataEdit = null;

    //  //INITIALIZE TO EMPTY StudyData EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
    //  StudyDataEdit deleteConfirmedStudyDataEdit = new StudyDataEdit();

    //  var isNewed = false;
    //  var isSaved = false;
    //  var isGotten = false;
    //  var isDeleted = false;
    //  var isDeleteConfirmed = false;

    //  //NEW
    //  StudyDataEdit.NewStudyDataEdit((s, r) =>
    //  {
    //    newError = r.Error;
    //    if (newError != null)
    //      throw newError;
    //    newStudyDataEdit = r.Object;
    //    isNewed = true;

    //    //EDIT
    //    newStudyDataEdit.NativeLanguageText = "Spanish";
    //    newStudyDataEdit.Username = SeedData.Ton.TestValidUsername;

    //    //SAVE
    //    newStudyDataEdit.BeginSave((s2, r2) =>
    //    {
    //      savedError = r2.Error;
    //      if (savedError != null)
    //        throw savedError;
    //      savedStudyDataEdit = r2.NewObject as StudyDataEdit;
    //      isSaved = true;
    //      //GET (CONFIRM SAVE)
    //      StudyDataEdit.GetStudyDataEditForCurrentUser(savedStudyDataEdit.Id, (s3, r3) =>
    //      {
    //        gottenError = r3.Error;
    //        if (gottenError != null)
    //          throw gottenError;
    //        gottenStudyDataEdit = r3.Object;
    //        isGotten = true;

    //        //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
    //        gottenStudyDataEdit.Delete();
    //        gottenStudyDataEdit.BeginSave((s4, r4) =>
    //        {
    //          deletedError = r4.Error;
    //          if (deletedError != null)
    //            throw deletedError;

    //          deletedStudyDataEdit = r4.NewObject as StudyDataEdit;
    //          //TODO: Figure out why StudyDataEditTests final callback gets thrown twice.  When server throws an exception (expected because we are trying to fetch a deleted StudyData that shouldn't exist), the callback is executed twice.
    //          StudyDataEdit.GetStudyDataEditForCurrentUser(deletedStudyDataEdit.Id, (s5, r5) =>
    //          {
    //            var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
    //            var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
    //            deleteConfirmedError = r5.Error;
    //            if (deleteConfirmedError != null)
    //            {
    //              isDeleteConfirmed = true;
    //              throw new ExpectedException(deleteConfirmedError);
    //            }
    //            deleteConfirmedStudyDataEdit = r5.Object;
    //          });
    //        });
    //      });
    //    });
    //  });

    //  EnqueueConditional(() => isNewed);
    //  EnqueueConditional(() => isSaved);
    //  EnqueueConditional(() => isGotten);
    //  EnqueueConditional(() => isDeleted);
    //  EnqueueConditional(() => isDeleteConfirmed);
    //  EnqueueCallback(
    //                  () => { Assert.IsNull(newError); },
    //                  () => { Assert.IsNull(savedError); },
    //                  () => { Assert.IsNull(gottenError); },
    //                  () => { Assert.IsNull(deletedError); },
    //                  //WE EXPECT AN ERROR, AS WE TRIED A GET ON AN ID THAT SHOULD HAVE BEEN DELETED
    //                  () => { Assert.IsNotNull(deleteConfirmedError); },

    //                  () => { Assert.IsNotNull(newStudyDataEdit); },
    //                  () => { Assert.IsNotNull(savedStudyDataEdit); },
    //                  () => { Assert.IsNotNull(gottenStudyDataEdit); },
    //                  () => { Assert.IsNotNull(deletedStudyDataEdit); },
    //                  () => { Assert.IsNull(deleteConfirmedStudyDataEdit); });

    //  EnqueueTestComplete();
    //}

  }
}