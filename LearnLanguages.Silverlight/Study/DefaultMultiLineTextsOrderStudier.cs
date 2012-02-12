using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Caliburn.Micro;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common.Args;
using LearnLanguages.Common.Interfaces;
using System.Windows;
using LearnLanguages.DataAccess;
using LearnLanguages.Silverlight.ViewModels;

namespace LearnLanguages.Silverlight
{
  /// <summary>
  /// 
  /// </summary>
  //[Export(typeof(IStudyMultiLineTexts))]
  public class DefaultMultiLineTextsOrderStudier: ViewModelBase<MultiLineTextList, MultiLineTextEdit, MultiLineTextDto>
  {
    #region Ctors and Init

    public DefaultMultiLineTextsOrderStudier()
    {

    }

    #endregion

    #region Properties

    private IEventAggregator _EventAggregator { get; set; }

    #endregion

    #region Methods

    public void StudyMultiLineTexts(MultiLineTextList multiLineTexts, IEventAggregator eventAggregator)
    {
      //get study multiple songs view
      ModelList = multiLineTexts;
      _EventAggregator = eventAggregator;

      //
    }

    #endregion

    #region Base
    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
    private Visibility _ViewModelVisibility = Visibility.Visible;
    public Visibility ViewModelVisibility
    {
      get { return _ViewModelVisibility; }
      set
      {
        if (value != _ViewModelVisibility)
        {
          _ViewModelVisibility = value;
          NotifyOfPropertyChange(() => ViewModelVisibility);
        }
      }
    }
    public void OnImportsSatisfied()
    {
      //
    } 
    #endregion
  }
}
