using ReactiveUI;
using System.Collections.ObjectModel;
using Server;
using System.Reactive;
using System.ComponentModel;
using Avalonia.Controls.Selection;
using ClientApp.Models;
using MessageBox.Avalonia.Enums;
using Splat;
using ClientApp.Views;

namespace ClientApp.ViewModels
{
    public class AdminHomeViewModel : ReactiveObject, IRoutableViewModel
    {

        //Holds onto the selected employee so that other pages can access the employee's information
        public static EmployeeModel? SelectedEmployee { get; set; }

        //Current employee selected
        public SelectionModel<EmployeeModel> EmployeeSelection { get; } = new();

        public RoutingState Router { get; } = new RoutingState();


        public ReactiveCommand<Unit, IRoutableViewModel> NavigateHome { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToImport { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToRegister { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToEmployeeInformation { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToUpdateEmployeeInfo { get; }


        //Determines if an element has been selected in the list view
        private bool _selectButtonEnabled;
        public bool SelectButtonEnabled
        {
            get
            {
                return _selectButtonEnabled;
            }
            set
            {
                //Updates that a value has been selected
                this.RaiseAndSetIfChanged(ref _selectButtonEnabled, value);
            }
        }
        //List of employees. Binded in view to display the employees
        private ObservableCollection<Models.EmployeeModel> _employees = new();
        public ObservableCollection<Models.EmployeeModel> Employees
        {
            get => _employees;
            set
            {
                this.RaiseAndSetIfChanged(ref _employees, value);
            }
        }


        public AdminHomeViewModel()
        {
            var client = new Server.Employee.EmployeeClient(Program.gRPCChannel);
            //Get employees from database
            AllEmployees info = client.getEmployees(new Google.Protobuf.WellKnownTypes.Empty());
            foreach (EmployeeInfo e in info.Employees)
            {
                //Add employees to the list
                _employees.Add(new Models.EmployeeModel(e.EmployeeId, e.FirstName, e.LastName, e.Credentials.Username, e.IsAdmin));
            }
            //Disables button every time the page is loaded
            SelectButtonEnabled = false;

            //Subscribes the selection to an event listener
            EmployeeSelection.SelectionChanged += SelectionChanged;

            Locator.CurrentMutable.Register(() => new HomePage(), typeof(IViewFor<HomePageViewModel>));
            NavigateHome = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new HomePageViewModel()));

            Locator.CurrentMutable.Register(() => new ImportFormView(), typeof(IViewFor<ImportFormViewModel>));
            NavigateToImport = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ImportFormViewModel()));

            //Locator.CurrentMutable.Register(() => new RegisterEmployeeView(), typeof(IViewFor<RegisterEmployeeViewModel>));
            NavigateToRegister = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new RegisterEmployeeViewModel()));

            Locator.CurrentMutable.Register(() => new EmployeeInformationView(), typeof(IViewFor<EmployeeInformationViewModel>));
            NavigateToEmployeeInformation = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new EmployeeInformationViewModel()));

            Locator.CurrentMutable.Register(() => new UpdateEmployeeView(), typeof(IViewFor<UpdateEmployeeViewModel>));
            NavigateToUpdateEmployeeInfo = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new UpdateEmployeeViewModel()));
        }

        /// <summary>
        /// When the user selects an employee, the button(s) are enabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs e)
        {
            // ... handle selection changed
            SelectButtonEnabled = true;
            SelectedEmployee = EmployeeSelection.SelectedItem;
        }

        /// <summary>
        /// Opens home page view and closes this view
        /// </summary>
        public void GoToHomeFromAdminHomeCommand()
        {
            NavigateHome.Execute();

        }

        /// <summary>
        /// Opens the importing form view and closes this view
        /// </summary>
        public void OpenImportFormView()
        {
            NavigateToImport.Execute();

        }

        /// <summary>
        /// Opens employee registration view and closes this view
        /// </summary>
        public void CreateEmployeeCommand()
        {
            Locator.CurrentMutable.Register(() => new RegisterEmployeeView(), typeof(IViewFor<RegisterEmployeeViewModel>));
            NavigateToRegister.Execute();
        }

        /// <summary>
        /// Deletes employee from database
        /// </summary>
        public async void DeleteEmployeeCommand()
        {

            //Want yes no dialog here
            ButtonResult result = await MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("title", "Confirm deletion", ButtonEnum.YesNo).Show();
            if (result == ButtonResult.Yes)
            {
                await MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("title", "To delete").Show();
            }
            else
            {
                await MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("title", "Clicked no").Show();
            }
        }

        /// <summary>
        /// Takes user to view the employee information
        /// </summary>
        public void GoToReadEmployeeInfoCommand()
        {
            
            NavigateToEmployeeInformation.Execute();
        }
        public void GoToUpdateEmployeeInfoCommand()
        {
            NavigateToUpdateEmployeeInfo.Execute();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
