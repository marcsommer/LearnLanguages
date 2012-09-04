using System;
using System.Linq;
using System.Threading;

using LearnLanguages.Common;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Business;
using Caliburn.Micro;
using Csla.Core;
using LearnLanguages.History;
using System.Collections.Generic;

namespace LearnLanguages.Study
{
  /// <summary>
  /// This advisor expects a question StudyResources.AdvisorQuestionWhatIsPhrasePercentKnown with 
  /// State == PhraseEdit, answer is type double between 0.0 and 1.0.
  /// </summary>
  public class DefaultPhrasePercentKnownAdvisor : IAdvisor, 
                                                  //IHandle<History.Events.ReviewedLineEvent>,
                                                  IHandle<History.Events.ReviewedPhraseEvent>
  {
    #region Singleton Pattern Members
    private static volatile DefaultPhrasePercentKnownAdvisor _Ton;
    private static object _Lock = new object();
    public static DefaultPhrasePercentKnownAdvisor Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new DefaultPhrasePercentKnownAdvisor();
          }
        }

        return _Ton;
      }
    }
    #endregion

    #region Ctors and Init

    public DefaultPhrasePercentKnownAdvisor()
    {
      Cache = new MobileDictionary<PhraseTextLanguageTextPair, double>();
      History.HistoryPublisher.Ton.SubscribeToEvents(this);
    }

    #endregion

    #region Properties

    public Guid Id { get { return Guid.Parse(StudyResources.AdvisorIdPhrasePercentKnownStudyAdvisor); } }

    private static AutoResetEvent _AutoResetEvent = new AutoResetEvent(false);
    private static Dictionary<Guid, AutoResetEvent> _AutoResetEvents = new Dictionary<Guid, AutoResetEvent>();
    private PhraseEdit _CurrentWordPhrase { get; set; }
    private int _CountWordsKnown { get; set; }

    private class PhraseTextLanguageTextPair
    {
      public PhraseTextLanguageTextPair(string phraseText, string languageText)
      {
        PhraseText = phraseText;
        LanguageText = languageText;
      }

      public string PhraseText { get; set; }
      public string LanguageText { get; set; }
    }
    private MobileDictionary<PhraseTextLanguageTextPair, double> Cache { get; set; }

    #endregion

    #region Methods

    public bool AskAdvice(QuestionArgs questionArgs, AsyncCallback<object> answerCallback)
    {
      if (questionArgs == null)
        throw new ArgumentNullException("questionArgs");
      if (answerCallback == null)
        throw new ArgumentNullException("answerQuestion");

      //WE ONLY KNOW HOW TO DEAL WITH PHRASE EDIT QUESTIONS ABOUT PERCENT KNOWN.
      if (!(questionArgs.State is PhraseEdit) ||
            questionArgs.Question != StudyResources.AdvisorQuestionWhatIsPhrasePercentKnown)
        return false;

      var phrase = (PhraseEdit)questionArgs.State;

      GetPercentKnown(phrase, (s, r) =>
      {
        if (r.Error != null)
          answerCallback(this, new ResultArgs<object>(r.Error));

        answerCallback(this, new ResultArgs<object>(r.Object));
      });


      return true;
    }

    private void GetPercentKnownAboutPhraseWithBeliefs(PhraseEdit phrase,
                                                       PhraseBeliefList beliefs,
                                                       AsyncCallback<double> callback)
    {
      try
      {
        if (beliefs.Count < 1)
          throw new ArgumentException("beliefs.Count");

        ///FOR NOW, THIS JUST GETS THE MOST RECENT BELIEF AND GOES WITH THE STRENGTH OF THAT BELIEF.
        ///THIS IS ABSOLUTELY NOT HOW IT SHOULD BE BUT THAT IS WHY WE HAVE A "DEFAULT" ADVISOR AND
        ///EXTENSIBILITY FOR THIS.

        var results = from belief in beliefs
                      orderby belief.TimeStamp
                      select belief;

        var mostRecentBelief = results.First();
        var percentKnown = mostRecentBelief.Strength;
        callback(this, new ResultArgs<double>(percentKnown));
        return;
      }
      catch (Exception ex)
      {
        callback(this, new ResultArgs<double>(ex));
        return;
      }
    }

    //public void Handle(History.Events.ReviewedLineEvent message)
    //{
    //  //THIS MESSAGE IS GOING TO BE RECORDED BY A DIFFERENT RECORDER OBJECT, BUT 
    //  //WE WANT TO PUT IT INTO OUR CACHE FOR QUICK ADVISING

    //  //LINETEXT
    //  string lineText = "";
    //  var messageHasLineText = message.TryGetDetail<string>(History.HistoryResources.Key_LineText, out lineText);
    //  if (!messageHasLineText)
    //    return;

    //  //LANGUAGE TEXT
    //  string languageText = "";
    //  var messageHasLanguageText =
    //    message.TryGetDetail<string>(History.HistoryResources.Key_LanguageText, out languageText);
    //  if (!messageHasLanguageText)
    //    return;

    //  //FEEDBACK
    //  double feedbackAsDouble = -1;
    //  var messageHasFeedbackAsDouble =
    //    message.TryGetDetail<double>(History.HistoryResources.Key_FeedbackAsDouble, out feedbackAsDouble);
    //  if (!messageHasFeedbackAsDouble)
    //    return;

    //  //ADD/REPLACE ENTRY
    //  var keyPhraseInfo = new PhraseTextLanguageTextPair(lineText, languageText);
    //  var containsLinePhrase = Cache.ContainsKey(keyPhraseInfo);
    //  if (!containsLinePhrase)
    //    Cache.Add(keyPhraseInfo, feedbackAsDouble);
    //  else
    //    Cache[keyPhraseInfo] = feedbackAsDouble;
    //}

    public void Handle(History.Events.ReviewedPhraseEvent message)
    {
      var phraseText = message.GetDetail<string>(HistoryResources.Key_PhraseText);
      var languageText = message.GetDetail<string>(HistoryResources.Key_LanguageText);
      var feedbackAsDouble = message.GetDetail<double>(HistoryResources.Key_FeedbackAsDouble);

      var results = from entry in Cache
                    where entry.Key.PhraseText == phraseText &&
                          entry.Key.LanguageText == languageText
                    select entry;

      if (results.Count() == 1)
      {
        //WE ARE REPLACING EXISTING PHRASE/LANGUAGE ENTRY'S FEEDBACK WITH MOST RECENT SCORE
        var entryKey = results.First().Key;
        Cache[entryKey] = feedbackAsDouble;
      }
      else
      {
        //WE ARE ADDING NEW PHRASE/LANGUAGE ENTRY WITH FEEDBACK SCORE
        var key = new PhraseTextLanguageTextPair(phraseText, languageText);
        Cache.Add(key, feedbackAsDouble);
      }
    }

    private void GetPercentKnown(PhraseEdit phrase, AsyncCallback<double> callback)
    {
      #region CHECK CACHE

      var results = from entry in Cache
                    where entry.Key.PhraseText == phrase.Text &&
                          entry.Key.LanguageText == phrase.Language.Text
                    select entry;

      if (results.Count() == 1)
      {
        #region GIVE ANSWER FROM CACHE

        var entry = results.First();

        var percentKnown = entry.Value;
        callback(this, new ResultArgs<double>(percentKnown));
        return;

        #endregion
      }

      #endregion

      #region CHECK BELIEFS IN DB

      PhraseBeliefList.GetBeliefsAboutPhrase(phrase.Id, (s, r) =>
      {
        if (r.Error != null)
        {
          callback(this, new ResultArgs<double>(r.Error));
          return;
        }

        var beliefs = r.Object;

        if (beliefs.Count <= 0)
        {
          //WE HAVE NO BELIEFS ABOUT THIS PHRASE
          #region GetPercentKnownAboutPhraseWithNoPriorBeliefs(phrase, (s2, r2) =>

          GetPercentKnownAboutPhraseWithNoPriorBeliefs(phrase, (s2, r2) =>
          {
            if (r2.Error != null)
            {
              callback(this, new ResultArgs<double>(r2.Error));
              return;
            }

            var percentKnown = r2.Object;
            callback(this, new ResultArgs<double>(percentKnown));
            return;
          });

          #endregion
        }
        else
        {
          //WE HAVE BELIEFS ABOUT THIS PHRASE
          #region GetPercentKnownAboutPhraseWithBeliefs(phrase, beliefs, (s3, r3) =>
          GetPercentKnownAboutPhraseWithBeliefs(phrase, beliefs, (s3, r3) =>
          {
            if (r3.Error != null)
            {
              callback(this, new ResultArgs<double>(r3.Error));
              return;
            }

            var percentKnown = r3.Object;
            callback(this, new ResultArgs<double>(percentKnown));
            return;
          });
          #endregion
        }
      });

      #endregion
    }

    private void GetPercentKnownAboutPhraseWithNoPriorBeliefs(PhraseEdit phrase, AsyncCallback<double> callback)
    {
      //FOR RIGHT NOW, THIS IS JUST GOING TO RETURN ZERO. IT SUCKS BUT OH WELL.
      //TODO: IMPLEMENT GETPERCENTKNOWNABOUTPHRASEWITHNOPRIORBELIEFS
      double percentKnown = 0;
      callback(this, new ResultArgs<double>(percentKnown));
      return;



//      //WE HAVE NO PRIOR BELIEFS ABOUT THIS PHRASE, SO WE WILL CHECK THE SUM OF THE BELIEFS OF THE INDIVIDUAL WORDS
//      var words = phrase.Text.ParseIntoWords();

//      #region ONLY ONE WORD IN PHRASE (RETURN WITH ZERO PERCENT KNOWN)
//      //IF WE ONLY HAVE A SINGLE WORD IN OUR PHRASE, THEN WE HAVE ALREADY CHECKED THIS SINGLE WORD FOR BELIEF 
//      //AND CAME UP EMPTY.  SO WE HAVE NO PERCENT KNOWN ABOUT THIS PHRASE.
//      if (words.Count < 2)
//      {
//        percentKnown = 0;
//        callback(this, new ResultArgs<double>(percentKnown));
//        return;
//      }
//      #endregion

//      #region MULTIPLE WORDS IN PHRASE (SUM PERCENT KNOWNS OF EACH WORD)

//      #region FIRST, GET THE PHRASES FOR EACH OF THE INDIVIDUAL WORDS
//      var phraseTextsCriteria = new Business.Criteria.PhraseTextsCriteria(phrase.Language.Text, words);
//      PhraseList.NewPhraseList(phraseTextsCriteria, (s, r) =>
//      {
//        //TODO: CHECK TO SEE WHAT HAPPENS IF WE DELETE A PHRASE THAT IS AN INDIVIDUAL WORD AND WE DO A FUNCTION (LIKE PERCENT KNOWN ABOUT PHRASE WITH NO PRIOR BELIEFS) THAT ASSUMES ALL INDIVIDUAL WORDS EXIST IN DATABASE.

//        if (r.Error != null)
//        {
//          callback(this, new ResultArgs<double>(r.Error));
//          return;
//        }

//        var wordPhrases = r.Object;

//        #region SECOND, CALL THIS ADVISOR'S GETPERCENTKNOWN RECURSIVELY FOR EACH INDIVIDUAL WORD'S PERCENT KNOWN

//        #region DECLARE ACTION THAT WILL USE WAITONE IN UPCOMING ASYNC FOR LOOP
//        Action<object> getPercentKnownWord = (object state) =>
//        {
//          //AutoResetEvent autoResetEvent = (AutoResetEvent)state;
//          Guid id = (Guid)state;
//          //IF THE ID IS NOT THERE, THEN THIS AUTORESETEVENT HAS ALREADY BEEN REMOVED FROM THE LIST
//          //IE, IT HAS TIMED OUT.
//          if (!_AutoResetEvents.ContainsKey(id))
//            return;

//          var autoResetEvent = _AutoResetEvents[(Guid)state];
////#if DEBUG
////          if (_CurrentWordPhrase.Text.Contains("Dans"))
////            System.Diagnostics.Debugger.Break();
////#endif 

//          #region PhrasePercentKnownAdvisor.Ton.GetPercentKnown(_CurrentWordPhrase, (s2, r2) =>
//          DefaultPhrasePercentKnownAdvisor.Ton.GetPercentKnown(_CurrentWordPhrase, (s2, r2) =>
//          {
//            if (r2.Error != null)
//            {
//              callback(this, new ResultArgs<double>(r2.Error));
//              return;
//            }

//            //WE ASSUME A SINGLE WORD IS EITHER KNOWN OR UNKNOWN.
//            if (r2.Object > 0)
//              _CountWordsKnown++;

//            autoResetEvent.Set();
//          });
//          #endregion
//        };

//        #endregion

//        #region EXECUTE ASYNC FOR LOOP EXECUTING ABOVE ACTION
//        _CountWordsKnown = 0;
//        var localAutoResetEvent = new AutoResetEvent(false);
//        var areID = Guid.NewGuid();
//        _AutoResetEvents.Add(areID, localAutoResetEvent);
//        foreach (var wordPhrase in wordPhrases)
//        {
//          _CurrentWordPhrase = wordPhrase;
//          int dummyWorker = -1;
//          int dummyCompletionPort = -1;
//          ThreadPool.GetMaxThreads(out dummyWorker, out dummyCompletionPort);
//          ThreadPool.QueueUserWorkItem(new WaitCallback(getPercentKnownWord), areID);

//          //ThreadPool.QueueUserWorkItem(new WaitCallback(getPercentKnownWord), _AutoResetEvent);
//          localAutoResetEvent.WaitOne(500);

//        }
//        #endregion

//        #region FINALLY, GET THE PERCENT KNOWN = COUNT WORDS KNOWN / COUNT ALL WORDS (AND RETURN)
//        var countAllWords = wordPhrases.Count;
//        percentKnown = ((double)_CountWordsKnown) / ((double)countAllWords);

//        callback(this, new ResultArgs<double>(percentKnown));
//        return;
//        #endregion

//        #endregion
//      });

//      #endregion

//      #endregion
    }


    #endregion

    #region Events

    #endregion

    
  }
}
