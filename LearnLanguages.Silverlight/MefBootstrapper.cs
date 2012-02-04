using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Caliburn.Micro;
//using LearnLanguages.DataAccess;

//THIS CODE IS BASED ON THE CODE MADE BY ROB EISENBERG AT http://caliburnmicro.codeplex.com/discussions/218561?ProjectName=caliburnmicro

namespace LearnLanguages.Silverlight
{
  public class MefBootstrapper : Bootstrapper<ViewModels.ShellViewModel>//, IHandle<Interfaces.IPartSatisfiedEventMessage>
  {
    private CompositionContainer _Container;

#if DEBUG
    private DebugEventMessageListener _Listener;
#endif

    protected override void Configure()
    {
      //CREATE ASSEMBLY CATALOGS FOR COMPOSITION OF APPLICATION (WPF)
      AssemblyCatalog catThis = new AssemblyCatalog(typeof(MefBootstrapper).Assembly);
      AssemblyCatalog catStudy = new AssemblyCatalog(typeof(Study.RandomPhraseStudyPartner).Assembly);
      AggregateCatalog catAll = new AggregateCatalog(catThis, catStudy);

      //NEW UP CONTAINER WITH CATALOGS
      _Container = new CompositionContainer(catAll);

      //ADD BATCH FOR SERVICES TO CONTAINER, INCLUDING CONTAINER ITSELF, AND INITIALIZE SERVICES
      var batch = new CompositionBatch();
      batch.AddExportedValue<IWindowManager>(new WindowManager());
      batch.AddExportedValue<IEventAggregator>(new EventAggregator());
      batch.AddExportedValue(_Container);
      _Container.Compose(batch);
      Services.Initialize(_Container);

      //INITIALIZE CSLA DATA PORTAL
      //Csla.DataPortalClient.WcfProxy.DefaultUrl = "http://localhost:50094/SlPortal.svc";
      Csla.DataPortal.ProxyTypeName = typeof(Compression.CompressedProxy<>).AssemblyQualifiedName;
      Csla.DataPortalClient.WcfProxy.DefaultUrl = DataAccess.DalResources.WcfProxyDefaultUrl;

      //INITIALIZE DALMANAGER
      //DalManager.Initialize(_Container);

#if DEBUG
      //SUBSCRIBE TO EVENTAGGREGATOR
      Services.EventAggregator.Subscribe(this);
      _Listener = new DebugEventMessageListener();
#endif
    }
    protected override object GetInstance(Type serviceType, string key)
    {
      string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
      var exports = _Container.GetExportedValues<object>(contract);

      if (exports.Count() > 0)
        return exports.First();

      throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
    }
    protected override IEnumerable<object> GetAllInstances(Type serviceType)
    {
      return _Container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
    }
    protected override void BuildUp(object instance)
    {
      _Container.SatisfyImportsOnce(instance);
    }

    //private bool ShellModelSatisfied = false;
    //private bool NavigationControllerSatisfied = false;

    //public void Handle(Interfaces.IPartSatisfiedEventMessage message)
    //{
    //  if (message.Part == "Shell")
    //    ShellModelSatisfied = true;
    //  else if (message.Part == "NavigationController")
    //    NavigationControllerSatisfied = true;

    //  if (ShellModelSatisfied && NavigationControllerSatisfied)
    //  {
    //    Services.EventAggregator.Unsubscribe(this);
    //    //Services.EventAggregator.Publish(new Events.NavigationRequestedEventMessage("Login"));
    //    Navigation.Publish.NavigationRequest<ViewModels.LoginViewModel>(AppResources.BaseAddress);
    //  }
    //}
  }
}
