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
  [Tag("phrase")]
  public class PhraseTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    
    [ClassInitialize]
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
        var serverEnglishLang = (from language in allLanguages
                                 where language.Text == SeedData.Instance.EnglishText
                                 select language).First();

        SeedData.Instance.EnglishLanguageDto.Id = serverEnglishLang.Id;

        var serverSpanishLang = (from language in allLanguages
                                 where language.Text == SeedData.Instance.SpanishText
                                 select language).First();

        SeedData.Instance.SpanishLanguageDto.Id = serverSpanishLang.Id;

        #endregion

        isLoaded = true;

        PhraseList.GetAll((s2, r2) =>
          {
            errorPhraseList = r2.Error;
            if (errorPhraseList != null)
              throw errorPhraseList;

            allPhrases = r2.Object;
            var serverHelloPhrase = (from phrase in allPhrases
                                     where phrase.Text == SeedData.Instance.HelloText
                                     select phrase).First();
            var serverHolaPhrase = (from phrase in allPhrases
                                     where phrase.Text == SeedData.Instance.HolaText
                                     select phrase).First();
            var serverLongPhrase = (from phrase in allPhrases
                                     where phrase.Text == SeedData.Instance.LongPhraseText
                                     select phrase).First();
            var serverDogPhrase = (from phrase in allPhrases
                                     where phrase.Text == SeedData.Instance.DogText
                                     select phrase).First();

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
                      () => { Assert.AreEqual(SeedData.Instance.EnglishId, newPhraseEdit.LanguageId); },
                      () => { Assert.IsNotNull(newPhraseEdit.Language); }
                      );
      EnqueueTestComplete();
    }

    //[TestMethod]
    //[Asynchronous]
    //public void CREATE_NEW_WITH_ID()
    //{
    //  Guid id = new Guid("BDEF87AC-21FA-4BAE-A155-91CDDA52C9CD");
    
    //  var isCreated = false;
    //  PhraseEdit PhraseEdit = null;
    //  PhraseEdit.NewPhraseEdit(id, (s,r) =>
    //    {
    //      if (r.Error != null)
    //        throw r.Error;

    //      PhraseEdit = r.Object;
    //      isCreated = true;
    //    });
    //  EnqueueConditional(() => isCreated);
    //  EnqueueCallback(() => { Assert.IsNotNull(PhraseEdit); },
    //                  () => { Assert.IsNull(null); },
    //                  () => { Assert.AreEqual(id, PhraseEdit.Id); });
    //  EnqueueTestComplete();
    //}

    [TestMethod]
    [Asynchronous]
    [Tag("pget")]
    public void GET()
    {
      Guid testId = Guid.Empty;
      var allLoaded = false;
      var isLoaded = false;
      Exception getAllError = new Exception();
      Exception error = new Exception();
      PhraseEdit PhraseEdit = null;

      PhraseList.GetAll((s1, r1) =>
        {
          getAllError = r1.Error;
          if (getAllError != null)
            throw r1.Error;
          testId = r1.Object.First().Id;
          allLoaded = true;
          PhraseEdit.GetPhraseEdit(testId, (s, r) =>
          {
            error = r.Error;
            PhraseEdit = r.Object;
            isLoaded = true;
          });
        });


      EnqueueConditional(() => isLoaded);
      EnqueueConditional(() => allLoaded);
      EnqueueCallback(() => { Assert.IsNull(error); },
                      () => { Assert.IsNull(getAllError); },
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
        newPhraseEdit.UserId = SeedData.Instance.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Instance.TestValidUsername;
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

      PhraseEdit newPhraseEdit = null;
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
        newPhraseEdit = r.Object;
        isNewed = true;

        //EDIT
        newPhraseEdit.Text = "TestPhrase NEW_EDIT_BEGINSAVE_GET_DELETE_GET";
        newPhraseEdit.UserId = SeedData.Instance.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Instance.TestValidUsername;

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

            //DELETE (MARKS DELETE.  SAVE INITIATES ACTUAL DELETE IN DB)
            gottenPhraseEdit.Delete();
            gottenPhraseEdit.BeginSave((s4, r4) =>
            {
              deletedError = r4.Error;
              if (deletedError != null)
                throw deletedError;

              deletedPhraseEdit = r4.NewObject as PhraseEdit;
              //TODO: Figure out why PhraseEditTests final callback gets thrown twice.  When server throws an exception (expected because we are trying to fetch a deleted phrase that shouldn't exist), the callback is executed twice.
              PhraseEdit.GetPhraseEdit(deletedPhraseEdit.Id, (s5, r5) =>
              {
                var debugExecutionLocation = Csla.ApplicationContext.ExecutionLocation;
                var debugLogicalExecutionLocation = Csla.ApplicationContext.LogicalExecutionLocation;
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

                      () => { Assert.IsNotNull(newPhraseEdit); },
                      () => { Assert.IsNotNull(savedPhraseEdit); },
                      () => { Assert.IsNotNull(gottenPhraseEdit); },
                      () => { Assert.IsNotNull(deletedPhraseEdit); },
                      () => { Assert.IsNull(deleteConfirmedPhraseEdit); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    [Tag("plist")]
    public void GET_ALL()
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

    [TestMethod]
    [Asynchronous]
    [Tag("preallylong")]
    public void MAKE_PHRASE_WITH_REALLY_LONG_VARIED_TEXT()
    {
      #region var reallyLongText
      var reallyLongText = @"asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`asd;flkjghweqrey8iuweqpoiuqtw[eoizsdfslknjvzxcknvl;/,asdmf;lkasdfj asldkf a/sldkf jwoieuofvahsdkj;fa;weio:KJSLDJIOWEJLKSJDFLWIUQOWUQ}R|)(_)(*+_)(*&^%$#@!~~!@#$%^&*()_+|}{POIUYTREWQASDFGHJKL:			ZXCVBNM<>? "" wijwfekjsdkfj sdfj zxcvbnm,./';lljhgfdsqwertyuiop[]\=-0987654321`";
      #endregion
      
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
        newPhraseEdit.Text = reallyLongText;
        newPhraseEdit.UserId = SeedData.Instance.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Instance.TestValidUsername;
        Assert.AreEqual(SeedData.Instance.EnglishId, newPhraseEdit.LanguageId);
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
    [Tag("preallyreallylong")]
    public void MAKE_PHRASE_WITH_REALLY_REALLY_LONG_NUMERICAL_TEXT()
    {
      var maxLength = 300000;//worked
      maxLength = 100000;//worked
      maxLength = 30000; //just for a big number, to lessen time to do test.
      var i = 0;
      string reallyReallyLongText = "";
      while (reallyReallyLongText.Length < maxLength)
      {
        reallyReallyLongText += i.ToString();
        i++;
        if (i == maxLength) //shouldn't happen, but no big deal if it does. It's just a reallyraeallyreallyreally long text.
          break;
      }
      if (reallyReallyLongText.Length > maxLength)
        reallyReallyLongText = reallyReallyLongText.Substring(0, maxLength);

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
        newPhraseEdit.Text = reallyReallyLongText;
        newPhraseEdit.UserId = SeedData.Instance.GetTestValidUserDto().Id;
        newPhraseEdit.Username = SeedData.Instance.TestValidUsername;

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
                      () => { Assert.AreEqual(reallyReallyLongText.Length, gottenPhraseEdit.Text.Length); },
                      () => { Assert.AreEqual(savedPhraseEdit.Text, gottenPhraseEdit.Text); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    [Tag("plist")]
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
    [Tag("plist")]
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

  }
}