using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.History;
using LearnLanguages.History.Events;
using LearnLanguages.Common;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(StudyPhraseManualQuestionAnswerViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyPhraseManualQuestionAnswerViewModel : StudyItemViewModelBase
  {
    #region Ctors and Init

    public StudyPhraseManualQuestionAnswerViewModel()
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
    public string ShowAnswerButtonLabel { get { return StudyResources.ButtonLabelShowAnswer; } }

    #endregion

    #region Methods

    public void Initialize(PhraseEdit question, PhraseEdit answer, ExceptionCheckCallback callback)
    {
      Question = question;
      _InitialQuestionText = question.Text;
      _ModifiedQuestionText = "";
      _InitialAnswerText = answer.Text;
      _ModifiedAnswerText = "";
      Answer = answer;
      HideAnswer();
      callback(null);

      //if (!question.IsValid)
      //  callback(new ArgumentException("question is not a valid phrase", "question"));
      //if (!answer.IsValid)
      //  callback(new ArgumentException("answer is not a valid phrase", "answer"));


      ////THE POINT OF THIS METHOD IS TO ASSIGN QUESTION, ANSWER, AND CALCULATE TIMING
      ////WE GET THE ACTUAL PHRASEEDITS FROM THE DB
      ////get these anew from database, so we know that we are NOT dealing
      ////with any children, to make things smoother when we are editing/saving them.


      //var criteria = new Business.Criteria.ListOfPhrasesCriteria(question, answer);
      //Business.PhrasesByTextAndLanguageRetriever.CreateNew(criteria, (s, r) =>
      //{
      //  if (r.Error != null)
      //  {
      //    callback(r.Error);
      //    return;
      //  }

      //  var retriever = r.Object;

      //  Question = retriever.RetrievedPhrases[question.Id];
      //  Answer = retriever.RetrievedPhrases[answer.Id];

      //  if (Question == null)
      //    callback(new ArgumentException("phrase not found in DB", "question"));
      //  if (Answer == null)
      //    callback(new ArgumentException("phrase not found in DB", "answer"));

      //  //FINISH UP
      //  HideAnswer();
      //  callback(null);
      //});
    }

    public override void Show(ExceptionCheckCallback callback)
    {
      base.Show(callback);
      _DateTimeQuestionShown = DateTime.Now;
      var viewingEvent = new History.Events.ViewingPhraseOnScreenEvent(Question);
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
      UpdateEditAnswerButtonVisibilities();
      HidingAnswer = false;

      _DateTimeAnswerShown = DateTime.Now;
      var duration = _DateTimeAnswerShown - _DateTimeQuestionShown;
      HistoryPublisher.Ton.PublishEvent(new ViewedPhraseOnScreenEvent(Question, duration));
      HistoryPublisher.Ton.PublishEvent(new ViewingPhraseOnScreenEvent(Answer));
      HistoryPublisher.Ton.PublishEvent(new ViewedPhraseOnScreenEvent(Answer, duration));

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
      return Guid.Parse(StudyResources.ReviewMethodIdStudyPhraseManualQA);
    }

    #region Edit/Save Question Stuff

    #region Question

    private Visibility _EditQuestionButtonVisibility = Visibility.Visible;
    public Visibility EditQuestionButtonVisibility
    {
      get { return _EditQuestionButtonVisibility; }
      set
      {
        if (value != _EditQuestionButtonVisibility)
        {
          _EditQuestionButtonVisibility = value;
          NotifyOfPropertyChange(() => EditQuestionButtonVisibility);
        }
      }
    }

    private Visibility _SaveQuestionButtonVisibility = Visibility.Collapsed;
    public Visibility SaveQuestionButtonVisibility
    {
      get { return _SaveQuestionButtonVisibility; }
      set
      {
        if (value != _SaveQuestionButtonVisibility)
        {
          _SaveQuestionButtonVisibility = value;
          NotifyOfPropertyChange(() => SaveQuestionButtonVisibility);
        }
      }
    }

    public string ButtonLabelEditQuestion { get { return StudyResources.ButtonLabelEdit; } }
    public string ButtonLabelSaveQuestion { get { return StudyResources.ButtonLabelSave; } }

    private bool _QuestionIsReadOnly = true;
    public bool QuestionIsReadOnly
    {
      get { return _QuestionIsReadOnly; }
      set
      {
        if (value != _QuestionIsReadOnly)
        {
          _QuestionIsReadOnly = value;
          NotifyOfPropertyChange(() => QuestionIsReadOnly);
        }
      }
    }

    public bool CanEditQuestion
    {
      get
      {
        //if the question is read only, then we are NOT currently editing the question.
        //therefore, we can press the edit question button which is what CanEditQuestion is asking.
        return QuestionIsReadOnly;
      }
    }
    public void EditQuestion()
    {
      QuestionIsReadOnly = false;
      //hack: UpdateEditButtonVisibilities method instead of visibility converter
      UpdateEditQuestionButtonVisibilities();
    }

    public bool CanSaveQuestion
    {
      get
      {
        //return Question != null && Question.IsSavable && SaveQuestionButtonIsEnabled;
        return Question != null && Question.IsValid && SaveQuestionButtonIsEnabled;
      }
    }
    public void SaveQuestion()
    {
      QuestionIsReadOnly = true;
      SaveQuestionButtonIsEnabled = false;

      if (Question.IsChild)
      {
        //QUESTION IS CHILD, AND I'M NOT ABLE TO GET THE PARENT SAVE TO WORK, SO
        //I'M GOING TO GET THE QUESTION FROM THE DB DIRECTLY, SAVE IT WITH THE NEW TEXT,
        //AND REPLACE THE QUESTION WITH THE SAVED DB NON-CHILD OBJECT.

        //I'M GOING TO HACK THIS UP NOW CUZ I NEED TO GET MOVING.
        //MAKING TWO FIELDS FOR THIS VM: INITIAL AND MODIFIED QUESTION TEXT.
        //GOING TO USE THESE TO MANIPULATE SHIT HOW I WANT IT

        //The Question PhraseEdit has the modified text, but we need to search
        //via the initial text so we're going to store the modified text,
        //re-apply the initial text, do the search, pull the old DB object,
        //modify THAT PhraseEdit with the stored modified text, save it,
        //then store the newly saved (with new text) PhraseEdit as our Question.

        _ModifiedQuestionText = Question.Text;
        Question.Text = _InitialQuestionText;
        var criteria = new Business.Criteria.ListOfPhrasesCriteria(Question);
        PhrasesByTextAndLanguageRetriever.CreateNew(criteria, (s, r) =>
        //PhraseEdit.GetPhraseEdit(Question.Id, (s, r) =>
          {
            if (r.Error != null)
              throw r.Error;
            //var retrievedQuestion = r.Object;

            var retrievedQuestion = r.Object.RetrievedPhrases[Question.Id];
            if (retrievedQuestion == null)
              throw new Exception("retrieved question is null");

            retrievedQuestion.Text = _ModifiedQuestionText;

            //RETRIEVED QUESTION IS NOW EDITED, NEED TO SAVE IT
            retrievedQuestion.BeginSave((s2, r2) =>
              {
                if (r2.Error != null)
                  throw r2.Error;
                Question = (PhraseEdit)r2.NewObject;

                //debug
                //confirm save
                var crit = new Business.Criteria.ListOfPhrasesCriteria(Question);
                PhrasesByTextAndLanguageRetriever.CreateNew(crit, (ss, rr) =>
                  {
                    if (rr.Error != null)
                      throw rr.Error;

                  });
                //end debug

                //FINISH UP
                SaveQuestionButtonIsEnabled = true;
                QuestionIsReadOnly = true;
                UpdateEditQuestionButtonVisibilities();
                _InitialQuestionText = _ModifiedQuestionText;
                _ModifiedQuestionText = "";
              });
          });
      }
      else
      {
        Question.BeginSave((s, r) =>
          {
            if (r.Error != null)
              throw r.Error;

            Question = (PhraseEdit)r.NewObject;
            SaveQuestionButtonIsEnabled = true;
            QuestionIsReadOnly = true;
            UpdateEditQuestionButtonVisibilities();
          });
      }
    }
    
    private bool _SaveQuestionButtonIsEnabled = true;
    public bool SaveQuestionButtonIsEnabled
    {
      get { return _SaveQuestionButtonIsEnabled; }
      set
      {
        if (value != _SaveQuestionButtonIsEnabled)
        {
          _SaveQuestionButtonIsEnabled = value;
          NotifyOfPropertyChange(() => SaveQuestionButtonIsEnabled);
          NotifyOfPropertyChange(() => CanSaveQuestion);
        }
      }
    }

    private void UpdateEditQuestionButtonVisibilities()
    {
      if (QuestionIsReadOnly)
      {
        EditQuestionButtonVisibility = Visibility.Visible;
        SaveQuestionButtonVisibility = Visibility.Collapsed;
      }
      else
      {
        EditQuestionButtonVisibility = Visibility.Collapsed;
        SaveQuestionButtonVisibility = Visibility.Visible;
      }
    }

    private string _InitialQuestionText;
    private string _ModifiedQuestionText;

    #endregion

    #region Answer

    private Visibility _EditAnswerButtonVisibility = Visibility.Collapsed;
    public Visibility EditAnswerButtonVisibility
    {
      get { return _EditAnswerButtonVisibility; }
      set
      {
        if (value != _EditAnswerButtonVisibility)
        {
          _EditAnswerButtonVisibility = value;
          NotifyOfPropertyChange(() => EditAnswerButtonVisibility);
        }
      }
    }

    private Visibility _SaveAnswerButtonVisibility = Visibility.Collapsed;
    public Visibility SaveAnswerButtonVisibility
    {
      get { return _SaveAnswerButtonVisibility; }
      set
      {
        if (value != _SaveAnswerButtonVisibility)
        {
          _SaveAnswerButtonVisibility = value;
          NotifyOfPropertyChange(() => SaveAnswerButtonVisibility);
        }
      }
    }

    public string ButtonLabelEditAnswer { get { return StudyResources.ButtonLabelEdit; } }
    public string ButtonLabelSaveAnswer { get { return StudyResources.ButtonLabelSave; } }

    private bool _AnswerIsReadOnly = true;
    public bool AnswerIsReadOnly
    {
      get { return _AnswerIsReadOnly; }
      set
      {
        if (value != _AnswerIsReadOnly)
        {
          _AnswerIsReadOnly = value;
          NotifyOfPropertyChange(() => AnswerIsReadOnly);
        }
      }
    }

    public bool CanEditAnswer
    {
      get
      {
        //if the question is read only, then we are NOT currently editing the question.
        //therefore, we can press the edit question button which is what CanEditAnswer is asking.
        return AnswerIsReadOnly;
      }
    }
    public void EditAnswer()
    {
      AnswerIsReadOnly = false;
      //hack: UpdateEditButtonVisibilities method instead of visibility converter
      UpdateEditAnswerButtonVisibilities();
    }

    public bool CanSaveAnswer
    {
      get
      {
        //return Answer != null && Answer.IsSavable && SaveAnswerButtonIsEnabled;
        return Answer != null && Answer.IsValid && SaveAnswerButtonIsEnabled;
      }
    }
    public void SaveAnswer()
    {
      AnswerIsReadOnly = true;
      SaveAnswerButtonIsEnabled = false;

      if (Answer.IsChild)
      {
        //QUESTION IS CHILD, AND I'M NOT ABLE TO GET THE PARENT SAVE TO WORK, SO
        //I'M GOING TO GET THE QUESTION FROM THE DB DIRECTLY, SAVE IT WITH THE NEW TEXT,
        //AND REPLACE THE QUESTION WITH THE SAVED DB NON-CHILD OBJECT.

        //I'M GOING TO HACK THIS UP NOW CUZ I NEED TO GET MOVING.
        //MAKING TWO FIELDS FOR THIS VM: INITIAL AND MODIFIED QUESTION TEXT.
        //GOING TO USE THESE TO MANIPULATE SHIT HOW I WANT IT

        //The Answer PhraseEdit has the modified text, but we need to search
        //via the initial text so we're going to store the modified text,
        //re-apply the initial text, do the search, pull the old DB object,
        //modify THAT PhraseEdit with the stored modified text, save it,
        //then store the newly saved (with new text) PhraseEdit as our Answer.

        _ModifiedAnswerText = Answer.Text;
        Answer.Text = _InitialAnswerText;
        var criteria = new Business.Criteria.ListOfPhrasesCriteria(Answer);
        PhrasesByTextAndLanguageRetriever.CreateNew(criteria, (s, r) =>
        //PhraseEdit.GetPhraseEdit(Answer.Id, (s, r) =>
          {
            if (r.Error != null)
              throw r.Error;
            //var retrievedAnswer = r.Object;

            var retrievedAnswer = r.Object.RetrievedPhrases[Answer.Id];
            if (retrievedAnswer == null)
              throw new Exception("retrieved question is null");

            retrievedAnswer.Text = _ModifiedAnswerText;

            //RETRIEVED QUESTION IS NOW EDITED, NEED TO SAVE IT
            retrievedAnswer.BeginSave((s2, r2) =>
              {
                if (r2.Error != null)
                  throw r2.Error;
                Answer = (PhraseEdit)r2.NewObject;

                //debug
                //confirm save
                var crit = new Business.Criteria.ListOfPhrasesCriteria(Answer);
                PhrasesByTextAndLanguageRetriever.CreateNew(crit, (ss, rr) =>
                  {
                    if (rr.Error != null)
                      throw rr.Error;

                  });
                //end debug

                //FINISH UP
                SaveAnswerButtonIsEnabled = true;
                AnswerIsReadOnly = true;
                UpdateEditAnswerButtonVisibilities();
                _InitialAnswerText = _ModifiedAnswerText;
                _ModifiedAnswerText = "";
              });
          });
      }
      else
      {
        Answer.BeginSave((s, r) =>
          {
            if (r.Error != null)
              throw r.Error;

            Answer = (PhraseEdit)r.NewObject;
            SaveAnswerButtonIsEnabled = true;
            AnswerIsReadOnly = true;
            UpdateEditAnswerButtonVisibilities();
          });
      }
    }
    
    private bool _SaveAnswerButtonIsEnabled = true;
    public bool SaveAnswerButtonIsEnabled
    {
      get { return _SaveAnswerButtonIsEnabled; }
      set
      {
        if (value != _SaveAnswerButtonIsEnabled)
        {
          _SaveAnswerButtonIsEnabled = value;
          NotifyOfPropertyChange(() => SaveAnswerButtonIsEnabled);
          NotifyOfPropertyChange(() => CanSaveAnswer);
        }
      }
    }

    private void UpdateEditAnswerButtonVisibilities()
    {
      //IF THE ANSWER ISN'T SHOWN YET, WE DON'T WANT TO SEE EDIT/SAVE BUTTONS EITHER
      if (AnswerVisibility == Visibility.Collapsed)
      {
        EditAnswerButtonVisibility = Visibility.Collapsed;
        SaveAnswerButtonVisibility = Visibility.Collapsed;
      }
      //IF THE ANSWER BUTTON IS SHOWN AND ANSWER IS READONLY, THEN WE CAN EDIT BUT NOT SAVE
      else if (AnswerIsReadOnly)
      {
        EditAnswerButtonVisibility = Visibility.Visible;
        SaveAnswerButtonVisibility = Visibility.Collapsed;
      }
      //OTHERWISE, WE CAN SAVE BUT NOT EDIT
      else
      {
        EditAnswerButtonVisibility = Visibility.Collapsed;
        SaveAnswerButtonVisibility = Visibility.Visible;
      }
    }

    private string _InitialAnswerText;
    private string _ModifiedAnswerText;

    #endregion

    #endregion
  }
}
