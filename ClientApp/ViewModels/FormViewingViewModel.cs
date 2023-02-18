using ClientApp.Views;
using ReactiveUI;
using Splat;
using System.Reactive;

namespace ClientApp.ViewModels
{
    public class FormViewingViewModel : ReactiveObject, IRoutableViewModel
    {
        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }

        public FormViewingViewModel()
        {
            Locator.CurrentMutable.Register(() => new ProcedureReadView(), typeof(IViewFor<ProcedureReadViewModel>));
            GoBack = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ProcedureReadViewModel()));
        }
        
        public void GoBackCommand()
        {
            GoBack.Execute();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
