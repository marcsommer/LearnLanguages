using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IWorkStatusUpdate
  {
    Guid JobId { get; }
    Statuses.Status Status { get; }
  }
}
