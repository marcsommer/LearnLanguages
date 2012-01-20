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
using System.ComponentModel;

namespace LearnLanguages.Silverlight.ViewModels
{
  public abstract class ViewModelBase<TCslaModel, TCslaModelDto> : ViewModelBase
    where TCslaModel : Business.Bases.BusinessBase<TCslaModel, TCslaModelDto>
    where TCslaModelDto : class
  {
    private TCslaModel _Model;
    public TCslaModel Model
    {
      get { return _Model; }
      set
      {
        if (value != _Model)
        {
          UnhookFrom(_Model);
          _Model = value;
          HookInto(_Model);
          NotifyOfPropertyChange(() => Model);
        }
      }
    }

    protected virtual void HookInto(TCslaModel model)
    {
      if (model != null)
        model.PropertyChanged += HandleModelPropertyChanged;
    }
    protected virtual void UnhookFrom(TCslaModel model)
    {
      if (model != null)
        model.PropertyChanged -= HandleModelPropertyChanged;
    }
    void HandleModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => e.PropertyName);
      NotifyOfPropertyChange(() => CanSave);
    }

    public bool CanSave
    {
      get
      {
        return (Model != null && Model.IsSavable);
      }
    }
    public virtual void Save()
    {
      Model.BeginSave((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          Model = (TCslaModel)r.NewObject;
          NotifyOfPropertyChange(() => CanSave);
        });
    }
  }
}
