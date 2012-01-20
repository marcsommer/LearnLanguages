using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LanguageSelectorViewModel))]
  public class LanguageSelectorViewModel : Conductor<IScreen>.Collection.OneActive, Interfaces.IViewModelBase
  {


    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public void OnImportsSatisfied()
    {
      Events.Publish.PartSatisfied(ViewModelBase.GetCoreViewModelName(GetType()));
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
  }
}
