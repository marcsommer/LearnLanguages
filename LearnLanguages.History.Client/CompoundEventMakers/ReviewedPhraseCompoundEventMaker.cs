using System;
using Caliburn.Micro;

namespace LearnLanguages.History.CompoundEventMakers
{
  /// <summary>
  /// This class listens to HistoryPublisher and once the PhraseReviewing events are published, in proper
  /// order if EventsAreSynchronous == true (false isn't implemented yet), this publishes a compound
  /// event called "ReviewedPhraseEvent".  This event contains the phraseId, languageId of that phrase, 
  /// timespan duration of review, and feedback (as type double) given.  
  /// </summary>
  public class ReviewedPhraseCompoundEventMaker : CompoundEventMakerBase, 
                                                  IHandle<Events.ReviewingPhraseEvent>,
                                                  IHandle<Events.ViewingPhraseOnScreenEvent>,
                                                  IHandle<Events.ViewedPhraseOnScreenEvent>,
                                                  IHandle<Events.FeedbackAsDoubleGivenEvent>
  {
    public ReviewedPhraseCompoundEventMaker()
    {
      var isEnabled = bool.Parse(HistoryResources.IsEnabledReviewedPhraseCompoundEventMaker);
      if (isEnabled)
        Enable();
      Reset();
    }

    #region State

    private Guid _ReviewMethodId { get; set; }
    private Guid _PhraseId { get; set; }
    private Guid _LanguageId { get; set; }
    private double _FeedbackAsDouble { get; set; }

    private bool _ReviewingEventHandled { get; set; }
    private bool _ViewingEventHandled { get; set; }
    private bool _ViewedEventHandled { get; set; }
    private bool _FeedbackGivenEventHandled { get; set; }

    private DateTime _ViewingTimestamp { get; set; }
    private DateTime _ViewedTimestamp { get; set; }
    private DateTime _FeedbackTimestamp { get; set; }

    #endregion

    private bool _IsReset { get; set; }
    protected override void Reset()
    {
      _ReviewingEventHandled = false;
      _FeedbackGivenEventHandled = false;
      _ViewingEventHandled = false;
      _ViewedEventHandled = false;

      _FeedbackTimestamp = DateTime.MinValue;
      _ViewingTimestamp = DateTime.MinValue;
      _ViewedTimestamp = DateTime.MinValue;
      _IsReset = true;
    }

    #region Saga Events

    #region #1 Reviewing Phrase
    public void Handle(Events.ReviewingPhraseEvent message)
    {
      Reset();
      _IsReset = false;
      _ReviewingEventHandled = true;
      _ReviewMethodId = message.GetDetail<Guid>(HistoryResources.Key_ReviewMethodId);
    }
    #endregion

    #region #2 Phrase Viewing
    /// <summary>
    /// THIS OCCURS AT THE START OF SHOWING THE PHRASE.
    /// </summary>
    public void Handle(Events.ViewingPhraseOnScreenEvent message)
    {
      if (!_ReviewingEventHandled)
      {
        return;
      }

      var phraseId = (Guid)message.GetDetail(HistoryResources.Key_PhraseId);
      var languageId = (Guid)message.GetDetail(HistoryResources.Key_LanguageId);

      _PhraseId = phraseId;
      _LanguageId = languageId;

      _ViewingEventHandled = true;
      _ViewingTimestamp = DateTime.Now;
    }
    #endregion

    #region #3 Phrase Viewed
    /// <summary>
    /// THIS OCCURS AFTER WE HAVE COMPLETED SHOWING THE PHRASE AND IT IS NOW NO LONGER IN VIEW.
    /// THIS IS THE SECOND MESSAGE WE LISTEN FOR.
    /// </summary>
    public void Handle(Events.ViewedPhraseOnScreenEvent message)
    {
      var phraseId = (Guid)message.GetDetail(HistoryResources.Key_PhraseId);
      var languageId = (Guid)message.GetDetail(HistoryResources.Key_LanguageId);

      if (_ViewingEventHandled &&
          phraseId == _PhraseId &&
          languageId == _LanguageId)
      {
        _ViewedEventHandled = true;
        _ViewedTimestamp = DateTime.Now;
      }
      else
      {
        return;
      }
    }
    #endregion

    #region #4 Feedback Given (dispatches compound event if successful)
    public void Handle(Events.FeedbackAsDoubleGivenEvent message)
    {
      if (!_ViewingEventHandled || !_ViewedEventHandled)
      {
        return;
      }

      var feedback = (double)message.GetDetail(HistoryResources.Key_FeedbackAsDouble);

      _FeedbackGivenEventHandled = true;
      _FeedbackTimestamp = DateTime.Now;

      DispatchCompoundEvent();
    }
    #endregion

    #endregion

    private void DispatchCompoundEvent()
    {
      if (!_ReviewingEventHandled ||
          !_ViewingEventHandled ||
          !_ViewedEventHandled ||
          !_FeedbackGivenEventHandled ||
          _PhraseId == Guid.Empty)
        throw new HistoryException();

      var duration = _ViewedTimestamp - _ViewingTimestamp;
      var reviewedEvent = new Events.ReviewedPhraseEvent(_PhraseId, _LanguageId, _ReviewMethodId, duration);
      HistoryPublisher.Ton.PublishEvent(reviewedEvent);
    }

    public override void Enable()
    {
      Reset();
      base.Enable();
    }
  }
}
