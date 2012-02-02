using System;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.ViewModels
{
  public class AskUserExtraDataViewModel : IViewModelBase
  {

    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(StudyResources.ShowGridLines); }
    }
    public void OnImportsSatisfied()
    {
      //
    }
  }
}
