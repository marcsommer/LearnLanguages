using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace LearnLanguages.DataAccess.Ef
{
  public static class EfHelper
  {
    public static PhraseDto ToDto(PhraseData data)
    {
      return new PhraseDto()
      {
        Id = data.Id,
        Text = data.Text,
        LanguageId = data.LanguageDataId
      };
    }
    public static PhraseData ToData(PhraseDto dto)
    {
      return new PhraseData()
      {
        Id = dto.Id,
        Text = dto.Text,
        LanguageDataId = dto.LanguageId
      };
    }

    public static LanguageDto ToDto(LanguageData data)
    {
      return new LanguageDto()
      {
        Id = data.Id,
        Text = data.Text
      };
    }
    public static LanguageData ToData(LanguageDto dto)
    {
      return new LanguageData()
      {
        Id = dto.Id,
        Text = dto.Text
      };
    }


    public static string GetConnectionString()
    {
      return ConfigurationManager.ConnectionStrings[EfResources.LearnLanguagesConnectionStringKey].ConnectionString;
    }
  }
}
