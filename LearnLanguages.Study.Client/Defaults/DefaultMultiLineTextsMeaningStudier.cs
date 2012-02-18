using System;
using System.Linq;
using LearnLanguages.Business;
using System.ComponentModel.Composition;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using Caliburn.Micro;
using LearnLanguages.Offer;

namespace LearnLanguages.Study
{
  /// <summary>
  /// This MeaningStudier manages a collection of SingleMultiLineTextMeaningStudiers.  Currently, this
  /// cycles through each of the MLTs and calls its Study method.  In the future, this is where we will
  /// adjust priorities to different MLTs, depending on priority of user, and how strongly we want to study 
  /// a certain MLT, due to its projected 'memory trace health' or whatever the paradigm we are using.
  /// </summary>
  public class DefaultMultiLineTextsMeaningStudier :
    StudierBase<StudyJobInfo<MultiLineTextList, IViewModelBase>, MultiLineTextList, IViewModelBase>,
    IHandle<IStatusUpdate<MultiLineTextEdit, IViewModelBase>>
  {
    public DefaultMultiLineTextsMeaningStudier()
    {
      _Studiers = new Dictionary<Guid, DefaultSingleMultiLineTextMeaningStudier>();
      Exchange.Ton.SubscribeToStatusUpdates(this);
    }

    private int _CurrentMultiLineTextIndex = -1;

    /// <summary>
    /// Collection of Studiers, indexed by the MLT that they study.
    /// </summary>
    private Dictionary<Guid, DefaultSingleMultiLineTextMeaningStudier> _Studiers { get; set; }

    #region Methods

    protected override void DoImpl()
    {
      //I KEEP FORGETTING...EACH OF THESE STUDIERS USE CREATED FOR ONE TARGET ONLY, SO
      //THERE IS NO CHANCE OF DO-ING MULTIPLE TARGETS.  THE PATTERN IS:  CREATE STUDIER,
      //CALL DO, THROW AWAY WHEN DONE.  IT NEVER REUSES STUDIERS.
      //SO...
      //WE DON'T NEED FIRST TO DETERMINE IF WE HAVE DONE THE CURRENT JOB'S MULTILINETEXTS BEFORE.
      //IF SO, THEN WE DEDUCE THIS IS A CONTINUATION OF STUDYING THEM.
      //if (!IsContinuationOfPreviousStudyTarget())
      if (!IsNotFirstRun)
        InitializeForThisJob();
      var multiLineTextIndex = GetNextMultiLineTextIndex();
      var currentMultiLineText = _StudyJobInfo.Target[multiLineTextIndex];
      var studier = _Studiers[currentMultiLineText.Id];

      //OUR CURRENT JOB ENTAILS ALL MULTILINETEXTEDITS, SO WE NEED TO CREATE 
      //A NEW JOB WITH ONLY THE ONE IN PARTICULAR WE CARE ABOUT.
      var originalJob = _StudyJobInfo;
      var newExpirationDate = 
        System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime;

      var studyJobCriteria = (StudyJobCriteria)originalJob.Criteria;
      var newJob = new StudyJobInfo<MultiLineTextEdit, IViewModelBase>(currentMultiLineText, 
                                                       studyJobCriteria.Language, 
                                                       newExpirationDate,
                                                       studyJobCriteria.ExpectedPrecision);
      //newJob.OriginalJobId = originalJob.Id;

      //NOTICE THAT THIS IS **NOT** PUBLISHED ON THE EXCHANGE.  YES IT IS A JOB INFO,
      //NO IT DOES NOT HAVE TO BE PUBLISHED.
      studier.Do(newJob);
    }

    private void InitializeForThisJob()
    {
      _Studiers.Clear();

 	    //CREATE STUDIER FOR EACH MULTILINETEXT IN MULTILINETEXTS TARGET
      var newJobMultiLineTextList = _StudyJobInfo.Target;
      foreach (var multiLineTextEdit in newJobMultiLineTextList)
      {
        var studier = new DefaultSingleMultiLineTextMeaningStudier();
        _Studiers.Add(multiLineTextEdit.Id, studier);
      }
    }

    private int GetNextMultiLineTextIndex()
    {
      //cycle through the , and get the corresponding studier for this MLT.
      _CurrentMultiLineTextIndex++;
      if (_CurrentMultiLineTextIndex > (_StudyJobInfo.Target.Count - 1))
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
        var results = from mlt in _StudyJobInfo.Target
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

    public void Handle(IStatusUpdate<MultiLineTextEdit, IViewModelBase> message)
    {
      //WE ONLY CARE ABOUT STUDY MESSAGES
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //WE ONLY CARE ABOUT MESSAGES PUBLISHED BY SINGLE MLT MEANING STUDIERS
      if (
           (message.Publisher != null) &&
           !(message.Publisher is DefaultSingleMultiLineTextMeaningStudier)
         )
        return;

      ////WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
      //if (message.PublisherId == Id)
      //  return;

      //THIS IS ONE OF THIS OBJECT'S UPDATES, SO BUBBLE IT BACK UP WITH THIS JOB'S INFO

      //IF THIS IS A COMPLETED STATUS UPDATE, THEN PRODUCT SHOULD BE SET.  SO, BUBBLE THIS ASPECT UP.
      if (message.Status == CommonResources.StatusCompleted && message.JobInfo.Product != null)
      {
        _StudyJobInfo.Product = message.JobInfo.Product;
      }

      //CREATE THE BUBBLING UP UPDATE
      var statusUpdate = new StatusUpdate<MultiLineTextList, IViewModelBase>(message.Status, null, null,
        null, _StudyJobInfo, Id, this, StudyResources.CategoryStudy, null);

      //PUBLISH TO BUBBLE UP
      Exchange.Ton.Publish(statusUpdate);
    }
  }
}
