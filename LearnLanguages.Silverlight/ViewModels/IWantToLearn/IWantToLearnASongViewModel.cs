using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Silverlight.Interfaces;
using Caliburn.Micro;
using System.Windows;
using LearnLanguages.Business;
using System.ComponentModel;
using System;
using LearnLanguages.Delegates;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(IWantToLearnASongViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class IWantToLearnASongViewModel : ViewModelBase
  {
    
  }
}
