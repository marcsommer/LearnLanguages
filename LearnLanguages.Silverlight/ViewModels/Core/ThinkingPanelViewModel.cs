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
                                        IHandle<EventMessages.ThinkingAboutSomethingEventMessage>,
                                        IHandle<EventMessages.ThinkedAboutSomethingEventMessage>
                                        
  {
    public ThinkingPanelViewModel()
    {
      Services.EventAggregator.Subscribe(this);
      Thoughts = new List<Guid>();
      ThinkingText = "Ready";
    }

    private List<Guid> Thoughts { get; set; }

    public void Handle(EventMessages.ThinkingAboutSomethingEventMessage message)
    {
      //IF WE AREN'T ALREADY TRACKING THIS THINKID, THEN ADD IT TO OUR THOUGHTS
      if (!(Thoughts.Contains(message.ThinkId)))
      {
        Thoughts.Add(message.ThinkId);
      }

      UpdateThinkingText();
    }

    private void UpdateThinkingText()
    {
      var thoughtCount = Thoughts.Count;
      if (thoughtCount > 0)
        ThinkingText = "Thinking About " + thoughtCount.ToString() + " Thing(s)..." + (new Random().Next().ToString());
      else
        ThinkingText = "Ready";
    }

    public void Handle(EventMessages.ThinkedAboutSomethingEventMessage message)
    {
      //IF WE ARE TRACKING THIS THINKID, THEN REMOVE IT FROM OUR THOUGHTS
      if (!(Thoughts.Contains(message.ThinkId)))
      {
        Thoughts.Remove(message.ThinkId);
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
