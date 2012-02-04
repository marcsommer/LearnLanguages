using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Silverlight.Interfaces;
using Caliburn.Micro;
using System.Windows;
using LearnLanguages.Business;
using System.ComponentModel;

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
        }
      }
    }

    private bool _HidingQuestion;
    public bool HidingQuestion
    {
      get { return _HidingQuestion; }
      set
      {
        if (value != _HidingQuestion)
        {
          _HidingQuestion = value;
          NotifyOfPropertyChange(() => HidingQuestion);
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

    public void AskQuestion(PhraseEdit question, PhraseEdit answer, int questionDurationInMilliseconds)
    {
      HidingQuestion = true;
      Question = question;
      Answer = answer;
      //QuestionDurationInMilliseconds = questionDurationInMilliseconds;
      BackgroundWorker timer = new BackgroundWorker();
      timer.DoWork += (s, e) =>
        {
          System.Threading.Thread.Sleep(questionDurationInMilliseconds);
          AnswerVisibility = Visibility.Visible;
          HidingQuestion = false;
        };
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
  }
}
