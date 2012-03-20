using System;
using System.Linq;
using LearnLanguages.Business;
using System.ComponentModel.Composition;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using Caliburn.Micro;
using LearnLanguages.Offer;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Navigation.EventMessages;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The questions for order include what is the following line to any given line.
  /// So, for any list of lines of count n, the number of questions is n-1.  Add to 
  /// this two specialized questions: 1st and last line.  This gives us n+1 distinct
  /// questions to know.
  /// 
  /// If we ask in order of the lines, then the beginning of the song will get precedence
  /// over the end of the song...if we go back to beginning each time we get one wrong.
  /// We could subdivide the text into groups of lines of size L.  We can keep an in memory
  /// object to keep track of each of these groups.  We study each in order.  If we get it all 
  /// correct, then we just go through the lines of the MLT in order, as if we are just reading
  /// it actively.  If we miss one, then we skip to the next group.  When we return to the previous 
  /// group, we start at the beginning.  Once we have reviewed each of the groups correctly, all
  /// groups are considered unlearned, and we restart the process.
  /// 
  /// A group as such can be considered an MLT itself.  So, our dictionary of studiers will be 
  /// indexed by its order in the global concatenation of MLTs.  We keep two current position 
  /// indexes: CurrentGroupIndex, and CurrentLineInGroupIndex.  We also keep a KnownGroupIndexes 
  /// list.
  /// </summary>
  public class DefaultMultiLineTextsOrderStudier : StudierBase<MultiLineTextList>,
                                                   IHandle<NavigationRequestedEventMessage>
  {
    public DefaultMultiLineTextsOrderStudier()
    {
      _Studiers = new Dictionary<Guid, DefaultSingleMultiLineTextOrderStudier>();
      Services.EventAggregator.Subscribe(this);//navigation
    }

    private int _CurrentMultiLineTextIndex = -1;

    ///// <summary>
    ///// Collection of Studiers, indexed by the MLT that they study.
    ///// </summary>
    //private Dictionary<Guid, DefaultSingleMultiLineTextOrderStudier> _Studiers { get; set; }

    #region Methods

    public override void GetNextStudyItemViewModel(AsyncCallback<StudyItemViewModelArgs> callback)
    {
      if (_AbortIsFlagged)
      {
        callback(this, new Common.ResultArgs<StudyItemViewModelArgs>(StudyItemViewModelArgs.Aborted));
        return;
      }
      //WE ARE CYCLING THROUGH ALL OF THE MLTS, SO GET NEXT MLT TO STUDY INDEX
      var multiLineTextIndex = GetNextMultiLineTextIndex();
      var currentMultiLineText = _Target[multiLineTextIndex];
      var studier = _Studiers[currentMultiLineText.Id];

      //WE HAVE ALREADY INITIALIZED EACH STUDIER, SO NOW JUST DELEGATE THE WORK TO THE STUDIER
      studier.GetNextStudyItemViewModel(callback);
    }

    private int GetNextMultiLineTextIndex()
    {
      //cycle through the , and get the corresponding studier for this MLT.
      _CurrentMultiLineTextIndex++;
      if (_CurrentMultiLineTextIndex > (_Target.Count - 1))
        _CurrentMultiLineTextIndex = 0;

      return _CurrentMultiLineTextIndex;
    }

    public double GetPercentKnown()
    {
      if (_Studiers.Count == 0)
        return 0.0d;

      var totalLineCount = 0;
      double totalPercentKnownNonNormalized = 0.0d;
      foreach (var studier in _Studiers)
      {
        var multiLineTextId = studier.Key;
        var results = from mlt in _Target
                      where mlt.Id == multiLineTextId
                      select mlt;
        var studierLineCount = results.First().Lines.Count;
        //var studierLineCount = studier.Key.Lines.Count;
        totalLineCount += studierLineCount;

        var studierPercentKnown = studier.Value.GetPercentKnown();
        totalPercentKnownNonNormalized += (studierPercentKnown / 100.0d) * studierLineCount;
        //eg 50% known 10 lines
        //25% known 1000 lines.  Percent known should be just over 25% known (255/1010 to be exact);
        //totalLineCount = 1010
        //totalPercentKnownNonNormalized = (.5 * 10) + (.25 * 1000) = 255
        //totalPercentKnownNonNormalized = 255 / 1010 = .252475
      }

      var totalPercentKnownNormalized = totalPercentKnownNonNormalized / totalLineCount;

      return totalPercentKnownNormalized;
    }

    #endregion

    public override void InitializeForNewStudySession(MultiLineTextList target,
                                                      ExceptionCheckCallback completedCallback)
    {
      _AbortIsFlagged = false;

      _Target = target;
      _Studiers.Clear();
      int initializedCount = 0;

      //CREATE STUDIER FOR EACH MULTILINETEXT IN MULTILINETEXTS TARGET
      var newJobMultiLineTextList = _Target;
      foreach (var multiLineTextEdit in newJobMultiLineTextList)
      {
        if (_AbortIsFlagged)
        {
          completedCallback(null);
          return;
        }
        var studier = new DefaultSingleMultiLineTextOrderStudier();
        studier.InitializeForNewStudySession(multiLineTextEdit, (e) =>
        {
          if (e != null)
            throw e;

          if (_AbortIsFlagged)
          {
            completedCallback(null);
            return;
          }

          _Studiers.Add(multiLineTextEdit.Id, studier);
          initializedCount++;
          if (initializedCount == newJobMultiLineTextList.Count)
            completedCallback(null);
        });
      }
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
