using System;
using System.Net;
using Caliburn.Micro;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudyPartner
  {
    void Study(IEventAggregator eventAggregator);
    event EventHandler StudyCompleted;
  }
}
