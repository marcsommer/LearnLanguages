using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Silverlight.Interfaces;
using Caliburn.Micro;
using System.Windows;
using LearnLanguages.Business;
using System.ComponentModel;
using System;
using LearnLanguages.Delegates;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(QuestionAnswerViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class QuestionAnswerViewModel : ViewModelBase//,
                                         //IHandle<Navigation.EventMessages.NavigatedEventMessage>
  {
    public QuestionAnswerViewModel()
    {
      Services.EventAggregator.Subscribe(this);
    }

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

    //public int QuestionDurationInMilliseconds { get; set; }

    /// <summary>
    /// Executes callback when answer is shown, or when exception is thrown.
    /// </summary>
    /// <param name="question">Question PhraseEdit</param>
    /// <param name="answer">Answer PhraseEdit</param>
    /// <param name="questionDurationInMilliseconds"></param>
    /// <param name="callback"></param>
    public void AskQuestion(PhraseEdit question, 
                            PhraseEdit answer, 
                            int questionDurationInMilliseconds, 
                            ExceptionCheckCallback callback)
    {
      try
      {
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
              if (AnswerVisibility == Visibility.Collapsed)
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
    }

    //public void Handle(Navigation.EventMessages.NavigatedEventMessage message)
    //{
    //  //WE ARE LISTENING FOR A MESSAGE THAT SAYS WE WERE SUCCESSFULLY NAVIGATED TO (SHELLVIEW.MAIN == STUDYVIEWMODEL)
    //  //SO WE ONLY CARE ABOUT NAVIGATED EVENT MESSAGES ABOUT OUR CORE AS DESTINATION.
    //  if (message.NavigationInfo.ViewModelCoreNoSpaces != ViewModelBase.GetCoreViewModelName(typeof(QuestionAnswerViewModel)))
    //    return;

    //  //WE HAVE BEEN SUCCESSFULLY NAVIGATED TO.

    //  //SINCE THIS IS A NONSHARED COMPOSABLE PART, WE ONLY CARE ABOUT NAVIGATED MESSAGE ONCE, SO UNSUBSCRIBE
    //  Services.EventAggregator.Unsubscribe(this);
    //}

    
    public string QuestionHeader
    {
      get 
      {
        if (Question == null)
          return AppResources.ErrorMsgQuestionIsNull;

        if (Question.Language == null)
          return AppResources.ErrorMsgLanguageIsNull;

        if (string.IsNullOrEmpty(Question.Language.Text))
          return AppResources.ErrorMsgLanguageTextIsNullOrEmpty;

        return Question.Language.Text;
      }
    }

    public string AnswerHeader
    {
      get
      {
        if (Answer == null)
          return AppResources.ErrorMsgAnswerIsNull;

        if (Answer.Language == null)
          return AppResources.ErrorMsgLanguageIsNull;

        if (string.IsNullOrEmpty(Answer.Language.Text))
          return AppResources.ErrorMsgLanguageTextIsNullOrEmpty;

        return Answer.Language.Text;
      }
    }
  }
}
