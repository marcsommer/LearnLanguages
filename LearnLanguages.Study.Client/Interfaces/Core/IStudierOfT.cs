using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudier<T> 
    where T : class
  {
    void Study(T studyTarget, IOfferExchange offerExchange);
  }
}
