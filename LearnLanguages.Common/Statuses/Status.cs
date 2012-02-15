using Csla.Serialization;
using System.Collections.Generic;
using Csla.Core;

namespace LearnLanguages.Statuses
{
  /// <summary>
  /// Abstract class from which all Status objects descend. If you wish to add additional
  /// information, use the ctor with Tag of type object.  Otherwise, just use the 
  /// Status.Common dictionary.
  /// </summary>
  [Serializable]
  public abstract class Status
  {
    public Status()
    {

    }

    public Status(object tag)
    {
      Tag = tag;
    }

    static Status()
    {
      Common = new MobileDictionary<string, Status>()
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
        { CommonResources.StatusProgressed, new StatusProgressed() },
        { CommonResources.StatusErrorEncountered, new StatusErrorEncountered() }
      };

    }

    //public Status this[string key]
    //{
    //  get { return Status.Common[key]; }
    //  //set { Status.Values[key] = value; }
    //}
    public static MobileDictionary<string, Status> Common { get; private set; }

    public abstract string GetMessage();
    public static void RegisterStatus(string key, Status statusObj)
    {
      if (!Status.Common.ContainsKey(key))
        Status.Common.Add(key, statusObj);
    }

    public override string ToString()
    {
      return this.GetMessage();
    }

    public object Tag { get; private set; }
  }
}
