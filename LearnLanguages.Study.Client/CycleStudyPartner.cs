using System;
using LearnLanguages.Study.Interfaces;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace LearnLanguages.Study
{
  [Export(typeof(IStudyPartner))]
  public class CycleStudyPartner : IStudyPartner, IAskUserExtraData
  {

    [Import]
    private IEventAggregator _EventAggregator { get; set; }

    [Import]
    private CompositionContainer _Container { get; set; }

    #region Methods

    public void Study()
    {
      //CONTAINER
      if (_Container == null)
      {
        _Container = Services.Container;
        if (_Container == null)
          throw new Common.Exceptions.PartNotSatisfiedException("Container");
      }

      //WE NEED EVENT AGGREGATOR FOR PUBLISHING NAVIGATION
      if (_EventAggregator == null)
      {
        _Container.SatisfyImportsOnce(this);

        //EVENT AGGREGATOR STILL NULL
        if (_EventAggregator == null)
          throw new TypeLoadException("EventAggregator could not be satisfied");
      }
    }
    
    public void AskUserExtraData()
    {
      throw new NotImplementedException();
    }

    #endregion

    #region Events

    public event EventHandler StudyCompleted;
    public event EventHandler AskUserExtraDataCompleted;

    #endregion

    public void Study(IEventAggregator eventAggregator)
    {
      throw new NotImplementedException();
    }
  }
}
