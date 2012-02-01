namespace LearnLanguages.Silverlight.EventMessages
{
  public class PartSatisfiedEventMessage 
  {
    public PartSatisfiedEventMessage(string part)
    {
      Part = part;
    }
    public string Part { get; private set; }
  }
}
