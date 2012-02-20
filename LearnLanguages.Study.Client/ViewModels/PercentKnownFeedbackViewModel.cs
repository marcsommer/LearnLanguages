using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Study.ViewModels
{
  /// <summary>
  /// Stateless viewmodel, just used for providing an interface to the user with
  /// providing feedback as to how much of something should be considered "known" 
  /// by the user.
  /// </summary>
  [Export(typeof(PercentKnownFeedbackViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class PercentKnownFeedbackViewModel : ViewModelBase
  {
    #region Ctors and Init

    public PercentKnownFeedbackViewModel()
    {
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

    private double _PercentKnown = -1;
    public double PercentKnown
    {
      get { return _PercentKnown; }
      set
      {
        if (value != _PercentKnown)
        {
          _PercentKnown = value;
          NotifyOfPropertyChange(() => PercentKnown);
        }
      }
    }

    #endregion

    #region Methods

    public void None()
    {
      PercentKnown = double.Parse(StudyResources.PercentKnownNone);
    }

    public void Some()
    {
      PercentKnown = double.Parse(StudyResources.PercentKnownSome);
    }

    public void Most()
    {
      PercentKnown = double.Parse(StudyResources.PercentKnownMost);
    }

    public void All()
    {
      PercentKnown = double.Parse(StudyResources.PercentKnownAll);
    }

    #endregion
  }
}
