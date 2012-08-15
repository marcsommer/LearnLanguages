using System;
using LearnLanguages.History;
using LearnLanguages.Business;
using LearnLanguages.History.Events;
using LearnLanguages.History.Bases;

namespace LearnLanguages.Study
{
  /// <summary>
  /// Not a "Recorder" strictly speaking, but rather a listener that listens for PhraseAutoTranslatedEvent's.
  /// If it hears this event and this ShouldRecord (enabled), then when a phrase is automatically translated
  /// using a service such as Microsoft Translator (Bing), then it automatically saves this to DB, which almost
  /// acts as a caching function until the user overrides the translation with a custom translation.
  /// </summary>
  public class DefaultPhraseAutoTranslatedRecorder : HistoryRecorderBase<History.Events.PhraseAutoTranslatedEvent>
  {
    public DefaultPhraseAutoTranslatedRecorder()
    {
      Id = Guid.Parse(StudyResources.DefaultPhraseAutoTranslatedRecorderId);
    }

    /// <summary>
    /// Always returns true.
    /// Since this is not really a recorder, it doesn't do anything special like filtering events.
    /// Use the IsEnabled property to enable/disable this object.
    /// </summary>
    protected override bool ShouldRecord(History.Events.PhraseAutoTranslatedEvent message)
    {
      return true;
    }

    /// <summary>
    /// Saves the translation if one does not already exist in DB.
    /// </summary>
    /// <param name="message"></param>
    protected override void Record(History.Events.PhraseAutoTranslatedEvent message)
    {
      //first, we need to make sure we don't already have this translation pair (source phrase, translation phrase)
      //in our database.
      var criteria = 
        new Business.Criteria.TranslationSearchCriteria(message.SourcePhrase, 
                                                        message.TranslatedPhrase.Language.Text);
      Business.TranslationSearchRetriever.CreateNew(criteria, (s, r) =>
        {
          if (r.Error != null)
          {
            throw r.Error;
          }

          if (r.Object.Translation != null)
          {
            //we already have a translation for this, so we don't need to do anything further.
            return;
          }
          

          //NO PRIOR TRANSLATION EXISTS, SO CREATE AND SAVE.

          //CREATE
          var createTranslationCriteria = 
            new Business.Criteria.ListOfPhrasesCriteria(message.SourcePhrase, message.TranslatedPhrase);
          TranslationCreator.CreateNew(createTranslationCriteria, (s2, r2) =>
            {
              if (r2.Error != null)
              {
                throw r2.Error;
              }

              //SAVE
              r2.Object.Translation.BeginSave((s3, r3) =>
                {
                  if (r3.Error != null)
                    throw r3.Error;
                });
            });
        });
    }
  }
}
