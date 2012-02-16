using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IWorkStatusUpdate : IExchangeMessage
  {
    Statuses.Status Status { get; }
  }
}
