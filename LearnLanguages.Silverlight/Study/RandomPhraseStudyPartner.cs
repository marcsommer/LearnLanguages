using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Caliburn.Micro;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Silverlight.Interfaces;

namespace LearnLanguages.Silverlight
{
  /// <summary>
  /// The RandomPhraseStudyPartner is a simple study partner that doesn't ask the user anything.
  /// It "simply" picks a phrase at random and studies it.  It has no memory.
  /// This is my most basic partner, as I am just forming how they work.  This isn't really a practical study
  /// partner.
  /// </summary>
  [Export(typeof(IStudyPartner))]
  public class RandomPhraseStudyPartner : StudyPartnerBase,
                                          IHandle<Navigation.EventMessages.NavigatedEventMessage>
  {
    #region Ctors and Init

    public RandomPhraseStudyPartner()
    {

    }

    #endregion

    #region Fields

    #endregion

    #region Properties

    private PhraseList _Phrases { get; set; }
    private bool _ShowingQuestion { get; set; }
    private Guid _NavigationId { get; set; }
    private PhraseEdit _Question { get; set; }
    private PhraseEdit _Answer { get; set; }

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

    #endregion

    #region Methods

    protected override void StudyImpl()
    {
      GetRandomQuestionAnswer((s, r) =>
      {
        if (r.Error != null)
        {
          if (r.Error is System.ServiceModel.CommunicationException)
          {
            Services.Log(AppResources.ErrorMsgTranslateServiceNotAvailable, LogPriority.High, LogCategory.Exception);
            Study(EventAggregator);
            return;
          }
          else
            throw r.Error;
        }
        if (r.Question == null || r.Answer == null)
          throw new ArgumentNullException("question or answer null");

        _Question = r.Question;
        _Answer = r.Answer;
        Services.EventAggregator.Subscribe(this);
        _NavigationId = Guid.NewGuid();
        Navigation.Publish.NavigationRequest<ViewModels.QuestionAnswerViewModel>(_NavigationId, AppResources.BaseAddress);
      });
    }

    protected void GetRandomQuestionAnswer(Delegates.QuestionAnswerCallback callback)
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
        {
          //callback(null, null, r.Error);
          callback(this, new Args.QuestionAnswerArgs(r.Error));
          return;
        }
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
          {
            callback(this, new Args.QuestionAnswerArgs(r2.Error));
            //callback(null, null, r2.Error);
            return;
          }
          //throw r2.Error;

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
            {
              var ex = new Exception("translation located, but all texts are equal");
              callback(this, new Args.QuestionAnswerArgs(ex));
              //callback(null, null, ex);
              return;
            }
            //WE HAVE BOTH QUESTION AND ANSWER SO INITIATE CALLBACK
            callback(this, new Args.QuestionAnswerArgs(question, answer));
            //callback(question, answer, null);
            return;
          }
          #endregion

          #region 3b. NO EXISTING TRANSLATION, SO AUTO TRANSLATE WITH BING AND CREATE NEW ANSWER PHRASE
          else
          {
            AutoTranslateText(callback, question, answer);
          }
          #endregion

        });

        #endregion
      });

      #endregion

      #endregion
    }

    private PhraseEdit AutoTranslateText(Delegates.QuestionAnswerCallback callback, PhraseEdit question, PhraseEdit answer)
    {
      //USING THE BING/MS TRANSLATE API
      var questionLanguageText = question.Language.Text;
      var questionLanguageCode = BingTranslateHelper.GetLanguageCode(questionLanguageText);
      //if question is in English, then we translate into spanish.  
      //if question is in foreign language, we translate into English
      //this is just hard-coded for this study partner
      string answerLanguageText = "";

      if (questionLanguageText == "English")
        answerLanguageText = "Spanish";
      else
        answerLanguageText = "English";
      var answerLanguageCode = BingTranslateHelper.GetLanguageCode(answerLanguageText);


      //ASSUME QUESTION IS IN SPANISH, AND ANSWER IS IN ENGLISH
      //var questionLanguageText = AppResources.SpanishLanguageText;
      //var questionLanguageCode = AppResources.SpanishLanguageCode;
      //var answerLanguageText = AppResources.EnglishLanguageText;
      //var answerLanguageCode = AppResources.EnglishLanguageCode;

      ////IF QUESTION IS IN ENGLISH, THEN ANSWER IS SPANISH
      //if (question.Language.Text == AppResources.EnglishLanguageText)
      //{
      //  questionLanguageText = AppResources.EnglishLanguageText;
      //  questionLanguageCode = AppResources.EnglishLanguageCode;
      //  answerLanguageText = AppResources.SpanishLanguageText;
      //  answerLanguageCode = AppResources.SpanishLanguageCode;
      //}

      BingTranslatorService.LanguageServiceClient client = new BingTranslatorService.LanguageServiceClient();
      
      client.TranslateCompleted += (s4, r4) =>
      {
        if (r4.Error != null)
        {
          //throw r4.Error;
          //callback(null, null, r4.Error);
          callback(this, new Args.QuestionAnswerArgs(r4.Error));
          return;
        }

        var translatedText = r4.Result;

        PhraseEdit.NewPhraseEdit(answerLanguageText, (s5, r5) =>
        {
          if (r5.Error != null)
          {
            //callback(null, null, r5.Error);
            callback(this, new Args.QuestionAnswerArgs(r5.Error));
            return;
          }
          //throw r5.Error;

          #region 5. POPULATE ANSWER WITH TRANSLATED TEXT AND INVOKE CALLBACK

          answer = r5.Object;
          answer.Text = translatedText;
          Debug.Assert(answer.Language != null);
          //callback(question, answer, null);
          callback(this, new Args.QuestionAnswerArgs(question, answer));

          #endregion
        });
      };

      client.TranslateAsync(AppResources.BingAppId,
                            question.Text,
                            questionLanguageCode,
                            answerLanguageCode,
                            @"text/plain", //this as opposed to "text/html"
                            "general"); //only supported category is "general"
      //GOTO ABOVE TRANSLATECOMPLETED HANDLER

      return answer;
    }

    protected override void AskUserExtraDataImpl(object criteria, Delegates.StudyDataCallback callback)
    {
      callback(this, new Args.StudyDataArgs());
    }

    public void Handle(Navigation.EventMessages.NavigatedEventMessage message)
    {
      //check to see if question and answer view model is the target
      if (message.NavigationInfo.NavigationId == _NavigationId &&
          message.NavigationInfo.ViewModelCoreNoSpaces !=
          ViewModelBase.GetCoreViewModelName(typeof(ViewModels.QuestionAnswerViewModel))
          &&
          message.ViewModel is ViewModels.QuestionAnswerViewModel)
      {
        return;
      }

      //UNSUBSCRIBE, AS WE HAVE THE QUESTIONANSWER NAVIGATED VIEWMODEL
      Services.EventAggregator.Unsubscribe(this);

      var qaViewModel = (ViewModels.QuestionAnswerViewModel)message.ViewModel;
      qaViewModel.AskQuestion(_Question, _Answer, 5000, (ex) =>
      {
        if (ex != null)
          throw ex;
      });
    }

    #endregion
  }
}
