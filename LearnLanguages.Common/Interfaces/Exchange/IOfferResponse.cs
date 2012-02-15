using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOfferResponse
  {
    Guid Id { get; }
    Guid OfferId { get; }
    Guid OriginatorId { get; }
    object Originator { get; }
    string Response { get; }
  }
}
