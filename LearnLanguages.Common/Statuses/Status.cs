using System.Collections.Generic;

namespace LearnLanguages.Statuses
{
  public abstract class Status
  {
    static Status()
    {
      Common = new Dictionary<string, Status>()
      {
        { CommonResources.StatusAdding, new StatusAdding() },
        { CommonResources.StatusAdded, new StatusAdded() },
        { CommonResources.StatusCanceled, new StatusCanceled() },
        { CommonResources.StatusCanceling, new StatusCanceling() },
        { CommonResources.StatusCompleted, new StatusCompleted() },
        { CommonResources.StatusDeleting, new StatusDeleting() },
        { CommonResources.StatusDeleted, new StatusDeleted() },
        { CommonResources.StatusInitialized, new StatusInitialized() },
        { CommonResources.StatusStarted, new StatusStarted() },
        { CommonResources.StatusUnspecified, new StatusUnspecified() },
        { CommonResources.StatusUpdating, new StatusUpdating() },
        { CommonResources.StatusUpdated, new StatusUpdated() },
      };
    }

    //public Status this[string key]
    //{
    //  get { return Status.Common[key]; }
    //  //set { Status.Values[key] = value; }
    //}
    public static Dictionary<string, Status> Common { get; private set; }

    public abstract string Value();
    public void RegisterStatus(string key, Status statusObj)
    {
      if (!Common.ContainsKey(key))
        Common.Add(key, statusObj);
    }

    public override string ToString()
    {
      return this.Value();
    }

  }
}
