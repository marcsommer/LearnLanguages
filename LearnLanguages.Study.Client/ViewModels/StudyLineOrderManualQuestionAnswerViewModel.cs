using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.History;
using LearnLanguages.History.Events;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(StudyLineOrderManualQuestionAnswerViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyLineOrderManualQuestionAnswerViewModel : StudyItemViewModelBase
  {
    #region Ctors and Init

    public StudyLineOrderManualQuestionAnswerViewModel()
    {
      //Services.EventAggregator.Subscribe(this);
    }

    #endregion

    #region Properties

    private LineEdit _Question;
    public LineEdit Question
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

    private LineEdit _Answer;
    public LineEdit Answer
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
          //NotifyOfPropertyChange(() => CanNext);
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
          NotifyOfPropertyChange(() => ShowAnswerButtonVisibility);
        }
      }
    }

    public Visibility ShowAnswerButtonVisibility
    {
      get
      {
        if (AnswerVisibility == Visibility.Collapsed)
          return Visibility.Visible;
        else
          return Visibility.Collapsed;
      }
    }

    //private Visibility _NextButtonVisibility = Visibility.Collapsed;
    //public Visibility NextButtonVisibility
    //{
    //  get { return _NextButtonVisibility; }
    //  set
    //  {
    //    if (value != _NextButtonVisibility)
    //    {
    //      _NextButtonVisibility = value;
    //      NotifyOfPropertyChange(() => NextButtonVisibility);
    //    }
    //  }
    //}

    public string QuestionHeader
    {
      get
      {
        if (Question == null)
          return StudyResources.ErrorMsgQuestionIsNull;

        return string.Format(StudyResources.StudyLineOrderQuestionHeader, Question.LineNumber);
      }
    }
    public string AnswerHeader
    {
      get
      {
        if (Answer == null)
          return StudyResources.ErrorMsgAnswerIsNull;

        return StudyResources.StudyLineOrderAnswerHeader;
      }
    }

    public string ShowAnswerButtonLabel { get { return StudyResources.ButtonLabelShowAnswer; } }

    //private ExceptionCheckCallback _CompletedCallback { get; set; }


    #endregion

    #region Methods

    public void Initialize(LineEdit question, LineEdit answer)
    {
      Question = question;
      Answer = answer;
      HideAnswer();
    }

    public override void Show(ExceptionCheckCallback callback)
    {
      base.Show(callback);
      _DateTimeQuestionShown = DateTime.Now;
      var viewingEvent = new History.Events.ViewingPhraseOnScreenEvent(Question.Phrase);
      HistoryPublisher.Ton.PublishEvent(viewingEvent);
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
      HistoryPublisher.Ton.PublishEvent(new ViewedPhraseOnScreenEvent(Question.Phrase, duration));
      HistoryPublisher.Ton.PublishEvent(new ViewingPhraseOnScreenEvent(Answer.Phrase));
      HistoryPublisher.Ton.PublishEvent(new ViewedPhraseOnScreenEvent(Answer.Phrase, duration));

      _Callback(null);
    }

    //public bool CanNext
    //{
    //  get
    //  {
    //    return (_CompletedCallback != null && !HidingAnswer);
    //  }
    //}
    //public void Next()
    //{
    //  _CompletedCallback(null);
    //}

    #endregion

    public override void Abort()
    {
      //ShowAnswer();
      QuestionVisibility = Visibility.Collapsed;
      AnswerVisibility = Visibility.Collapsed;
      if (_Callback != null)
        _Callback(null);
    }

    protected override Guid GetReviewMethodId()
    {
      return Guid.Parse(StudyResources.ReviewMethodIdStudyLineOrderManualQA);
    }
  }
}
