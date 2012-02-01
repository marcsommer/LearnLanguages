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
using System.Collections.Specialized;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{
  public abstract class ViewModelBase<TCslaModelList, TCslaModel, TCslaModelDto> : ViewModelBase
    where TCslaModelList : Common.CslaBases.BusinessListBase<TCslaModelList, TCslaModel, TCslaModelDto>
    where TCslaModel : Common.CslaBases.BusinessBase<TCslaModel, TCslaModelDto>
    where TCslaModelDto : class
  {
    private TCslaModelList _ModelList;
    public TCslaModelList Model
    {
      get { return _ModelList; }
      set
      {
        if (value != _ModelList)
        {
          UnhookFrom(_ModelList);
          _ModelList = value;
          HookInto(_ModelList);
          NotifyOfPropertyChange(() => Model);
        }
      }
    }

    protected virtual void HookInto(TCslaModelList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void UnhookFrom(TCslaModelList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void HandleChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.Model);
      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanCancelEdit);
    }
    protected virtual void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.Model);
      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanCancelEdit);
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

          Model = (TCslaModelList)r.NewObject;
          NotifyOfPropertyChange(() => CanSave);
        });
    }

    public bool CanCancelEdit
    {
      get
      {
        return (Model != null && Model.IsDirty);
      }
    }
    public virtual void CancelEdit()
    {
      Model.CancelEdit();
      NotifyOfPropertyChange(() => CanCancelEdit);
      NotifyOfPropertyChange(() => CanSave);
    }


  }
}
