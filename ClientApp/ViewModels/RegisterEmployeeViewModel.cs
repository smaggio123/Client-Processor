using ClientApp.Views;
using ReactiveUI;
using Server;
using Splat;
using System;
using System.ComponentModel;
using System.Reactive;

namespace ClientApp.ViewModels
{
    public class RegisterEmployeeViewModel : ReactiveObject, IRoutableViewModel
    {


        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> GoToAdminHome { get; }

        /// <summary>
        /// Constructor for the viewmodel. initializes the view
        /// </summary>
        public RegisterEmployeeViewModel()
        {
            Locator.CurrentMutable.Register(() => new AdminHomeView(), typeof(IViewFor<AdminHomeViewModel>));
            GoToAdminHome = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new AdminHomeViewModel()));
        }
        //public event PropertyChangingEventHandler? PropertyChanging;

        //Holds first name of employee
        public string EmployeeFirstName { get; set; } = string.Empty;
        //Holds last name of employee
        public string EmployeeLastName { get; set; } = string.Empty;
        //Holds user name of employee
        public string EmployeeUserName{ get; set; } = string.Empty;
        //Holds password of employee
        public string EmployeePassword { get; set; } = string.Empty;

        //Holds admin status of employee
        private bool _employeeIsAdmin = false;
        public bool EmployeeIsAdmin
        {
            get { return _employeeIsAdmin; }
            set
            {
                _employeeIsAdmin = value;
                this.RaiseAndSetIfChanged(ref _employeeIsAdmin, value);
                //OnPropertyChanged(nameof(EmployeeIsAdmin));
            }
        }

        
        /// <summary>
        /// Onclick event for registering employee
        /// </summary>
        public void EmployeeRegisterCommand()
        {
            //If all of the fields are filled in
            if (EmployeeUserName != null && EmployeeUserName != "" && EmployeePassword != null && EmployeePassword != "" && EmployeeFirstName != null && EmployeeFirstName != "" && EmployeeLastName != null && EmployeeLastName != "")
            {
                var employee = new Server.Employee.EmployeeClient(Program.gRPCChannel);
                var loginCredentials = new LoginCredentials 
                {
                    Username = EmployeeUserName,
                    Password = EmployeePassword
                };

                var employeeInfo = new EmployeeInfo
                {
                    FirstName = EmployeeFirstName,
                    LastName = EmployeeLastName,
                    Credentials = loginCredentials,
                    IsAdmin = EmployeeIsAdmin,
                };
                //Makes employee in database
                var createResponse = employee.newEmployee(employeeInfo);


                //Takes user back to admin home view
                GoToAdminHome.Execute();
            }
        }

        /// <summary>
        /// Takes uesr to admin home view
        /// </summary>
        public void ToAdminHomeCommand()
        {
            GoToAdminHome.Execute();
        }

        public void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
