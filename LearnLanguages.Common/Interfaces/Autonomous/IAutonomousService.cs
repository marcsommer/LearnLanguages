namespace LearnLanguages.Common.Interfaces.Autonomous
{
  public interface IAutonomousService
  {
    /// <summary>
    /// Indicates whether or not the service is able to run.
    /// </summary>
    bool IsEnabled { get; }
    bool IsRunning { get; }

    bool Enable();
    bool Disable();

    void Run(int iterations);
    void Abort();

    IAutonomousServiceInfo Info { get; set; }
  }
}
