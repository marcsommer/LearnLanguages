namespace LearnLanguages.Common.Interfaces.Autonomous
{
  /// <summary>
  /// This is class that exposes 
  /// </summary>
  public interface IAutonomousService : IHaveId
  {
    /// <summary>
    /// The services name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Indicates whether or not the service is able to run.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Indicates if the service is currently executing
    /// </summary>
    bool IsExecuting { get; }

    /// <summary>
    /// Turn on the service, putting it in a state to be able to execute.
    /// Returns true if enable was successful, false otherwise.
    /// </summary>
    /// <returns>true if enable was successful, false otherwise</returns>
    bool Enable();
    /// <summary>
    /// Turn off the service, putting it in a state to NO LONGER be able to execute.
    /// Returns true if enable was successful, false otherwise.
    /// </summary>
    /// <returns>true if enable was successful, false otherwise</returns>
    bool Disable();

    /// <summary>
    /// Executes a service recommendedIterations times.
    /// </summary>
    /// <param name="recommendedIterations">number of iterations recommended to execute, based on past performance</param>
    /// <param name="maxExecutionTimeInMs">timeout in ms for execute method</param>
    void Execute(int recommendedIterations, int maxExecutionTimeInMs);

    /// <summary>
    /// Number of iterations that this service has completed during
    /// this execution cycle.
    /// </summary>
    int NumIterationsCompletedThisExecution { get; }

    /// <summary>
    /// Number of iterations that this service has completed during
    /// this service's lifespan (since its constructor was called).
    /// </summary>
    int NumIterationsCompletedThisLifetime { get; }

    /// <summary>
    /// Aborts the service's execution within the given timeAllowed. 
    /// This is NOT an async timeout. This value is how long the service is being 
    /// given before that service's executing thread is no longer guaranteed. After
    /// that time, the thread may be stopped dead, and the service will be 
    /// marked as an untrustworthy service. 
    /// </summary>
    void Abort(int timeAllowed);

  }
}
