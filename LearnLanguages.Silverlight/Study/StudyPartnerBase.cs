using System;
using LearnLanguages.Silverlight.Interfaces;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Silverlight
{
  /// <summary>
  /// Simple IStudyPartner base class.  You do NOT need to descend from this, it just provides some 
  /// very basic plumbing for events and checking for 
  /// </summary>
  public abstract class StudyPartnerBase : PropertyChangedBase, IStudyPartner, IGetStudyData
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
    protected abstract void StudyImpl();
    
    #endregion

    #region Events

    public event EventHandler StudyCompleted;

    #endregion

    public void AskUserExtraData(object criteria, AsyncCallback<StudyDataEdit> callback)
    {
      AskUserExtraDataImpl(criteria, callback);
    }

    protected abstract void AskUserExtraDataImpl(object criteria, AsyncCallback<StudyDataEdit> callback);

    public void StudyMultiLineTexts(MultiLineTextList multiLineTexts, IEventAggregator eventAggregator)
    {
      throw new NotImplementedException();
    }
  }
}
