using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;
using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;
using System.Linq;
using LearnLanguages.Study;

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

    //  //WE NEED TO UPDATE THE CLIENT SEEDDATA.INSTANCE IDS.  
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
    //                          where language.Text == SeedData.Instance.EnglishText
    //                          select language).First();

    //    SeedData.Instance.EnglishLanguageDto.Id = _ServerEnglishLang.Id;

    //    _ServerSpanishLang = (from language in allLanguages
    //                          where language.Text == SeedData.Instance.SpanishText
    //                          select language).First();

    //    SeedData.Instance.SpanishLanguageDto.Id = _ServerSpanishLang.Id;

    //    #endregion

    //    isLoaded = true;

    //    StudyDataList.GetAll((s2, r2) =>
    //      {
    //        errorStudyDataList = r2.Error;
    //        if (errorStudyDataList != null)
    //          throw errorStudyDataList;

    //        allStudyDatas = r2.Object;

    //        var serverHelloStudyDataQuery = (from StudyData in allStudyDatas
    //                                 where StudyData.Text == SeedData.Instance.HelloText && 
    //                                       StudyData.Language.Text == SeedData.Instance.EnglishText
    //                                 select StudyData);
    //        StudyDataEdit serverHelloStudyData = null;
    //        if (serverHelloStudyDataQuery.Count() == 0) //we don't have the hello StudyData in the db, so put it there
    //        {
    //          var StudyData = allStudyDatas[0];
    //          StudyData.BeginEdit();
    //          StudyData.Text = SeedData.Instance.HelloText;
    //          StudyData.Language = _ServerEnglishLang;
    //          StudyData.ApplyEdit();
    //          serverHelloStudyData = StudyData;
    //        }
    //        else
    //          serverHelloStudyData = serverHelloStudyDataQuery.First();


    //        var serverHolaStudyDataQuery = (from StudyData in allStudyDatas
    //                                      where StudyData.Text == SeedData.Instance.HolaText &&
    //                                            StudyData.Language.Text == SeedData.Instance.EnglishText
    //                                      select StudyData);
    //        StudyDataEdit serverHolaStudyData = null;
    //        if (serverHolaStudyDataQuery.Count() == 0) //we don't have the Hola StudyData in the db, so put it there
    //        {
    //          var StudyData = allStudyDatas[1];
    //          StudyData.BeginEdit();
    //          StudyData.Text = SeedData.Instance.HolaText;
    //          StudyData.Language = _ServerSpanishLang;
    //          StudyData.ApplyEdit();
    //          serverHolaStudyData = StudyData;
    //        }
    //        else
    //          serverHolaStudyData = serverHolaStudyDataQuery.First();

    //        var serverLongStudyDataQuery = (from StudyData in allStudyDatas
    //                                     where StudyData.Text == SeedData.Instance.LongText &&
    //                                           StudyData.Language.Text == SeedData.Instance.EnglishText
    //                                     select StudyData);
    //        StudyDataEdit serverLongStudyData = null;
    //        if (serverLongStudyDataQuery.Count() == 0) //we don't have the Long StudyData in the db, so put it there
    //        {
    //          var StudyData = allStudyDatas[2];
    //          StudyData.BeginEdit();
    //          StudyData.Text = SeedData.Instance.LongText;
    //          StudyData.Language = _ServerEnglishLang;
    //          StudyData.ApplyEdit();
    //          serverLongStudyData = StudyData;
    //        }
    //        else
    //          serverLongStudyData = serverLongStudyDataQuery.First();


    //        var serverDogStudyDataQuery = (from StudyData in allStudyDatas
    //                                     where StudyData.Text == SeedData.Instance.DogText &&
    //                                           StudyData.Language.Text == SeedData.Instance.EnglishText
    //                                     select StudyData);
    //        StudyDataEdit serverDogStudyData = null;
    //        if (serverDogStudyDataQuery.Count() == 0) //we don't have the Dog StudyData in the db, so put it there
    //        {
    //          var StudyData = allStudyDatas[3];
    //          StudyData.BeginEdit();
    //          StudyData.Text = SeedData.Instance.DogText;
    //          StudyData.Language = _ServerSpanishLang;
    //          StudyData.ApplyEdit();
    //          serverDogStudyData = StudyData;
    //        }
    //        else
    //          serverDogStudyData = serverDogStudyDataQuery.First();

    //        var validUserId = serverHelloStudyData.UserId;
    //        SeedData.Instance.GetTestValidUserDto().Id = validUserId;

    //        SeedData.Instance.HelloStudyDataDto.Id = serverHelloStudyData.Id;
    //        SeedData.Instance.HolaStudyDataDto.Id = serverHolaStudyData.Id;
    //        SeedData.Instance.LongStudyDataDto.Id = serverLongStudyData.Id;
    //        SeedData.Instance.DogStudyDataDto.Id = serverDogStudyData.Id;

    //        SeedData.Instance.HelloStudyDataDto.UserId = serverHelloStudyData.UserId;
    //        SeedData.Instance.HolaStudyDataDto.UserId = serverHolaStudyData.UserId;
    //        SeedData.Instance.LongStudyDataDto.UserId = serverLongStudyData.UserId;
    //        SeedData.Instance.DogStudyDataDto.UserId = serverDogStudyData.UserId;

    //        StudyDatasCorrected = true;
    //      });
    //  });

    //  EnqueueConditional(() => isLoaded);
    //  EnqueueConditional(() => StudyDatasCorrected);
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
      StudyDataEdit newStudyDataEdit = null;
      Exception newError = new Exception();
      
      StudyDataEdit.NewStudyDataEdit( (s,r) =>
        {
          newError = r.Error;
          if (newError != null)
            throw newError;

          newStudyDataEdit = r.Object;
          isCreated = true;
        });
      EnqueueConditional(() => isCreated);
      EnqueueCallback(
                      () => { Assert.IsNotNull(newStudyDataEdit); },
                      () => { Assert.IsNull(newError); },
                      () => { Assert.IsNotNull(newStudyDataEdit.User); },
                      () => { Assert.IsFalse(string.IsNullOrEmpty(newStudyDataEdit.Username)); }
                      );
      EnqueueTestComplete();
    }

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
    public void GET()
    {
      Guid testId = Guid.Empty;
      var allLoaded = false;
      var isLoaded = false;
      Exception getAllError = new Exception();
      Exception error = new Exception();
      StudyDataEdit studyDataEdit = null;

      StudyDataList.GetAll((s1, r1) =>
        {
          getAllError = r1.Error;
          if (getAllError != null)
            throw r1.Error;
          testId = r1.Object.First().Id;
          allLoaded = true;
          StudyDataEdit.GetStudyDataEdit(testId, (s, r) =>
          {
            error = r.Error;
            studyDataEdit = r.Object;
            isLoaded = true;
          });
        });


      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => allLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNull(getAllError); },
                      () => { Assert.IsNotNull(studyDataEdit); },
                      () => { Assert.AreEqual(testId, studyDataEdit.Id); });
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

      StudyDataEdit newStudyDataEdit = null;
      StudyDataEdit savedStudyDataEdit = null;
      StudyDataEdit gottenStudyDataEdit = null;    
  
      var isNewed = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;
      
      //NEW
      StudyDataEdit.NewStudyDataEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newStudyDataEdit = r.Object;
        isNewed = true;

        //EDIT
        newStudyDataEdit.NativeLanguageText = "Spanish";
        //newStudyDataEdit.Username = SeedData.Instance.TestValidUsername;
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
          StudyDataEdit.GetStudyDataEdit(savedStudyDataEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;
            gottenStudyDataEdit = r3.Object;
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
                      () => { Assert.IsNotNull(newStudyDataEdit); },
                      () => { Assert.IsNotNull(savedStudyDataEdit); },
                      () => { Assert.IsNotNull(gottenStudyDataEdit); },
                      () => { Assert.AreEqual(savedStudyDataEdit.Id, gottenStudyDataEdit.Id); },
                      () => { Assert.AreEqual(savedStudyDataEdit.NativeLanguageText,
                                              gottenStudyDataEdit.NativeLanguageText); });

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

      StudyDataEdit newStudyDataEdit = null;
      StudyDataEdit savedStudyDataEdit = null;
      StudyDataEdit gottenStudyDataEdit = null;
      StudyDataEdit deletedStudyDataEdit = null;

      //INITIALIZE TO EMPTY StudyData EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
      StudyDataEdit deleteConfirmedStudyDataEdit = new StudyDataEdit();

      var isNewed = false;
      var isSaved = false;
      var isGotten = false;
      var isDeleted = false;
      var isDeleteConfirmed = false;

      //NEW
      StudyDataEdit.NewStudyDataEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newStudyDataEdit = r.Object;
        isNewed = true;

        //EDIT
        newStudyDataEdit.NativeLanguageText = "Spanish";
        newStudyDataEdit.Username = SeedData.Instance.TestValidUsername;

        //SAVE
        newStudyDataEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          savedStudyDataEdit = r2.NewObject as StudyDataEdit;
          isSaved = true;
          //GET (CONFIRM SAVE)
          StudyDataEdit.GetStudyDataEdit(savedStudyDataEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;
            gottenStudyDataEdit = r3.Object;
            isGotten = true;

            //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
            gottenStudyDataEdit.Delete();
            gottenStudyDataEdit.BeginSave((s4, r4) =>
            {
              deletedError = r4.Error;
              if (deletedError != null)
                throw deletedError;

              deletedStudyDataEdit = r4.NewObject as StudyDataEdit;
              //TODO: Figure out why StudyDataEditTests final callback gets thrown twice.  When server throws an exception (expected because we are trying to fetch a deleted StudyData that shouldn't exist), the callback is executed twice.
              StudyDataEdit.GetStudyDataEdit(deletedStudyDataEdit.Id, (s5, r5) =>
              {
                var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
                var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
                deleteConfirmedError = r5.Error;
                if (deleteConfirmedError != null)
                {
                  isDeleteConfirmed = true;
                  throw new ExpectedException(deleteConfirmedError);
                }
                deleteConfirmedStudyDataEdit = r5.Object;
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

                      () => { Assert.IsNotNull(newStudyDataEdit); },
                      () => { Assert.IsNotNull(savedStudyDataEdit); },
                      () => { Assert.IsNotNull(gottenStudyDataEdit); },
                      () => { Assert.IsNotNull(deletedStudyDataEdit); },
                      () => { Assert.IsNull(deleteConfirmedStudyDataEdit); });

      EnqueueTestComplete();
    }

  }
}