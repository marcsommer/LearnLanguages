using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel;
using LearnLanguages.DataAccess;
using System.Diagnostics;

namespace LearnLanguages.Service
{
  class Program
  {
    static void Main(string[] args)
    {
      Trace.Listeners.Add(new ConsoleTraceListener());

      var phraseHost = StartService("Phrase", typeof(IPhraseDalAsync), typeof(PhraseService));
      var languageHost = StartService("Language", typeof(ILanguageDalAsync), typeof(LanguageService));

      Trace.WriteLine("Services started.  Press <ENTER> to exit.");
      Console.ReadLine();

      phraseHost.Abort();
      languageHost.Abort();
    }

    private static ServiceHost StartService(string serviceNameWithoutServiceOnTheEnd, 
                                     Type contractInterface, 
                                     Type contractImplementorService)
    {
      //Uri baseAddress = new Uri(@"http://localhost:8000/LearnLanguage/Phrase");
      Uri baseAddress = new Uri(ServiceResources.BaseAddressEndsWithSlashButNeedsSuffix + serviceNameWithoutServiceOnTheEnd);
      ServiceHost host = new ServiceHost(contractImplementorService, baseAddress);
      try
      {
        //ADD ENDPOINT
        var endpointAddress = serviceNameWithoutServiceOnTheEnd + "Service";
        host.AddServiceEndpoint(contractInterface, new BasicHttpBinding(), endpointAddress);

        //ENABLE METADATA EXCHANGE
        ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
        smb.HttpGetEnabled = true;
        host.Description.Behaviors.Add(smb);

        host.Open();
        Trace.WriteLine(serviceNameWithoutServiceOnTheEnd + " Service Started at \r\n");
        Trace.WriteLine(baseAddress.ToString() + "\r\n");
      }
      catch (CommunicationException ce)
      {
        Trace.WriteLine("Could not start " + serviceNameWithoutServiceOnTheEnd + " Service at \r\n)");
        Trace.WriteLine(baseAddress.ToString() + "\r\n");
        Trace.WriteLine(DateTime.Now.ToShortTimeString() + " | " + ce.Message + "\r\n");
        host.Abort();
      }

      return host;
    }
  }
}
