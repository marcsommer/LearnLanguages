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
  [Tag("line")]
  public class LineTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    private LanguageEdit _ServerEnglishLang;
    //private LanguageEdit _ServerSpanishLang;

    [ClassInitialize]
    [Asynchronous]
    public void InitializeLineTests()
    {
      var isLoaded = false;
      LanguageEdit.GetLanguageEdit("English", (s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          _ServerEnglishLang = r.Object;
          isLoaded = true;
        });

      EnqueueConditional(() => isLoaded);
      EnqueueCallback(() => { Assert.IsNotNull(_ServerEnglishLang); });
      EnqueueTestComplete();
    }

    //  //WE NEED TO UPDATE THE CLIENT SeedData.Ton IDS.  
    //  var linesCorrected = false;
    //  Exception error = new Exception();
    //  Exception errorLineList = new Exception();
    //  LanguageList allLanguages = null;
    //  LineList allLines = null;

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

    //    LineList.GetAll((s2, r2) =>
    //      {
    //        errorLineList = r2.Error;
    //        if (errorLineList != null)
    //          throw errorLineList;

    //        allLines = r2.Object;

    //        var serverHelloLineQuery = (from line in allLines
    //                                 where line.Text == SeedData.Ton.HelloText && 
    //                                       line.Language.Text == SeedData.Ton.EnglishText
    //                                 select line);
    //        LineEdit serverHelloLine = null;
    //        if (serverHelloLineQuery.Count() == 0) //we don't have the hello line in the db, so put it there
    //        {
    //          var line = allLines[0];
    //          line.BeginEdit();
    //          line.Text = SeedData.Ton.HelloText;
    //          line.Language = _ServerEnglishLang;
    //          line.ApplyEdit();
    //          serverHelloLine = line;
    //        }
    //        else
    //          serverHelloLine = serverHelloLineQuery.First();


    //        var serverHolaLineQuery = (from line in allLines
    //                                      where line.Text == SeedData.Ton.HolaText &&
    //                                            line.Language.Text == SeedData.Ton.EnglishText
    //                                      select line);
    //        LineEdit serverHolaLine = null;
    //        if (serverHolaLineQuery.Count() == 0) //we don't have the Hola line in the db, so put it there
    //        {
    //          var line = allLines[1];
    //          line.BeginEdit();
    //          line.Text = SeedData.Ton.HolaText;
    //          line.Language = _ServerSpanishLang;
    //          line.ApplyEdit();
    //          serverHolaLine = line;
    //        }
    //        else
    //          serverHolaLine = serverHolaLineQuery.First();

    //        var serverLongLineQuery = (from line in allLines
    //                                     where line.Text == SeedData.Ton.LongText &&
    //                                           line.Language.Text == SeedData.Ton.EnglishText
    //                                     select line);
    //        LineEdit serverLongLine = null;
    //        if (serverLongLineQuery.Count() == 0) //we don't have the Long line in the db, so put it there
    //        {
    //          var line = allLines[2];
    //          line.BeginEdit();
    //          line.Text = SeedData.Ton.LongText;
    //          line.Language = _ServerEnglishLang;
    //          line.ApplyEdit();
    //          serverLongLine = line;
    //        }
    //        else
    //          serverLongLine = serverLongLineQuery.First();


    //        var serverDogLineQuery = (from line in allLines
    //                                     where line.Text == SeedData.Ton.DogText &&
    //                                           line.Language.Text == SeedData.Ton.EnglishText
    //                                     select line);
    //        LineEdit serverDogLine = null;
    //        if (serverDogLineQuery.Count() == 0) //we don't have the Dog line in the db, so put it there
    //        {
    //          var line = allLines[3];
    //          line.BeginEdit();
    //          line.Text = SeedData.Ton.DogText;
    //          line.Language = _ServerSpanishLang;
    //          line.ApplyEdit();
    //          serverDogLine = line;
    //        }
    //        else
    //          serverDogLine = serverDogLineQuery.First();

    //        var validUserId = serverHelloLine.UserId;
    //        SeedData.Ton.GetTestValidUserDto().Id = validUserId;

    //        SeedData.Ton.HelloLineDto.Id = serverHelloLine.Id;
    //        SeedData.Ton.HolaLineDto.Id = serverHolaLine.Id;
    //        SeedData.Ton.LongLineDto.Id = serverLongLine.Id;
    //        SeedData.Ton.DogLineDto.Id = serverDogLine.Id;

    //        SeedData.Ton.HelloLineDto.UserId = serverHelloLine.UserId;
    //        SeedData.Ton.HolaLineDto.UserId = serverHolaLine.UserId;
    //        SeedData.Ton.LongLineDto.UserId = serverLongLine.UserId;
    //        SeedData.Ton.DogLineDto.UserId = serverDogLine.UserId;

    //        linesCorrected = true;
    //      });
    //  });

    //  EnqueueConditional(() => isLoaded);
    //  EnqueueConditional(() => linesCorrected);
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
      LineEdit newLineEdit = null;
      Exception newError = new Exception();
      
      LineEdit.NewLineEdit( (s,r) =>
        {
          newError = r.Error;
          if (newError != null)
            throw newError;

          newLineEdit = r.Object;
          isCreated = true;
        });
      EnqueueConditional(() => isCreated);
      EnqueueCallback(
                      () => { Assert.IsNotNull(newLineEdit); },
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
    //  LineEdit LineEdit = null;
    //  LineEdit.NewLineEdit(id, (s,r) =>
    //    {
    //      if (r.Error != null)
    //        throw r.Error;

    //      LineEdit = r.Object;
    //      isCreated = true;
    //    });
    //  EnqueueConditional(() => isCreated);
    //  EnqueueCallback(() => { Assert.IsNotNull(LineEdit); },
    //                  () => { Assert.IsNull(null); },
    //                  () => { Assert.AreEqual(id, LineEdit.Id); });
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
      LineEdit LineEdit = null;

      LineList.GetAll((s1, r1) =>
        {
          getAllError = r1.Error;
          if (getAllError != null)
            throw r1.Error;
          testId = r1.Object.First().Id;
          allLoaded = true;
          LineEdit.GetLineEdit(testId, (s, r) =>
          {
            error = r.Error;
            LineEdit = r.Object;
            isLoaded = true;
          });
        });


      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => allLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNull(getAllError); },
                      () => { Assert.IsNotNull(LineEdit); },
                      () => { Assert.AreEqual(testId, LineEdit.Id); });
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

      LineEdit newLineEdit = null;
      LineEdit savedLineEdit = null;
      LineEdit gottenLineEdit = null;    
  
      var isNewed = false;
      var isEdited = false;
      var isSaved = false;
      var isGotten = false;
      
      //NEW
      LineEdit.NewLineEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newLineEdit = r.Object;
        isNewed = true;

        //EDIT
        newLineEdit.Phrase.Text = "TestLine.PhraseText NEW_EDIT_BEGINSAVE_GET";
        newLineEdit.Phrase.Language = _ServerEnglishLang;
        newLineEdit.LineNumber = 0;
        Assert.AreEqual(SeedData.Ton.TestValidUsername, newLineEdit.Username);

        //newLineEdit.UserId = SeedData.Ton.GetTestValidUserDto().Id;
        //newLineEdit.Username = SeedData.Ton.TestValidUsername;
        isEdited = true;

        //SAVE
        newLineEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;
          
          savedLineEdit = (LineEdit)r2.NewObject;
          isSaved = true;

          //GET (CONFIRM SAVE)
          LineEdit.GetLineEdit(savedLineEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;

            gottenLineEdit = r3.Object;
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
                      () => { Assert.IsNotNull(newLineEdit); },
                      () => { Assert.IsNotNull(savedLineEdit); },
                      () => { Assert.IsNotNull(gottenLineEdit); },
                      () => { Assert.AreEqual(savedLineEdit.Id, gottenLineEdit.Id); }
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

      LineEdit newLineEdit = null;
      LineEdit savedLineEdit = null;
      LineEdit gottenLineEdit = null;
      LineEdit deletedLineEdit = null;

      //INITIALIZE TO EMPTY Line EDIT, BECAUSE WE EXPECT THIS TO BE NULL LATER
      LineEdit deleteConfirmedLineEdit = new LineEdit();

      var isNewed = false;
      var isSaved = false;
      var isGotten = false;
      var isDeleted = false;
      var isDeleteConfirmed = false;

      //NEW
      LineEdit.NewLineEdit((s, r) =>
      {
        newError = r.Error;
        if (newError != null)
          throw newError;
        newLineEdit = r.Object;
        isNewed = true;

        //EDIT
        newLineEdit.Phrase.Text = "TestLine.PhraseText NEW_EDIT_BEGINSAVE_GET";
        newLineEdit.Phrase.Language = _ServerEnglishLang;
        newLineEdit.LineNumber = 0;
        Assert.AreEqual(SeedData.Ton.TestValidUsername, newLineEdit.Username);

        //SAVE
        newLineEdit.BeginSave((s2, r2) =>
        {
          savedError = r2.Error;
          if (savedError != null)
            throw savedError;

          savedLineEdit = (LineEdit)r2.NewObject;
          isSaved = true;

          //GET (CONFIRM SAVE)
          LineEdit.GetLineEdit(savedLineEdit.Id, (s3, r3) =>
          {
            gottenError = r3.Error;
            if (gottenError != null)
              throw gottenError;

            gottenLineEdit = r3.Object;
            isGotten = true;

            //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
            gottenLineEdit.Delete();
            gottenLineEdit.BeginSave((s4, r4) =>
            {
              deletedError = r4.Error;
              if (deletedError != null)
                throw deletedError;

              deletedLineEdit = (LineEdit)r4.NewObject;

              LineEdit.GetLineEdit(deletedLineEdit.Id, (s5, r5) =>
              {
                var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
                var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
                deleteConfirmedError = r5.Error;
                if (deleteConfirmedError != null)
                {
                  isDeleteConfirmed = true;
                  throw new ExpectedException(deleteConfirmedError);
                }
                deleteConfirmedLineEdit = r5.Object;
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

                      () => { Assert.IsNotNull(newLineEdit); },
                      () => { Assert.IsNotNull(savedLineEdit); },
                      () => { Assert.IsNotNull(gottenLineEdit); },
                      () => { Assert.IsNotNull(deletedLineEdit); },
                      () => { Assert.IsNull(deleteConfirmedLineEdit); });

      EnqueueTestComplete();
    }
  }
}