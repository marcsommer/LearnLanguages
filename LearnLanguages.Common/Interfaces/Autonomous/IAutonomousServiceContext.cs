namespace LearnLanguages.Common.Interfaces.Autonomous
{
  public interface IAutonomousServiceContext
  {
    bool IsExecuting { get; }
    bool IsLoaded { get; }
    void Load(IAutonomousService service);
    int MinExecutionTime { get; set; }
    int MaxExecutionTime { get; set; }
  }
}
