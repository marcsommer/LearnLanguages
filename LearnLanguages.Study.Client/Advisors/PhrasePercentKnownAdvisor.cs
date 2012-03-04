using System;
using System.Linq;

using LearnLanguages.Common;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Business;
using Caliburn.Micro;
using Csla.Core;
using System.Threading;

namespace LearnLanguages.Study.Advisors
{
  /// <summary>
  /// This advisor expects a question StudyResources.AdvisorQuestionWhatIsPhrasePercentKnown with 
  /// State == PhraseEdit, answer is type double between 0.0 and 1.0.
  /// </summary>
  public class PhrasePercentKnownAdvisor : IAdvisor, IHandle<History.Events.ReviewedLineEvent>
  {
    public PhrasePercentKnownAdvisor()
    {
      Cache = new MobileDictionary<PhraseEdit, double>();
    }

    #region Singleton Pattern Members
    private static volatile PhrasePercentKnownAdvisor _Ton;
    private static object _Lock = new object();
    public static PhrasePercentKnownAdvisor Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new PhrasePercentKnownAdvisor();
          }
        }

        return _Ton;
      }
    }
    #endregion

    public Guid Id { get { return Guid.Parse(StudyResources.AdvisorIdPhrasePercentKnownStudyAdvisor); } }
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

    private void GetPercentKnown(PhraseEdit phrase, AsyncCallback<double> callback)
    {
      ///TODO: COME UP WITH ANSWER
      ///1) First, check cache.  This will be the most up-to-date that we care about.
      ///2) Get all history events related to this phrase.  This includes:
      ///   -if this phrase exists in db, check for beliefs about phrase.  If have any, then use those.  otherwise,
      ///   -check for a phrase that contains this phrase
      ///   -composition of individual words
      ///2) Analyze history events to project how much we think the user knows for this phrase.
      
      #region CHECK CACHE

      var results = from entry in Cache
                    where entry.Key.Text == phrase.Text &&
                          entry.Key.Language.Text == phrase.Language.Text
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
      
      #region CHECK FOR ANY BELIEFS ABOUT PHRASE.

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
      double percentKnown = 0;

      //WE HAVE NO PRIOR BELIEFS ABOUT THIS PHRASE, SO WE WILL CHECK THE SUM OF THE BELIEFS OF THE INDIVIDUAL WORDS
      var words = phrase.Text.ParseIntoWords();

      #region ONLY ONE WORD IN PHRASE (RETURN WITH ZERO PERCENT KNOWN)
      //IF WE ONLY HAVE A SINGLE WORD IN OUR PHRASE, THEN WE HAVE ALREADY CHECKED THIS SINGLE WORD FOR BELIEF 
      //AND CAME UP EMPTY.  SO WE HAVE NO PERCENT KNOWN ABOUT THIS PHRASE.
      if (words.Count < 2)
      {
        percentKnown = 0;
        callback(this, new ResultArgs<double>(percentKnown));
        return;
      }
      #endregion

      #region MULTIPLE WORDS IN PHRASE (SUM PERCENT KNOWNS OF EACH WORD)

      #region FIRST, GET THE PHRASES FOR EACH OF THE INDIVIDUAL WORDS
      var phraseTextsCriteria = new Business.Criteria.PhraseTextsCriteria(phrase.Language.Text, words);
      PhraseList.NewPhraseList(phraseTextsCriteria, (s, r) =>
        {
          //TODO: CHECK TO SEE WHAT HAPPENS IF WE DELETE A PHRASE THAT IS AN INDIVIDUAL WORD AND WE DO A FUNCTION (LIKE PERCENT KNOWN ABOUT PHRASE WITH NO PRIOR BELIEFS) THAT ASSUMES ALL INDIVIDUAL WORDS EXIST IN DATABASE.

          if (r.Error != null)
          {
            callback(this, new ResultArgs<double>(r.Error));
            return;
          }

          var wordPhrases = r.Object;

          #region SECOND, CALL THIS ADVISOR'S GETPERCENTKNOWN RECURSIVELY FOR EACH INDIVIDUAL WORD'S PERCENT KNOWN

          #region DECLARE ACTION THAT WILL USE WAITONE IN UPCOMING ASYNC FOR LOOP
          Action<object> getPercentKnownWord = (object state) =>
            {
              AutoResetEvent autoResetEvent = (AutoResetEvent)state;
            
              #region PhrasePercentKnownAdvisor.Ton.GetPercentKnown(_CurrentWordPhrase, (s2, r2) =>
              PhrasePercentKnownAdvisor.Ton.GetPercentKnown(_CurrentWordPhrase, (s2, r2) =>
                {
                  if (r2.Error != null)
                  {
                    callback(this, new ResultArgs<double>(r2.Error));
                    return;
                  }

                  //WE ASSUME A SINGLE WORD IS EITHER KNOWN OR UNKNOWN.
                  if (r2.Object > 0)
                    _CountWordsKnown++;

                  autoResetEvent.Set();
                });
              #endregion
            };

          #endregion

          #region EXECUTE ASYNC FOR LOOP EXECUTING ABOVE ACTION
          _CountWordsKnown = 0;
          foreach (var wordPhrase in wordPhrases)
          {
            _CurrentWordPhrase = wordPhrase;
            ThreadPool.QueueUserWorkItem(new WaitCallback(getPercentKnownWord), _AutoResetEvent);
            _AutoResetEvent.WaitOne();
          }
          #endregion

          #region FINALLY, GET THE PERCENT KNOWN = COUNT WORDS KNOWN / COUNT ALL WORDS (AND RETURN)
          var countAllWords = wordPhrases.Count;
          percentKnown = ((double)_CountWordsKnown) / ((double)countAllWords);

          callback(this, new ResultArgs<double>(percentKnown));
          return;
          #endregion

          #endregion
        });

      #endregion

      #endregion
    }

    private static AutoResetEvent _AutoResetEvent = new AutoResetEvent(false);
    private PhraseEdit _CurrentWordPhrase { get; set; }
    private int _CountWordsKnown { get; set; }


    private void GetPercentKnownAboutPhraseWithBeliefs(PhraseEdit phrase, PhraseBeliefList beliefs, AsyncCallback<double> callback)
    {
      throw new NotImplementedException();
    }

    public void Handle(History.Events.ReviewedLineEvent message)
    {
      throw new NotImplementedException();
    }

    private MobileDictionary<PhraseEdit, double> Cache { get; set; }
  }
}
