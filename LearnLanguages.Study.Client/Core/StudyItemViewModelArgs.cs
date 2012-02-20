using System;
using System.Net;
using LearnLanguages.Study.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Study
{
  [Serializable]
  public class StudyItemViewModelArgs
  {
    /// <summary>
    /// This is for serialization.  Do not use this ctor.
    /// </summary>
    public StudyItemViewModelArgs()
    {
    }

    public StudyItemViewModelArgs(IStudyItemViewModelBase viewModel)
    {
      ViewModel = viewModel;
    }

    public IStudyItemViewModelBase ViewModel { get; private set; }
  }
}
