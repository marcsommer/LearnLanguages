using System;
using System.ComponentModel.Composition;
using System.Windows;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common.Feedback;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common;

namespace LearnLanguages.Study.ViewModels
{
  /// <summary>
  /// Stateless viewmodel, just used for providing an interface to the user with
  /// providing feedback as to how much of something should be considered "known" 
  /// by the user.
  /// </summary>
  [Export(typeof(PercentKnownFeedbackViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class PercentKnownFeedbackViewModel : FeedbackViewModelBase
  {
    #region Ctors and Init

    public PercentKnownFeedbackViewModel()
    {
      Feedback = new Feedback<double>();
      SetFeedback(-1);
    }

    #endregion

    #region Properties

    private string _Instructions = StudyResources.InstructionsSelectFeedbackNoneSomeMostAll;
    public string Instructions
    {
      get { return _Instructions; }
      set
      {
        if (value != _Instructions)
        {
          _Instructions = value;
          NotifyOfPropertyChange(() => Instructions);
        }
      }
    }

    private string _NoneInstructions = StudyResources.InstructionsSelectFeedbackNone;
    public string NoneInstructions
    {
      get { return _NoneInstructions; }
      set
      {
        if (value != _NoneInstructions)
        {
          _NoneInstructions = value;
          NotifyOfPropertyChange(() => NoneInstructions);
        }
      }
    }

    private string _SomeInstructions = StudyResources.InstructionsSelectFeedbackSome;
    public string SomeInstructions
    {
      get { return _SomeInstructions; }
      set
      {
        if (value != _SomeInstructions)
        {
          _SomeInstructions = value;
          NotifyOfPropertyChange(() => SomeInstructions);
        }
      }
    }

    private string _MostInstructions = StudyResources.InstructionsSelectFeedbackMost;
    public string MostInstructions
    {
      get { return _MostInstructions; }
      set
      {
        if (value != _MostInstructions)
        {
          _MostInstructions = value;
          NotifyOfPropertyChange(() => MostInstructions);
        }
      }
    }

    private string _AllInstructions = StudyResources.InstructionsSelectFeedbackAll;
    public string AllInstructions
    {
      get { return _AllInstructions; }
      set
      {
        if (value != _AllInstructions)
        {
          _AllInstructions = value;
          NotifyOfPropertyChange(() => AllInstructions);
        }
      }
    }

    private Visibility _InstructionsVisibility = Visibility.Visible;
    public Visibility InstructionsVisibility
    {
      get { return _InstructionsVisibility; }
      set
      {
        if (value != _InstructionsVisibility)
        {
          _InstructionsVisibility = value;
          NotifyOfPropertyChange(() => InstructionsVisibility);
        }
      }
    }

    #endregion

    #region Methods

    public override IFeedback GetFeedback(int timeoutMilliseconds)
    {
      TimeSpan timeoutTimeSpan = new TimeSpan(0, 0, 0, 0, timeoutMilliseconds);
      DateTime timeoutDateTime = DateTime.UtcNow + timeoutTimeSpan;
      IsEnabled = true;
      bool feedbackIsProvided = false;
      do
      {
        System.Threading.Thread.Sleep(int.Parse(StudyResources.DefaultFeedbackCheckIntervalMilliseconds));
        var feedbackValue = ((Feedback<double>)Feedback).Value;
        feedbackIsProvided = feedbackValue != -1;
      }
      while (DateTime.UtcNow < timeoutDateTime && !feedbackIsProvided);
      IsEnabled = false;

      return Feedback;
    }

    public override void GetFeedbackAsync(int timeoutMilliseconds,
                                          AsyncCallback<IFeedback> callback)
    {
      try
      {
        IsEnabled = true;
        TimeSpan timeoutTimeSpan = new TimeSpan(0, 0, 0, 0, timeoutMilliseconds);
        DateTime timeoutDateTime = DateTime.UtcNow + timeoutTimeSpan;
        bool feedbackIsProvided = false;
        do
        {
          var feedbackValue = ((Feedback<double>)Feedback).Value;
          feedbackIsProvided = feedbackValue != -1;
          System.Threading.Thread.Sleep(int.Parse(StudyResources.DefaultFeedbackCheckIntervalMilliseconds));
        }
        while (DateTime.UtcNow < timeoutDateTime && !feedbackIsProvided);

        IsEnabled = false;
        callback(this, new ResultArgs<IFeedback>(Feedback));
      }
      catch (Exception ex) 
      {
        IsEnabled = false;
        callback(this, new ResultArgs<IFeedback>(ex));
      }
    }

    private void SetFeedback(double feedbackValue)
    {
      ((Feedback<double>)Feedback).Value = feedbackValue;
    }

    public void None()
    {
      SetFeedback(double.Parse(StudyResources.PercentKnownNone));
    }

    public void Some()
    {
      SetFeedback(double.Parse(StudyResources.PercentKnownSome));
    }

    public void Most()
    {
      SetFeedback(double.Parse(StudyResources.PercentKnownMost));
    }

    public void All()
    {
      SetFeedback(double.Parse(StudyResources.PercentKnownAll));
    }

    #endregion
  }
}
