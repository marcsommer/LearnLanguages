using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using LearnLanguages.Common;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.History;
using LearnLanguages.History.Events;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(StudyPhraseTimedQuestionAnswerViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyPhraseTimedQuestionAnswerViewModel : StudyItemViewModelBase
  {
    #region Ctors and Init

    public StudyPhraseTimedQuestionAnswerViewModel()
    {
      Services.EventAggregator.Subscribe(this);
    }

    #endregion

    #region Properties

    private PhraseEdit _Question;
    public PhraseEdit Question
    {
      get { return _Question; }
      set
      {
        if (value != _Question)
        {
          _Question = value;
          NotifyOfPropertyChange(() => Question);
          NotifyOfPropertyChange(() => QuestionHeader);
        }
      }
    }

    private PhraseEdit _Answer;
    public PhraseEdit Answer
    {
      get { return _Answer; }
      set
      {
        if (value != _Answer)
        {
          _Answer = value;
          NotifyOfPropertyChange(() => Answer);
          NotifyOfPropertyChange(() => AnswerHeader);
        }
      }
    }

    private bool _HidingAnswer;
    public bool HidingAnswer
    {
      get { return _HidingAnswer; }
      set
      {
        if (value != _HidingAnswer)
        {
          _HidingAnswer = value;
          NotifyOfPropertyChange(() => HidingAnswer);
        }
      }
    }

    private Visibility _QuestionVisibility = Visibility.Visible;
    public Visibility QuestionVisibility
    {
      get { return _QuestionVisibility; }
      set
      {
        if (value != _QuestionVisibility)
        {
          _QuestionVisibility = value;
          NotifyOfPropertyChange(() => QuestionVisibility);
        }
      }
    }

    private Visibility _AnswerVisibility = Visibility.Collapsed;
    public Visibility AnswerVisibility
    {
      get { return _AnswerVisibility; }
      set
      {
        if (value != _AnswerVisibility)
        {
          _AnswerVisibility = value;
          NotifyOfPropertyChange(() => AnswerVisibility);
        }
      }
    }

    public string QuestionHeader
    {
      get
      {
        if (Question == null)
          return StudyResources.ErrorMsgQuestionIsNull;

        if (Question.Language == null)
          return StudyResources.ErrorMsgLanguageIsNull;

        if (string.IsNullOrEmpty(Question.Language.Text))
          return StudyResources.ErrorMsgLanguageTextIsNullOrEmpty;

        return Question.Language.Text;
      }
    }
    public string AnswerHeader
    {
      get
      {
        if (Answer == null)
          return StudyResources.ErrorMsgAnswerIsNull;

        if (Answer.Language == null)
          return StudyResources.ErrorMsgLanguageIsNull;

        if (string.IsNullOrEmpty(Answer.Language.Text))
          return StudyResources.ErrorMsgLanguageTextIsNullOrEmpty;

        return Answer.Language.Text;
      }
    }

    private int _QuestionDurationInMilliseconds;
    public int QuestionDurationInMilliseconds
    {
      get { return _QuestionDurationInMilliseconds; }
      set
      {
        if (value != _QuestionDurationInMilliseconds)
        {
          _QuestionDurationInMilliseconds = value;
          NotifyOfPropertyChange(() => QuestionDurationInMilliseconds);
        }
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Executes callback when answer is shown, or when exception is thrown.
    /// </summary>
    /// <param name="question">Question PhraseEdit</param>
    /// <param name="answer">Answer PhraseEdit</param>
    /// <param name="questionDurationInMilliseconds"></param>
    /// <param name="callback"></param>
    protected void AskQuestion(PhraseEdit question,
                            PhraseEdit answer,
                            int questionDurationInMilliseconds,
                            ExceptionCheckCallback callback)
    {
      try
      {

        //BingTranslatorService.LanguageServiceClient client = new BingTranslatorService.LanguageServiceClient();
        //client.SpeakCompleted += client_SpeakCompleted;
        //client.SpeakAsync(StudyResources.BingAppId, question.Text, BingTranslateHelper.GetLanguageCode(question.Language.Text), string.Empty, string.Empty);

        HideAnswer();
        Question = question;
        Answer = answer;
        //QuestionDurationInMilliseconds = questionDurationInMilliseconds;
        BackgroundWorker timer = new BackgroundWorker();
        timer.DoWork += (s, e) =>
        {
          try
          {

            System.Threading.Thread.Sleep(questionDurationInMilliseconds);
            //if (AnswerVisibility == Visibility.Collapsed)
              
            ShowAnswer();
            callback(null);
          }
          catch (Exception ex)
          {
            callback(ex);
          }
        };

        timer.RunWorkerAsync();
      }
      catch (Exception ex)
      {
        callback(ex);
      }
    }

    public void Initialize(PhraseEdit question, PhraseEdit answer, ExceptionCheckCallback callback)
    {
      //get these anew from database, so we know that we are NOT dealing
      //with any children.
      
      #region 1) Determine which phrase is "foreign"
      bool questionIsForeign = false;
      PhraseEdit foreignPhrase = null;
      PhraseEdit nativePhrase = null;
      var criteria = new Business.Criteria.PhraseCriteria(question);
      Business.StudyDataRetriever.CreateNew((s, r) =>
        {
          if (r.Error != null)
            callback(r.Error);

          var nativeLanguageText = r.Object.StudyData.NativeLanguageText;
          if (question.Language.Text == nativeLanguageText)
          {
            foreignPhrase = answer;
            nativePhrase = question;
            questionIsForeign = false;
          }
          else
          {
            foreignPhrase = question;
            nativePhrase = answer;
            questionIsForeign = true;
          }
        
        #region Now find the translation related to the question and answer
          
      var criteria = 
        new Business.Criteria.TranslationSearchCriteria(message.SourcePhrase, 
                                                        message.TranslatedPhrase.Language.Text);
      Business.TranslationSearchRetriever.CreateNew(criteria, (s, r) =>
        {
          if (r.Error != null)
          {
            throw r.Error;
          }

          if (r.Object.Translation != null)
          {
            //we already have a translation for this, so we don't need to do anything further.
            return;
          }
          

          //NO PRIOR TRANSLATION EXISTS, SO CREATE AND SAVE.

          //CREATE
          var createTranslationCriteria = 
            new Business.Criteria.ListOfPhrasesCriteria(message.SourcePhrase, message.TranslatedPhrase);
          TranslationCreator.CreateNew(createTranslationCriteria, (s2, r2) =>
            {
              if (r2.Error != null)
              {
                throw r2.Error;
              }

              //SAVE
              r2.Object.Translation.BeginSave((s3, r3) =>
                {
                  if (r3.Error != null)
                    throw r3.Error;
                });
            });
        });
        #endregion
        });
      #endregion
      //QUESTION
      PhraseEdit.GetPhraseEdit(, (s, r) =>
      {
        if (r.Error != null)
          callback(r.Error);
        Question = r.Object;

        //ANSWER
        PhraseEdit.GetPhraseEdit(answer.Id, (s2, r2) =>
        {
          if (r2.Error != null)
            callback(r2.Error);

          Answer = r2.Object;

          //FINISH UP
          var words = Question.Text.ParseIntoWords();
          var durationMilliseconds = words.Count * (int.Parse(StudyResources.DefaultMillisecondsTimePerWordInQuestion));
          QuestionDurationInMilliseconds = durationMilliseconds;
          HideAnswer();
          callback(null);
        });
      });

      //Question = question;
      //Answer = answer;
      //var words = question.Text.ParseIntoWords();
      //var durationMilliseconds = words.Count * (int.Parse(StudyResources.DefaultMillisecondsTimePerWordInQuestion));
      //QuestionDurationInMilliseconds = durationMilliseconds;
      //HideAnswer();
    }

    public override void Show(ExceptionCheckCallback callback)
    {
      _DateTimeQuestionShown = DateTime.Now;
      ViewModelVisibility = Visibility.Visible;
      DispatchShown();
      var eventViewing = new History.Events.ViewingPhraseOnScreenEvent(Question);
      History.HistoryPublisher.Ton.PublishEvent(eventViewing);
      AskQuestion(Question, Answer, QuestionDurationInMilliseconds, (e) =>
        {
          if (e != null)
          {
            callback(e);
          }
          else
          {
            //WAIT FOR ALOTTED TIME FOR USER TO THINK ABOUT ANSWER.
            System.Threading.Thread.Sleep(int.Parse(StudyResources.DefaultThinkAboutAnswerTime));
            callback(null);
          }
        });
    }

    public override void Abort()
    {
      QuestionVisibility = Visibility.Collapsed;
      AnswerVisibility = Visibility.Collapsed;
      if (_Callback != null)
        _Callback(null);
    }
    
    private void HideAnswer()
    {
      HidingAnswer = true;
      AnswerVisibility = Visibility.Collapsed;
    }

    public bool CanShowAnswer
    {
      get
      {
        return (Answer != null);
      }
    }
    public void ShowAnswer()
    {
      AnswerVisibility = Visibility.Visible;
      HidingAnswer = false;

      _DateTimeAnswerShown = DateTime.Now;
      var duration = _DateTimeAnswerShown - _DateTimeQuestionShown;
      HistoryPublisher.Ton.PublishEvent(new ViewedPhraseOnScreenEvent(Question, duration));
      HistoryPublisher.Ton.PublishEvent(new ViewingPhraseOnScreenEvent(Answer));
      HistoryPublisher.Ton.PublishEvent(new ViewedPhraseOnScreenEvent(Answer, duration));
    }

    protected override Guid GetReviewMethodId()
    {
      return Guid.Parse(StudyResources.ReviewMethodIdStudyPhraseTimedQA);
    }

    #endregion
  }
}
