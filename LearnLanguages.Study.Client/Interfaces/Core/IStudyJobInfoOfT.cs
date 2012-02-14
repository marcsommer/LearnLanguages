using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using System;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudyJobInfo<T> 
  {
    Guid Id { get; }
    T StudyTarget { get; }
    DateTime JobExpirationDate { get; }
    string GetStudyContext();
  }
}
