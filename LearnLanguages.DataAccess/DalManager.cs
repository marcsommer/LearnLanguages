
using System.ComponentModel.Composition.Hosting;
namespace LearnLanguages.DataAccess
{
  public static class DalManager
  {
    public static void Initialize(CompositionContainer _Container)
    {
      throw new System.NotImplementedException();
    }

    public static IPhraseDalSync Phrase()
    {
      return Services.Container.GetExportedValue<IPhraseDalSync>();
    }

    public static ILanguageDalSync Language()
    {
      return Services.Container.GetExportedValue<ILanguageDalSync>();
    }
  }
}
