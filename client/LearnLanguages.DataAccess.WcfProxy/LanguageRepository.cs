using System;
using System.Windows;
using System.Collections.Generic;
using System.ServiceModel;

namespace LearnLanguages.DataAccess.WcfProxy
{
  public class LanguageRepository : DependencyObject, ILanguageDalAsync
  {

    #region todo
    public Result<LanguageDto> New(object criteria)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> Fetch(Guid id)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> Update(LanguageDto dto)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> Insert(LanguageDto dto)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> Delete(Guid id)
    {
      throw new NotImplementedException();
    }

    #endregion

    public Result<ICollection<LanguageDto>> GetAll()
    {
      Result<ICollection<LanguageDto>> retResult = Result<ICollection<LanguageDto>>.Undefined(null);
      try
      {
        ICollection<LanguageDto> dtos = null;
        var languageService = GetLanguageService();

        AsyncCallback asyncCallback = (result =>
        {

        });


        retResult = Result<ICollection<LanguageDto>>.Success(dtos);
      }
      catch (Exception ex)
      {
        retResult = Result<ICollection<LanguageDto>>.FailureWithInfo(null, ex);
      }

      return retResult;
    }

    public ILanguageDalAsync GetLanguageService()
    {
      EndpointAddress address = new EndpointAddress(@"http://localhost:8000/LearnLanguage/LanguageService");
      BasicHttpBinding binding = new BasicHttpBinding();
      ChannelFactory<ILanguageDalAsync> factory = new ChannelFactory<ILanguageDalAsync>(binding, address);
      ILanguageDalAsync channel = factory.CreateChannel();
      return channel;
    }
  }
}
