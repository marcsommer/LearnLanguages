﻿using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Study.ViewModels
{
  [Export(typeof(StudyQuestionAnswerViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyQuestionAnswerViewModel : ViewModelBase
  {
    #region Ctors and Init

    public StudyQuestionAnswerViewModel()
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

    #endregion

    #region Methods

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

    #endregion
  }
}