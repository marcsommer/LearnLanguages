
namespace LearnLanguages.Business.Bases
{
  public abstract class ReadOnlyListBase<L, C, CDto> :
    Csla.ReadOnlyListBase<L, C>
    where L : Bases.ReadOnlyListBase<L, C, CDto>
    where C : Bases.BusinessBase<C, CDto>
    where CDto : class
  {

  }
}
