using ClientApp.Models;
using ClientApp.Views;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive;

namespace ClientApp.ViewModels
{
    public class EmployeeInformationViewModel : ReactiveObject, IRoutableViewModel
    {

        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> GoToHomePage { get; }

        private ObservableCollection<EmployeeModel> displayEmployee=new();
        public ObservableCollection<EmployeeModel> DisplayEmployee
        {
            get => displayEmployee;
            set => this.RaiseAndSetIfChanged(ref displayEmployee, value);
        }


        public EmployeeInformationViewModel()
        {
            Locator.CurrentMutable.Register(() => new AdminHomeView(), typeof(IViewFor<AdminHomeViewModel>));
            GoToHomePage = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new AdminHomeViewModel()));
            if(AdminHomeViewModel.SelectedEmployee!=null)DisplayEmployee.Add(new EmployeeModel(AdminHomeViewModel.SelectedEmployee.ID,AdminHomeViewModel.SelectedEmployee.FirstName, AdminHomeViewModel.SelectedEmployee.LastName, AdminHomeViewModel.SelectedEmployee.UserName, AdminHomeViewModel.SelectedEmployee.IsAdmin));
        }

        public void GoToHomePageCommand()
        {
            GoToHomePage.Execute();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
