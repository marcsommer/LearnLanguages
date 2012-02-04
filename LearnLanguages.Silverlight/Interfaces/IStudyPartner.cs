using System;
using System.Net;
using Caliburn.Micro;

namespace LearnLanguages.Silverlight.Interfaces
{
  public interface IStudyPartner
  {
    void Study(IEventAggregator eventAggregator);
    event EventHandler StudyCompleted;
  }
}
