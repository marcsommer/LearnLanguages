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

    #region Methods

    public void Study(CompositionContainer container)
    {
      //WE NEED EVENT AGGREGATOR FOR PUBLISHING NAVIGATION
      if (_EventAggregator == null)
      {
        if (container == null)
          throw new ArgumentNullException("container");

        container.SatisfyImportsOnce(this);

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
