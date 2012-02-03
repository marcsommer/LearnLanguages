using System;
using LearnLanguages.Study.Interfaces;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace LearnLanguages.Study
{
  /// <summary>
  /// Simple IStudyPartner base class.  You do NOT need to descend from this, it just provides some 
  /// very basic plumbing for events and checking for 
  /// </summary>
  public abstract class StudyPartnerBase : IStudyPartner, IAskUserExtraData
  {
    #region Properties 

    [Import]
    public IEventAggregator EventAggregator { get; set; }
    [Import]
    public CompositionContainer Container { get; set; }

    #endregion

    #region Methods

    public virtual void Study(IEventAggregator eventAggregator)
    {
      try
      {
        //INITIALIZE
        //CONTAINER
        if (Container == null)
        {
          Container = Services.Container;
          if (Container == null)
            throw new Common.Exceptions.PartNotSatisfiedException("Container");
        }

        //WE NEED EVENT AGGREGATOR FOR PUBLISHING NAVIGATION
        if (EventAggregator == null)
        {
          Container.SatisfyImportsOnce(this);

          //EVENT AGGREGATOR STILL NULL
          if (EventAggregator == null)
            throw new TypeLoadException("EventAggregator could not be satisfied");
        }

        StudyImpl();
      }
      finally
      {
        if (StudyCompleted != null)
          StudyCompleted(this, EventArgs.Empty);
      }
    }
    protected abstract void StudyImpl();

    public virtual void AskUserExtraData()
    {
      try
      {
        AskUserExtraDataImpl();
      }
      finally
      {
        if (AskUserExtraDataCompleted != null)
          AskUserExtraDataCompleted(this, EventArgs.Empty);
      }
    }
    protected abstract void AskUserExtraDataImpl();

    #endregion

    #region Events

    public event EventHandler StudyCompleted;
    public event EventHandler AskUserExtraDataCompleted;

    #endregion
  }
}
