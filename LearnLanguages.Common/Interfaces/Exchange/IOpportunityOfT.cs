using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOpportunity<T> : IOpportunity
  {
    IJobInfo<T> JobInfo { get; }
  }
}
