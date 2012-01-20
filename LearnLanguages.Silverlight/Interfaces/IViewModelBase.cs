using System;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.Interfaces
{
  public interface IViewModelBase : IPartImportsSatisfiedNotification 
  {
    bool LoadFromUri(Uri uri);
    //string GetCoreViewModelName(bool withSpaces = false);
    bool ShowGridLines { get; }
  }
}
