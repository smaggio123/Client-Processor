using ClientApp.Views;
using ReactiveUI;
using Server;
using Splat;
using System;
using System.Reactive;

namespace ClientApp.ViewModels
{
    public class ProcedureUpdateViewModel : ReactiveObject, IRoutableViewModel
    {

        public string _ProcedureName { get; set; }
        public string _ProcedureDescription { get; set; }

        public RoutingState Router { get; } = new RoutingState();
        
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToProcedureListing { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToEditDocuments { get; }


        public ProcedureUpdateViewModel()
        {
            _ProcedureName = ClientProcedureListingViewModel.ProcedureName;
            _ProcedureDescription = ClientProcedureListingViewModel.ProcedureNotes;

            Locator.CurrentMutable.Register(() => new ClientProcedureListingView(), typeof(IViewFor<ClientProcedureListingViewModel>));
            NavigateToProcedureListing = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ClientProcedureListingViewModel()));

            Locator.CurrentMutable.Register(() => new MakeAProcedureView(), typeof(IViewFor<MakeAProcedureViewModel>));
            NavigateToEditDocuments = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new MakeAProcedureViewModel()));
        }

        public void UpdateProcedureCommand()
        {                              // localhost for testing purposes
            var RPCProcedure = new Procedure.ProcedureClient(Program.gRPCChannel);
            ProcedureInfo newInfo = new()
            {
                ProcedureID = ClientProcedureListingViewModel.Procedure_Id,
                ProcedureName = _ProcedureName,
                ProcedureNotes = _ProcedureDescription,
                EmployeeID = LoginPageViewModel.GlobalEmployeeID
            };
            ServiceStatus status = RPCProcedure.updateProcedure(newInfo);
            MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("title", status.ToString()).Show();
            //Route back
            NavigateToProcedureListing.Execute();
        }

        public void BackCommand()
        {
            //Route back
            NavigateToProcedureListing.Execute();
        }

        public void GoToUpdateDocuments()
        {
            NavigateToEditDocuments.Execute();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
