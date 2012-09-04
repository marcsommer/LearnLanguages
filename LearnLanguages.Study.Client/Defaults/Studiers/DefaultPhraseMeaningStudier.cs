using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Common;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Navigation.EventMessages;
using Caliburn.Micro;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The studier that actually "does" something.
  /// 
  /// This implementation: "Dumb" studier.  This doesn't know anything about whether 
  /// a phrase's history, it "simply" takes the phrase, produces an offer to show a Q & A about it.
  /// Listens for the ViewModel to publish a Q & A response
  /// </summary>
  public class DefaultPhraseMeaningStudier : StudierBase<PhraseEdit>, 
                                             IStudyReviewMethod,
                                             IHandle<NavigationRequestedEventMessage>

  {
    #region Ctors and Init
    public DefaultPhraseMeaningStudier()
    {
      Services.EventAggregator.Subscribe(this);//navigation
    }
    #endregion

    #region Methods

    public override void InitializeForNewStudySession(PhraseEdit target, ExceptionCheckCallback completedCallback)
    {
#if DEBUG
      var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif

      _AbortIsFlagged = false;
      _Target = target;
      completedCallback(null);
    }

    public override void GetNextStudyItemViewModel(Common.Delegates.AsyncCallback<StudyItemViewModelArgs> callback)
    {
      if (_AbortIsFlagged)
      {
        callback(this, new ResultArgs<StudyItemViewModelArgs>(StudyItemViewModelArgs.Aborted));
        return;
      }
      //USING THE TARGET PHRASE, 
      //IF IT IS IN THE NATIVE LANGUAGE, THEN IT JUST POPS UP A NATIVE LANGUAGE STUDY QUESTION.
      //IF IT IS IN A DIFFERENT LANGUAGE, THEN IT POPS UP EITHER DIRECTION Q & A, 50% CHANCE.
      StudyDataRetriever.CreateNew((s, r) =>
      {
        if (r.Error != null)
        {
          callback(this, new ResultArgs<StudyItemViewModelArgs>(r.Error));
          return;
        }

        if (_AbortIsFlagged)
        {
          callback(this, new ResultArgs<StudyItemViewModelArgs>(StudyItemViewModelArgs.Aborted));
          return;
        }

        var retriever = r.Object;
        var nativeLanguageText = retriever.StudyData.NativeLanguageText;
        if (string.IsNullOrEmpty(nativeLanguageText))
          throw new StudyException("No native language set.");

        if (_Target == null)
          throw new StudyException("No PhraseEdit to study, _StudyJobInfo.Target == null.");

        var phraseEdit = _Target;
        var phraseText = phraseEdit.Text;
        if (string.IsNullOrEmpty(phraseText))
          throw new StudyException("Attempted to study empty phrase text, (PhraseEdit)_Target.Text is null or empty.");

        var languageText = phraseEdit.Language.Text;

        //WE HAVE A PHRASEEDIT WITH A LANGUAGE AND WE HAVE OUR NATIVE LANGUAGE, 
        //SO WE HAVE ENOUGH TO PROCURE A VIEW MODEL NOW.
        StudyItemViewModelFactory.Ton.Procure(phraseEdit, nativeLanguageText, (s2, r2) =>
          {
            if (r2.Error != null)
            {
              callback(this, new ResultArgs<StudyItemViewModelArgs>(r2.Error));
              return;
            }

            if (_AbortIsFlagged)
            {
              callback(this, new ResultArgs<StudyItemViewModelArgs>(StudyItemViewModelArgs.Aborted));
              return;
            }

            var studyItemViewModel = r2.Object;
            studyItemViewModel.Shown += new EventHandler(studyItemViewModel_Shown);
            var result = new StudyItemViewModelArgs(studyItemViewModel);
            _Phrase = phraseEdit;//hack
            callback(this, new ResultArgs<StudyItemViewModelArgs>(result));
          });
      });
    }

    private void studyItemViewModel_Shown(object sender, EventArgs e)
    {
      if (_AbortIsFlagged)
        return;

      //we are now reviewing a phrase
      var eventReviewingPhrase = new History.Events.ReviewingPhraseEvent(_Phrase, ReviewMethodId);
      History.HistoryPublisher.Ton.PublishEvent(eventReviewingPhrase);
    }

    private PhraseEdit _Phrase { get; set; }

    #endregion

    public Guid ReviewMethodId
    {
      get { return Guid.Parse(StudyResources.ReviewMethodIdDefaultPhraseMeaningStudier); }
    }

    public void Handle(NavigationRequestedEventMessage message)
    {
      AbortStudying();
    }

    private void AbortStudying()
    {
      _AbortIsFlagged = true;
    }

    private object _AbortLock = new object();
    private bool _abortIsFlagged = false;
    private bool _AbortIsFlagged
    {
      get
      {
        lock (_AbortLock)
        {
          return _abortIsFlagged;
        }
      }
      set
      {
        lock (_AbortLock)
        {
          _abortIsFlagged = value;
        }
      }
    }
  }
}
