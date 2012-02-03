using System;
using LearnLanguages.Study.Interfaces;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The CycleStudyPartner is a simple study partner that asks what the user's native language is.
  /// It then cycles through phrases in foreign languages asking if the user knows it.  It has no memory.
  /// This is my most basic partner, as I am just forming how they work.  This isn't really a practical study
  /// partner.
  /// </summary>
  [Export(typeof(IStudyPartner))]
  public class CycleStudyPartner : IStudyPartner, IAskUserExtraData
  {
    [Import]
    public IEventAggregator _EventAggregator { get; set; }

    [Import]
    public CompositionContainer _Container { get; set; }

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
      System.Windows.MessageBox.Show("study happened.");
      if (StudyCompleted != null)
        StudyCompleted(this, EventArgs.Empty);
    }
  }
}
