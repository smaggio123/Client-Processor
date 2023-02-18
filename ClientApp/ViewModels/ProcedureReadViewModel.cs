using ClientApp.Views;
using ReactiveUI;
using Splat;
using System.Reactive;

namespace ClientApp.ViewModels
{
    public class ProcedureReadViewModel : ReactiveObject, IRoutableViewModel
    {
        public string NameOfProcedure { get; set; } = string.Empty;
        public string NotesOfProcedure { get; set; } = string.Empty;

        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> GoToViewPhotos { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoToViewForms { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoBackToListing { get; }

        public ProcedureReadViewModel()
        {

            //ClientProcedureListingViewModel.Procedure_Id
            //ClientProcedureListingViewModel.SelectedProcedure
            if(ClientProcedureListingViewModel.SelectedProcedure!=null)NameOfProcedure = ClientProcedureListingViewModel.SelectedProcedure.ProcedureName;
            if (ClientProcedureListingViewModel.SelectedProcedure != null) NotesOfProcedure = ClientProcedureListingViewModel.SelectedProcedure.procedureNotes;
            Locator.CurrentMutable.Register(() => new PhotosViewingView(), typeof(IViewFor<PhotosViewingViewModel>));
            GoToViewPhotos = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new PhotosViewingViewModel()));

            Locator.CurrentMutable.Register(() => new FormViewingView(), typeof(IViewFor<FormViewingViewModel>));
            GoToViewForms = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new FormViewingViewModel()));

            Locator.CurrentMutable.Register(() => new ClientProcedureListingView(), typeof(IViewFor<ClientProcedureListingViewModel>));
            GoBackToListing = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ClientProcedureListingViewModel()));
        }

        public void GoToFormViewingMenu()
        {
            GoToViewForms.Execute();
        }

        public void GoToPhotosViewingMenu()
        {
            GoToViewPhotos.Execute();
        }

        public void GoToProcedureListingCommand()
        {
            GoBackToListing.Execute();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
