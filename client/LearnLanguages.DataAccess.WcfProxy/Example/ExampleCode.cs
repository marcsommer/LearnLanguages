using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.Collections.ObjectModel;

namespace LearnLanguages.DataAccess.WcfProxy.Example
{
  public interface ICompanyService
  {
    [OperationContract]
    FetchCompanyResponse GetAllCompanies();

    [OperationContract]
    FetchContactsResponse FetchContacts(FetchContactsRequest request);

    [OperationContract]
    SaveResponse Save(SaveCompanyRequest request);
  }

  public class CompanyRepository : DependencyObject, ICompanyRepository
  {
    public ObservableCollection<Company> GetAllCompanies()
    {
      ObservableCollection<Company> companies = new ObservableCollection<Company>();

      ICompanyService companyService = GetCompanyService();

      AsyncCallback asyncCallback = (result =>
      {
        FetchCompanyResponse response = ((ICompanyService)result.AsyncState).EndGetAllCompanies(result);

        CompanyMapper mapper = new CompanyMapper();

        var query = from companyInfo in response.Companies
                    select mapper.MapTo(companyInfo);

        Action method = () => query.ToList().ForEach(x => companies.Add(x));

        Dispatcher.BeginInvoke(method);
      });

      companyService.BeginGetAllCompanies(asyncCallback, companyService);

      return companies;
    }

    public ObservableCollection<Contact> GetContactsByCompany(Company company)
    {
      if (company == null)
      {
        throw new ArgumentNullException("company");
      }

      ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();

      ICompanyService companyService = GetCompanyService();

      AsyncCallback asyncCallback = (result =>
      {
        FetchContactsResponse response = ((ICompanyService)result.AsyncState).EndFetchContacts(result);

        ContactMapper mapper = new ContactMapper();

        Action method = () =>
        {
          mapper.MapTo(response.Contacts).ForEach(x => contacts.Add(x));
        };

        Dispatcher.BeginInvoke(method);
      });

      FetchContactsRequest request = new FetchContactsRequest { CompanyId = company.Id };

      companyService.BeginFetchContacts(request, asyncCallback, companyService);

      return contacts;
    }

    public void Save(Company company)
    {
      if (company == null)
      {
        throw new ArgumentNullException("company");
      }

      ICompanyService companyService = GetCompanyService();

      AsyncCallback asyncCallback = (result =>
      {
        SaveResponse response = ((ICompanyService)result.AsyncState).EndSave(result);

        Action method = () =>
        {
          if (response.WasSuccessful)
          {
            // Do whatever
          }
          else
          {
            // Show some validation message, etc etc. not relavant for this example
          }
        };

        Dispatcher.BeginInvoke(method);
      });

      SaveCompanyRequest request = new SaveCompanyRequest
      {
        Company = new CompanyMapper().MapFrom(company)
      };

      companyService.BeginSave(request, asyncCallback, companyService);
    }

    private ICompanyService GetCompanyService()
    {
      return new ChannelFactory("SomeEndpointThatMatchsTheConfig").CreateChannel();
    }
  }
}
