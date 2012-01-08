
namespace LearnLanguages.Business.Bases
{
  public abstract class ReadOnlyListBase<T, C, TDto> :
    Csla.ReadOnlyListBase<T, C>
    where T : Bases.ReadOnlyListBase<T, C, TDto>
    where C : Bases.BusinessBase<C, TDto>
    where TDto : class
  {

  }
}
