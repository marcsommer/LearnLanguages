
namespace LearnLanguages.Business.Bases
{
  public abstract class BusinessListBase<T, C, TDto> :
    Csla.BusinessListBase<T, C>
    where T : Bases.BusinessListBase<T, C, TDto>
    where C : Bases.BusinessBase<C, TDto>
    where TDto : class
  {

  }
}
