
using System.ComponentModel.Composition.Hosting;
namespace LearnLanguages.DataAccess
{
  public static class DalManager
  {
    public static void Initialize(CompositionContainer _Container)
    {
      throw new System.NotImplementedException();
    }

    public static IPhraseDalAsync Phrase()
    {
      return Services.Container.GetExportedValue<IPhraseDalAsync>();
    }

    public static ILanguageDalAsync Language()
    {
      return Services.Container.GetExportedValue<ILanguageDalAsync>();
    }
  }
}
