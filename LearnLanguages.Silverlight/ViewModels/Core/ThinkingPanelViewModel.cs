using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using System.Windows;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ThinkingPanelViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ThinkingPanelViewModel : ViewModelBase,
                                        IHandle<History.Events.ThinkingAboutTargetEvent>,
                                        IHandle<History.Events.ThinkedAboutTargetEvent>
                                        
  {
    public ThinkingPanelViewModel()
    {
      History.HistoryPublisher.Ton.SubscribeToEvents(this);
      Thoughts = new List<Guid>();
      ThinkingText = "Ready";
    }

    private List<Guid> Thoughts { get; set; }

    public void Handle(History.Events.ThinkingAboutTargetEvent message)
    {
      //IF WE AREN'T ALREADY TRACKING THIS TargetId, THEN ADD IT TO OUR THOUGHTS
      if (!(Thoughts.Contains(message.TargetId)))
      {
        Thoughts.Add(message.TargetId);
      }

      UpdateThinkingText();
    }

    private void UpdateThinkingText()
    {
      //todo: updatethinkingtext strings to resx
      var thoughtCount = Thoughts.Count;
      if (thoughtCount > 1)
        ThinkingText = "Thinking About " + thoughtCount.ToString() + " Thing(s)..." + (new Random().Next().ToString());
      else if (thoughtCount == 1)
        ThinkingText = "Thinking About 1 Thing..." + (new Random().Next().ToString());
      else 
        ThinkingText = "Ready. Give me something to think about!";
    }

    public void Handle(History.Events.ThinkedAboutTargetEvent message)
    {
      //IF WE ARE TRACKING THIS TargetId, THEN REMOVE IT FROM OUR THOUGHTS
      if (Thoughts.Contains(message.TargetId))
      {
        Thoughts.Remove(message.TargetId);
      }

      UpdateThinkingText();
    }
    
    private string _ThinkingText;
    public string ThinkingText
    {
      get { return _ThinkingText; }
      set
      {
        if (value != _ThinkingText)
        {
          _ThinkingText = value;
          NotifyOfPropertyChange(() => ThinkingText);
        }
      }
    }

    public void OnImportsSatisfied()
    {
      //var coreViewModelName = ViewModelBase.GetCoreViewModelName(typeof(ThinkingPanelViewModel));
      //Services.EventAggregator.Publish(new Events.PartSatisfiedEventMessage(coreViewModelName));
    }
    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
    private Visibility _ViewModelVisibility;
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
  }
}
