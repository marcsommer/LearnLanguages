using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  public class ViewModelBase : PropertyChangedBase, IViewModelBase
  {
    public ViewModelBase()
    {
      //Services.Container.SatisfyImportsOnce(this);
    }

    /// <summary>
    /// Use this to configure the ViewModel's properties.  
    /// The intended format (I haven't implemented it yet) is key= property name, value = property value as string.
    /// Return true if query string was loaded correctly.
    /// Default base implementation just returns true, ignoring the queryString parameter.
    /// </summary>
    public bool LoadFromUri(Uri uri)
    {
      return true;
    }

    /// <summary>
    /// Publish Parts Satisfied to EventAggregator.
    /// </summary>
    public virtual void OnImportsSatisfied()
    {
      Services.EventAggregator.Publish(
        new Events.PartSatisfiedEventMessage(ViewModelBase.GetCoreViewModelName(GetType().Name, false)));
    }

    ///// <summary>
    ///// Retrieves core test of ViewModel's type name, using convention "[CoreText]ViewModel".  
    ///// Returns [Text], optionally inserting spaces
    ///// before any capital letters if withSpaces == true.  
    ///// E.g. AddNewViewModel, withSpaces = false returns "AddNew".
    ///// AddNewViewModel, withSpaces = true returns "Add New".
    ///// </summary>
    ///// <returns>
    ///// Returns [Text], optionally inserting spaces
    ///// before any capital letters if withSpaces == true.  
    ///// E.g. AddNewViewModel, withSpaces = false returns "AddNew".
    ///// AddNewViewModel, withSpaces = true returns "Add New".
    ///// </returns>
    //public static string GetCoreViewModelName(bool withSpaces = false)
    //  where TViewModel : IViewModelBase
    //{
    //  return GetCoreViewModelName(typeof(TViewModel).Name, withSpaces);
    //}
    public static string GetCoreViewModelName(Type viewModelType, bool withSpaces = false)
    {
      return GetCoreViewModelName(viewModelType.Name, withSpaces);
    }

    private static string GetCoreViewModelName(string viewModelTypeName, bool withSpaces = false)
    {
      string retCoreViewModelName = "";

      //var thisTypeName = this.GetType().Name;
      var thisTypeName = viewModelTypeName;

      var thisTypeMinusViewModelText =
        thisTypeName.Replace(@"ViewModel", "");
      //var thisTypeMinusViewModelText = 
      //thisTypeName.Substring(0, (thisTypeName.Length - @"ViewModel".Length));

      retCoreViewModelName = thisTypeMinusViewModelText;

      if (withSpaces)
      {
        //ADD SPACES PRECEDING CAPITAL LETTERS
        var textStringWithSpaces = "";
        textStringWithSpaces += thisTypeMinusViewModelText[0];//The first letter has no preceding space
        for (int i = 1; i < thisTypeMinusViewModelText.Length; i++)
        {
          var character = thisTypeMinusViewModelText[i];
          if (Char.IsLower(character))
            textStringWithSpaces += character;
          else
            textStringWithSpaces += " " + character;
        }
        retCoreViewModelName = textStringWithSpaces;
      }

      return retCoreViewModelName;
    }

    //public string GetCoreViewModelName(bool withSpaces = false)
    //{
    //  throw new NotImplementedException();
    //}

    private bool _ShowGridLines = bool.Parse(AppResources.ShowGridLines);
    public bool ShowGridLines
    {
      get { return _ShowGridLines; }
      set
      {
        if (value != _ShowGridLines)
        {
          _ShowGridLines = value;
          NotifyOfPropertyChange(() => ShowGridLines);
        }
      }
    }
  }
}
