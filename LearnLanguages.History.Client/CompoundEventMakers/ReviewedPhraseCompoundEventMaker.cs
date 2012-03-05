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
      EventsAreSynchronous = true;
      var isEnabled = bool.Parse(HistoryResources.IsEnabledReviewedPhraseCompoundEventMaker);
      if (isEnabled)
        Enable();
    }

    /// <summary>
    /// If events are synchronous, this resets every time an event is handled out of order.  
    /// This defaults to true.
    /// 
    /// E.g. If we hear a Viewing event for a phrase part of line, then hear feedback, *WITHOUT*
    /// hearing an intermediate event for viewed that phrase, then we assume something wrong happened
    /// and we reset anew.  
    /// E.g. #2: If we hear a Viewing event for a phrase, then hear another Viewing event
    /// for a different phrase, we reset for the most recent Viewing event, assigning state to the more
    /// recent phrase.
    /// </summary>
    public bool EventsAreSynchronous { get; set; }

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
    
    protected override void Reset()
    {
      _ReviewingEventHandled = false;
      _FeedbackGivenEventHandled = false;
      _ViewingEventHandled = false;
      _ViewedEventHandled = false;

      _FeedbackTimestamp = DateTime.MinValue;
      _ViewingTimestamp = DateTime.MinValue;
      _ViewedTimestamp = DateTime.MinValue;
    }

    #region #1 Reviewing Phrase
    public void Handle(Events.ReviewingPhraseEvent message)
    {
      if (EventsAreSynchronous)
      {
        Reset();
        _ReviewingEventHandled = true;
      }
      else
      {
        //EVENTS ARE ASYNCHRONOUS, I.E. EVENTS CAN RUN IN PARALLEL.  THIS AINT IMPLEMENTED YET.
        throw new NotImplementedException();
      }
    }
    #endregion

    #region #2 Phrase Viewing
    /// <summary>
    /// THIS OCCURS AT THE START OF SHOWING THE PHRASE.  IT ALWAYS RESETS THIS COMPOUND EVENT MAKER
    /// IF EVENTS ARE SYNCHRONOUS, BECAUSE THIS IS THE FIRST MESSAGE THAT WE LISTEN FOR.
    /// </summary>
    public void Handle(Events.ViewingPhraseOnScreenEvent message)
    {
      if (EventsAreSynchronous)
      {
        if (!_ReviewingEventHandled)
        {
          //EITHER WE HAVE SKIPPED A PREVIOUS MESSAGE, THERE IS AN ERROR AND WE WILL 
          //RESET FOR A NEW SAGA.
          Reset();
          Services.Log(HistoryResources.ErrorMsgSagaMessageIncorrectOrder, LogPriority.Medium, LogCategory.Warning);
          return;
        }

        var phraseId = (Guid)message.GetDetail(HistoryResources.Key_PhraseId);
        var languageId = (Guid)message.GetDetail(HistoryResources.Key_LanguageId);

        _PhraseId = phraseId;
        _LanguageId = languageId;

        _ViewingEventHandled = true;
        _ViewingTimestamp = DateTime.Now;
      }
      else
      {
        //EVENTS ARE ASYNCHRONOUS, I.E. EVENTS CAN RUN IN PARALLEL.  THIS AINT IMPLEMENTED YET.
        throw new NotImplementedException();
      }
    }
    #endregion

    #region #3 Phrase Viewed
    /// <summary>
    /// THIS OCCURS AFTER WE HAVE COMPLETED SHOWING THE PHRASE AND IT IS NOW NO LONGER IN VIEW.
    /// THIS IS THE SECOND MESSAGE WE LISTEN FOR.
    /// </summary>
    public void Handle(Events.ViewedPhraseOnScreenEvent message)
    {
      if (EventsAreSynchronous)
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
          //EITHER WE HAVE SKIPPED OUR FIRST MESSAGE, OR THE PHRASEID IS WRONG
          //OR THE LANGUAGE ID IS WRONG.  IN ANY CASE, THERE IS AN ERROR AND WE WILL 
          //RESET FOR A NEW SAGA.
          Reset();
          Services.Log(HistoryResources.ErrorMsgSagaMessageIncorrectOrder, LogPriority.Medium, LogCategory.Warning);
          return;
        }
      }
      else
      {
        //EVENTS ARE ASYNCHRONOUS, I.E. EVENTS CAN RUN IN PARALLEL.  THIS AINT IMPLEMENTED YET.
        throw new NotImplementedException();
      }
    }
    #endregion

    #region #4 Feedback Given (dispatches compound event if successful)
    public void Handle(Events.FeedbackAsDoubleGivenEvent message)
    {
      if (EventsAreSynchronous)
      {
        if (!_ViewingEventHandled || !_ViewedEventHandled)
        {
          //EITHER FIRST OR SECOND STEPS HAVE BEEN SKIPPED, AND WE ARE SYNCHRONOUS, 
          //SO THIS IS AN ERROR AND THE FEEDBACK IS MOOT.
          Reset();
          Services.Log(HistoryResources.ErrorMsgSagaMessageIncorrectOrder, LogPriority.Medium, LogCategory.Warning);
          return;
        }

        var feedback = (double)message.GetDetail(HistoryResources.Key_FeedbackAsDouble);

        _FeedbackGivenEventHandled = true;
        _FeedbackTimestamp = DateTime.Now;

        DispatchCompoundEvent();
      }
      else
      {
        //EVENTS ARE ASYNCHRONOUS, I.E. EVENTS CAN RUN IN PARALLEL.  THIS AINT IMPLEMENTED YET.
        throw new NotImplementedException();
      }
    }
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
  }
}
