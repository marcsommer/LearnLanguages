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
    #region Ctors and Init

    public DefaultMultiLineTextsOrderStudier()
    {
      Services.EventAggregator.Subscribe(this);//navigation
      LineGroupSize = int.Parse(StudyResources.DefaultOrderStudierLineGroupSize);
      _KnownGroupNumbers = new List<int>();
    }

    #endregion

    #region Methods

    public override void GetNextStudyItemViewModel(AsyncCallback<StudyItemViewModelArgs> callback)
    {
      if (_AbortIsFlagged)
      {
        callback(this, new Common.ResultArgs<StudyItemViewModelArgs>(StudyItemViewModelArgs.Aborted));
        return;
      }

      //IF WE KNOW ALL OF OUR GROUPS, RESET AND START OVER
      if (_KnownGroupNumbers.Count == _LineGroups.Count)
        _KnownGroupNumbers.Clear();

      //GET THE INITIAL GROUP NUMBER TO TRY TO STUDY
      var groupNumberToStudy = _LastGroupNumberStudied + 1;
      if (groupNumberToStudy == _LineGroups.Count)
        groupNumberToStudy = 0;

      //FIND THE FIRST GROUP NUMBER THAT IS UNKNOWN
      for (int i = 0; i < _LineGroups.Count; i++)
      {
        if (!_KnownGroupNumbers.Contains(groupNumberToStudy))
          break;

        //THAT GROUP NUMBER IS ALREADY KNOWN.  UPDATE FOR THE NEXT TRY.
        groupNumberToStudy++;
        if (groupNumberToStudy == _LineGroups.Count)
          groupNumberToStudy = 0;
      }

      //WE NOW HAVE THE GROUP NUMBER TO STUDY.  
      var group = _LineGroups[groupNumberToStudy];
      var lineToStudy = group.GetLine();

      //WE NOW HAVE THE LINE TO STUDY.  PROCURE A LINE ORDER 
      StudyItemViewModelFactory.Ton.Procure(lineToStudy, group.MultiLineText, (s, r) =>
        {
          if (r.Error != null)
            callback(this, new Common.ResultArgs<StudyItemViewModelArgs>(r.Error));

          var viewModel = r.Object;
          StudyItemViewModelArgs args = new StudyItemViewModelArgs(viewModel);
          callback(this, new Common.ResultArgs<StudyItemViewModelArgs>(args));
          return;
        });
      return;
    }

    public override void InitializeForNewStudySession(MultiLineTextList target,
                                                      ExceptionCheckCallback completedCallback)
    {
      try
      {
        _AbortIsFlagged = false;
        _Target = target;
        if (_AbortIsFlagged)
        {
          completedCallback(null);
          return;
        }
        _LastGroupNumberStudied = -1;
        PopulateGroups();
        _KnownGroupNumbers.Clear();

        completedCallback(null);
      }
      catch (Exception ex)
      {
        completedCallback(ex);
      }
    }

    private void PopulateGroups()
    {
      _LineGroups = new List<LineGroup>();
      var currentGroupIndex = -1;

      foreach (var multiLineText in _Target)
      {
        //get ordered list of lines in MLT
        var orderedList = multiLineText.Lines.OrderBy<LineEdit, int>((l) =>
          {
            return l.LineNumber;
          }).ToList();

        var count = orderedList.Count();
        
        //WE ARE GOING TO CREATE GROUPS WITH GROUPSIZE NUMBER OF LINES PER GROUP.
        LineGroup tmpGroup = null;

        for (int i = 0; i < count; i++)
        {
          var line = orderedList[i];

          if (tmpGroup == null)
          {
            currentGroupIndex++;
            tmpGroup = new LineGroup(currentGroupIndex, multiLineText);
          }
          tmpGroup.Lines.Add(line);

          //IF OUR GROUP HAS REACHED THE GROUP SIZE OR IF WE ARE ON THE LAST ELEMENT IN THE MLT
          if (tmpGroup.Lines.Count == LineGroupSize ||
              i == count - 1)
          {
            //THIS GROUP IS FINISHED.  ADD IT TO OUR _LINEGROUPS, AND RESET OUR TMP VAR
            _LineGroups.Add(tmpGroup);
            tmpGroup = null;
          }
        }
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

    #endregion

    #region Properties

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

    public int LineGroupSize { get; set; }

    private List<LineGroup> _LineGroups { get; set; }
    private List<int> _KnownGroupNumbers { get; set; }

    private int _LastGroupNumberStudied { get; set; }

    #endregion

    #region Nested Classes

    private class LineGroup : IHandle<History.Events.ReviewedLineEvent>
    {
      public LineGroup(int groupNumber, MultiLineTextEdit multiLineText)
      {
        MultiLineText = multiLineText;
        Lines = new List<LineEdit>();
        GroupNumber = groupNumber;
        _LastStudied = 0;
        History.HistoryPublisher.Ton.SubscribeToEvents(this);
      }

      public int HitCount { get; set; }
      public int MissCount { get; set; }
      public int TotalCount { get { return HitCount + MissCount; } }

      public MultiLineTextEdit MultiLineText { get; set; }
      public List<LineEdit> Lines { get; set; }
      public int GroupNumber { get; set; }
      /// <summary>
      /// Private variable used to keep track of which line this line group should be studying.
      /// </summary>
      private int _LastStudied { get; set; }
      /// <summary>
      /// Tells this LineGroup that whatever line it is on, it was reviewed correctly (known).
      /// </summary>
      public void MarkHit()
      {
        HitCount++;
        _LastStudied++;
        if (_LastStudied >= Lines.Count)
          ResetLastStudied();
        CurrentLine = null;
      }
      /// <summary>
      /// Tells this LineGroup that whatever line it is on, it was reviewed incorrectly (unknown).
      /// </summary>
      public void MarkMiss()
      {
        MissCount++;
        ResetLastStudied();
        CurrentLine = null;
      }

      private void ResetLastStudied()
      {
        _LastStudied = -1;
      }

      public LineEdit GetLine()
      {
        var toStudy = _LastStudied++;
        if (toStudy >= Lines.Count)
          toStudy = 0;
        var nextLine = Lines[toStudy];
        CurrentLine = nextLine;
        _LastStudied = toStudy;
        return nextLine;
      }

      private LineEdit CurrentLine { get; set; }

      public void Handle(History.Events.ReviewedLineEvent message)
      {
        var msgLineText = message.GetDetail<string>(History.HistoryResources.Key_LineText);
        if (CurrentLine == null ||
            CurrentLine.Phrase == null ||
            string.IsNullOrEmpty(CurrentLine.Phrase.Text) ||
            msgLineText != CurrentLine.Phrase.Text)
          return;

        var feedbackAsDouble = message.GetDetail<double>(History.HistoryResources.Key_FeedbackAsDouble);
        var feedbackIsCorrect = feedbackAsDouble >= double.Parse(StudyResources.DefaultKnowledgeThreshold);
        if (feedbackIsCorrect)
          MarkHit();
        else
          MarkMiss();
      }
    }

    #endregion
  }
}
