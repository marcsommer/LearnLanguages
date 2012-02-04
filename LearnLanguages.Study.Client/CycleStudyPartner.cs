using System;
using System.Linq;
using LearnLanguages.Study.Interfaces;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using LearnLanguages.Business;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The RandomPhraseStudyPartner is a simple study partner that doesn't ask the user anything.
  /// It simply picks a phrase at random and studies it.  It has no memory.
  /// This is my most basic partner, as I am just forming how they work.  This isn't really a practical study
  /// partner.
  /// </summary>
  [Export(typeof(IStudyPartner))]
  public class RandomPhraseStudyPartner : StudyPartnerBase
  {
    private PhraseList _Phrases;
    private bool _ShowingQuestion { get; set; }

    protected override void StudyImpl()
    {
      GetRandomQuestionAnswer((q, a) =>
        {
          
        });
    }

    protected void GetRandomQuestionAnswer(QuestionAnswerCallback callback)
    {
      #region Initialize

      _ShowingQuestion = true;
      PhraseEdit question = null;
      PhraseEdit answer = null;
      TranslationEdit qaTranslation = null;

      #region 1. CHOOSE RANDOM PHRASE

      PhraseList.GetAll((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        _Phrases = r.Object;

        Random random = new Random(DateTime.Now.Millisecond +
                                   DateTime.Now.Second +
                                   DateTime.Now.Month +
                                   (int)(Mouse.Position.X * 1000));

        var randomIndex = random.Next(0, _Phrases.Count);
        question = _Phrases[randomIndex];

        #region 2. GET TRANSLATION FOR THAT PHRASE, IF WE DON'T HAVE ONE THEN CREATE TRANSLATION.

        TranslationList.GetAllTranslationsContainingPhraseById(question, (s2, r2) =>
        {
          if (r2.Error != null)
            throw r2.Error;

          #region 3a. FOUND EXISTING TRANSLATION.  SET QUESTION AND ANSWER, AND CALL CALLBACK
          //IF WE HAVE FOUND ONE OR MORE TRANSLATIONS, THEN PICK ONE OF THEM AT RANDOM
          var foundTranslations = r2.Object;
          if (foundTranslations.Count > 0)
          {
            randomIndex = random.Next(0, foundTranslations.Count);
            qaTranslation = foundTranslations[randomIndex];
            //PICK ONE OF THE TRANSLATION'S OTHER LANGUAGES THAN THE QUESTION
            answer = null;
            foreach (var phrase in qaTranslation.Phrases)
            {
              if (phrase.Text != question.Text)
              {
                answer = phrase;
                break;
              }
            }
            if (answer == null)
              throw new Exception("translation located, but all texts are equal");

            //WE HAVE BOTH QUESTION AND ANSWER SO INITIATE CALLBACK
            callback(question, answer);
          }
          #endregion

          #region 3b. NO EXISTING TRANSLATION, SO CREATE NEW TRANSLATION
          else
          {
            //WE DON'T HAVE A TRANSLATION FOR THIS PHRASE, SO CREATE NEW TRANSLATION
            TranslationEdit.NewTranslationEdit((s3, r3) =>
              {
                if (r3.Error != null)
                  throw r3.Error;

                qaTranslation = r3.Object;
                
                //USING THE BING/MS TRANSLATE API
                //assume question is spanish
                var questionLanguageText = StudyResources.SpanishLanguageText;
                var questionLanguageCode = StudyResources.SpanishLanguageCode;
                var answerLanguageText = StudyResources.EnglishLanguageText;
                var answerLanguageCode = StudyResources.EnglishLanguageCode;


                //if question is already in english, then we're going to create spanish
                if (question.Language.Text == StudyResources.EnglishLanguageText)
                {
                  questionLanguageText = StudyResources.EnglishLanguageText;
                  questionLanguageCode = StudyResources.EnglishLanguageCode;
                  answerLanguageText = StudyResources.SpanishLanguageText;
                  answerLanguageCode = StudyResources.SpanishLanguageCode;
                }


                BingTranslatorService.LanguageServiceClient client = new BingTranslatorService.LanguageServiceClient();

                client.TranslateCompleted += (s4, r4) =>
                {
                  if (r4.Error != null)
                    throw r4.Error;

                  var translatedText = r4.Result;
                  var addingQuestion = true;

                  qaTranslation.Phrases.AddedNew += (s5, r5) =>
                  {
                    if (addingQuestion)
                      question
                    answer = r5.NewObject;

                    //we need to assign language to newly created phrase object
                    LanguageEdit.GetLanguageEdit(answerLanguageText, (s6, r6) =>
                    {
                      if (r6.Error != null)
                        throw r6.Error;

                      answer.Language = r6.Object;
                      answer.Text = translatedText;

                      //WE NOW HAVE VALID QUESTION AND ANSWER PHRASES, SO CREATE TRANSLATION
                      TranslationEdit.NewTranslationEdit((s7, r7) =>
                      {
                        if (r7.Error != null)
                          throw r7.Error;

                        //qaTranslation 
                      });

                    });




                  };


                  qaTranslation.Phrases.AddNew();//question
                  qaTranslation.Phrases.AddNew();//answer
                  //GOTO ABOVE ADDEDNEW HANDLER
                };

                client.TranslateAsync(StudyResources.BingAppId, question.Text, questionLanguageCode, answerLanguageCode);
                //GOTO ABOVE TRANSLATEcOMPLETED HANDLER

              });
          }
          #endregion

        });

        #endregion
      });

      #endregion

      #endregion
    }
    
    protected override void AskUserExtraDataImpl()
    {
      //NOT USED IN THIS STUDY PARTNER.
    }

    private bool _IsBusy;
    public bool IsBusy
    {
      get { return _IsBusy; }
      set
      {
        if (value != _IsBusy)
        {
          _IsBusy = value;
          NotifyOfPropertyChange(() => IsBusy);
        }
      }
    }

    
  }
}
