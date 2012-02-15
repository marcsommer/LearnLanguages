using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOpportunity<T>
  {
    Guid Id { get; }
    IJobInfo<T> Target { get; }
    Guid OriginatorId { get; }
    object Originator { get; }
  }
}
