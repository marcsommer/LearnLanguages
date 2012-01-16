using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using LearnLanguages.Silverlight.ViewModels;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;


namespace LearnLanguages.Silverlight
{
  [Export(typeof(Interfaces.INavigationController))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class NavigationController : Interfaces.INavigationController,
                                      IHandle<Interfaces.IAuthenticationChangedEventMessage>,
                                      IPartImportsSatisfiedNotification
  {
    public NavigationController()
    {
      Services.EventAggregator.Subscribe(this);
    }
    
    public void OnImportsSatisfied()
    {
      Services.EventAggregator.Publish(new Events.PartSatisfiedEventMessage("NavigationController"));
      //Services.EventAggregator.Publish(
      //  (Interfaces.INavigationRequestedEventMessage)(new Events.NavigationRequestedEventMessage("Login")));
    }
    
    #region Handle Event Messages

    public void Handle(IAuthenticationChangedEventMessage message)
    {
      if (message.IsAuthenticated)
        Events.Publish.NavigationRequest<AuthenticationStatusViewModel>();
      else
        Events.Publish.NavigationRequest<LoginViewModel>();
    }

    public void Handle(INavigationRequestedEventMessage message)
    {
      //EXTRACT THE TYPE FROM THE NAVIGATION MESSAGE
      Type requestedViewModelType = ExtractType(message);
      if (requestedViewModelType == null)
      {
        Events.Publish.NavigationFailed<LoginViewModel>(message.NavigationInfo.NavigationId);
        return;
      }


      string requestedContractName = requestedViewModelType.FullName;

      var importDef = new System.ComponentModel.Composition.Primitives.ContractBasedImportDefinition(
        requestedContractName, System.ComponentModel.Composition.AttributedModelServices.GetTypeIdentity(requestedViewModelType),
        null, System.ComponentModel.Composition.Primitives.ImportCardinality.ExactlyOne, true, false, CreationPolicy.Any);

      var exports = Services.Container.GetExports(importDef);
      //GET THE VIEWMODEL FROM THE CONTAINER
      //var exports = Services.Container.GetExports(requestedViewModelType, typeof(ViewModelMetadataAttribute), requestedContractName);
      var exportFound = false;
      ViewModels.ViewModelBase requestedViewModel = null;
      //Hack: I can't seem to use GetExports(type,null,null).FirstOrDefault() extension method for whatever reason, so i'm just doing a foreach with a break at the end.
      foreach (var export in exports)
      {
        requestedViewModel = export.Value as ViewModels.ViewModelBase;
        if (requestedViewModel != null)
          exportFound = true;

        break; //breaks out of the foreach after the first iteration.
      }

      //IF WE FOUND A VIEWMODEL, INJECT IT IN THE SHELLVIEWMODEL
      if (exportFound)
      {
        var shellViewModel = Services.Container.GetExportedValue<ViewModels.ShellViewModel>();
        shellViewModel.Main = requestedViewModel;
        var currentUser = Csla.ApplicationContext.User;
        shellViewModel.ReloadNavigationPanel();
      }
    }

    

    #endregion

    #region Helper Methods

    private string ExtractContractName(INavigationRequestedEventMessage message)
    {
      return message.NavigationInfo.ViewModelCoreNoSpaces + @"ViewModel";
    }
    private Type ExtractType(INavigationRequestedEventMessage message)
    {
      //FORMAT: [message]ViewModel
      var retType = Type.GetType(@"LearnLanguages.Silverlight.ViewModels." + message.NavigationInfo.ViewModelCoreNoSpaces + @"ViewModel");
      return retType;
    }

    #endregion
  }
}
