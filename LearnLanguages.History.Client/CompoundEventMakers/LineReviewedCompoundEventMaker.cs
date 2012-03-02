using System;
using Caliburn.Micro;

namespace LearnLanguages.History.CompoundEventMakers
{
  /// <summary>
  /// This class listens to HistoryPublisher and once the LineReviewing events are published, in proper
  /// order if EventsAreSynchronous == true (false isn't implemented yet), this publishes a compound
  /// event called "ReviewedLineEvent".  This event contains the phraseId, languageId of that phrase, 
  /// timespan duration of review, and feedback (as type double) given.  
  /// </summary>
  public class LineReviewedCompoundEventMaker : CompoundEventMakerBase, 
                                                  IHandle<Events.ReviewingLineEvent>,
                                                  IHandle<Events.ReviewedPhraseEvent>
  {
    public LineReviewedCompoundEventMaker()
    {
      EventsAreSynchronous = true;
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
    private Guid _LineId { get; set; }
    private string _LineText { get; set; }
    private int _LineNumber { get; set; }
    private Guid _PhraseId { get; set; }
    private Guid _LanguageId { get; set; }
    private string _LanguageText { get; set; }
    private TimeSpan _ReviewedPhraseDuration { get; set; }
    private double _FeedbackAsDouble { get; set; }

    private bool _ReviewingLineEventHandled { get; set; }
    private bool _ReviewedPhraseEventHandled { get; set; }

    #endregion
    
    protected override void Reset()
    {
      _ReviewMethodId = Guid.Empty;
      _LineId = Guid.Empty;
      _PhraseId = Guid.Empty;
      _LineText = "";
      _LanguageId = Guid.Empty;
      _LanguageText = "";
      _ReviewedPhraseDuration = TimeSpan.MinValue;
      _FeedbackAsDouble = double.NaN;

      _ReviewingLineEventHandled = false;
      _ReviewedPhraseEventHandled = false;
    }

    #region #1 Reviewing Line
    public void Handle(Events.ReviewingLineEvent message)
    {
      if (EventsAreSynchronous)
      {
        Reset();
        
        _ReviewMethodId = message.Ids[HistoryResources.Key_ReviewMethodId];
        _LineId = message.Ids[HistoryResources.Key_LineId];
        _PhraseId = message.Ids[HistoryResources.Key_PhraseId];
        _LineText = message.Strings[HistoryResources.Key_LineText];
        _LanguageId = message.Ids[HistoryResources.Key_LanguageId];
        _LanguageText = message.Strings[HistoryResources.Key_LanguageText];

        object msgLineNumberObj = (int)-1;
        int msgLineNumber = -1;
        var msgHasLineNumber = message.TryGetDetail(HistoryResources.Key_LineNumber, out msgLineNumberObj);
        if (msgHasLineNumber)
          msgLineNumber = (int)msgLineNumberObj;
        _LineNumber = msgLineNumber;

        _ReviewingLineEventHandled = true;
      }
      else
      {
        //EVENTS ARE ASYNCHRONOUS, I.E. EVENTS CAN RUN IN PARALLEL.  THIS AINT IMPLEMENTED YET.
        throw new NotImplementedException();
      }
    }
    #endregion

    #region #2 Reviewed Phrase 
    /// <summary>
    /// THIS OCCURS AFTER A PHRASE HAS BEEN REVIEWED.  THIS MEANS THE PHRASE WAS VIEWED AND
    /// FEEDBACK WAS GIVEN.  TO START WITH, THIS WILL ONLY OCCUR WHEN THE ENTIRE LINE'S PHRASE
    /// IS REVIEWED.  OTHERWISE, THIS SHOULD NOT BE TRIGGERING.
    /// </summary>
    public void Handle(Events.ReviewedPhraseEvent message)
    {
      if (EventsAreSynchronous)
      {
        if (!_ReviewingLineEventHandled)
        {
          //EITHER WE HAVE SKIPPED A PREVIOUS MESSAGE, THERE IS AN ERROR AND WE WILL 
          //RESET FOR A NEW SAGA.
          Reset();
          Services.Log(HistoryResources.ErrorMsgSagaMessageIncorrectOrder, LogPriority.Medium, LogCategory.Warning);
          return;
        }
                
        var msgPhraseId = message.Ids[HistoryResources.Key_PhraseId];
        var msgPhraseText = message.Strings[HistoryResources.Key_PhraseText];
        var msgLanguageId = message.Ids[HistoryResources.Key_LanguageId];
        var msgLanguageText = message.Strings[HistoryResources.Key_LanguageText];
        var msgReviewedPhraseDuration = message.Duration;
        
        //MAKE SURE PHRASE IDS, LINE TEXT AND PHRASE TEXT, LANGUAGE ID, AND LANGUAGE TEXT MATCH 
        //BETWEEN REVIEWINGLINE EVENT AND REVIEWED PHRASE EVENT
        if (_PhraseId != msgPhraseId ||
            _LineText != msgPhraseText ||
            _LanguageId != msgLanguageId ||
            _LanguageText != msgLanguageText)
        {
          Reset();
          Services.Log(HistoryResources.ErrorMsgSagaMessageIncorrectOrder, LogPriority.Medium, LogCategory.Warning);
          return;
        }

        //GET THE FEEDBACK FROM THE REVIEWED PHRASE EVENT
        _FeedbackAsDouble = message.Doubles[HistoryResources.Key_FeedbackAsDouble];

        
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
      if (!_ReviewingLineEventHandled ||
          !_ReviewedPhraseEventHandled)
        throw new HistoryException();

        //DISPATCH THE REVIEWED LINE COMPOUND EVENT
        var reviewedLineEvent = new Events.LineReviewedEvent(_LineId, _ReviewMethodId, _LineText, _LineNumber, 
          _PhraseId, _LanguageId, _LanguageText, _FeedbackAsDouble, _ReviewedPhraseDuration);
        HistoryPublisher.Ton.PublishEvent(reviewedLineEvent);   
    }
  }
}
