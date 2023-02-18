using ClientApp.Views;
using ReactiveUI;
using Server;
using Splat;
using System;
using System.Reactive;

namespace ClientApp.ViewModels
{
    public class InitializeProcedureViewModel : ReactiveObject, IRoutableViewModel
    {
        public string _ProcedureName { get; set; } = string.Empty;
        public string _ProcedureDescription { get; set; } = string.Empty;
        public static int CurrentProcedureID { get; set; } = -1;

        public RoutingState Router { get; } = new RoutingState();
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToMakeProcedure { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToClientProcedureListing { get; }

        public InitializeProcedureViewModel()
        {
            Locator.CurrentMutable.Register(() => new MakeAProcedureView(), typeof(IViewFor<MakeAProcedureViewModel>));
            NavigateToMakeProcedure = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new MakeAProcedureViewModel()));

            Locator.CurrentMutable.Register(() => new ClientProcedureListingView(), typeof(IViewFor<ClientProcedureListingViewModel>));
            NavigateToClientProcedureListing = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ClientProcedureListingViewModel()));
        }
        public void ProceedToMakeProcedure()
        {
            var RPCProcedure = new Procedure.ProcedureClient(Program.gRPCChannel);
            ProcedureInfo i = new()
            {
                ClientID = HomePageViewModel.Client_ID,
                EmployeeID = LoginPageViewModel.GlobalEmployeeID,
                ProcedureName = _ProcedureName,
                ProcedureNotes = _ProcedureDescription
            };
            ProcedureID pid = RPCProcedure.addProcedure(i);
            ClientProcedureListingViewModel.Procedure_Id = pid.PID;
            NavigateToMakeProcedure.Execute();
        }

        public void BackCommand()
        {
            NavigateToClientProcedureListing.Execute();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
