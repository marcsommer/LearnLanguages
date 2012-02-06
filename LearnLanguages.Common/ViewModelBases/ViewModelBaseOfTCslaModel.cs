using System.ComponentModel;
using LearnLanguages.Common.Interfaces;
using System;

namespace LearnLanguages.Common.ViewModelBases
{
  public abstract class ViewModelBase<TCslaModel, TCslaModelDto> : ViewModelBase,
                                                                   IHaveModel<TCslaModel>
    where TCslaModel : Common.CslaBases.BusinessBase<TCslaModel, TCslaModelDto>
    where TCslaModelDto : class
  {
    private TCslaModel _Model;
    public TCslaModel Model
    {
      get { return _Model; }
      set
      {
        SetModel(value);
      }
    }

    public virtual void SetModel(TCslaModel model)
    {
      if (model != _Model)
      {
        UnhookFrom(_Model);
        _Model = model;
        HookInto(_Model);
        NotifyOfPropertyChange(() => Model);
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
    protected virtual void HandleModelPropertyChanged(object sender, PropertyChangedEventArgs e)
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
        if (Saved != null)
          Saved(this, new EventArgs.ModelEventArgs<TCslaModel>(Model));
        NotifyOfPropertyChange(() => CanSave);
      });
    }
    public event EventHandler<Common.EventArgs.ModelEventArgs<TCslaModel>> Saved;
  }
}
