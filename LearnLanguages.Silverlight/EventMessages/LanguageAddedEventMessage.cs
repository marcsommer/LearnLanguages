namespace LearnLanguages.Silverlight.EventMessages
{
  public class LanguageAddedEventMessage : LanguageEventMessage
  {
    public LanguageAddedEventMessage(string newLanguageText)
    {
    }

    public string NewLanguageText { get; private set; }
  }
}
