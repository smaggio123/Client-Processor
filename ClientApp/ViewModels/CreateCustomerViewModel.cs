using ClientApp.Views;
using ReactiveUI;
using Server;
using Splat;
using System;
using System.ComponentModel;
using System.Reactive;

namespace ClientApp.ViewModels
{
    public class CreateCustomerViewModel:ReactiveObject, IRoutableViewModel
    {
        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> GoHome { get; }
        //Holds input for first name of client
        public string FirstName { get; set; } = string.Empty;
        
        //Holds input for last name of client
        public string LastName { get; set; } = string.Empty;

        //Holds input for email of client
        public string Email { get; set; } = string.Empty;

        //Holds input for phone number of client
        public string PhoneNumber { get; set; } = string.Empty;


        private bool buttonVisible = false;
        public bool ButtonVisible
        {
            get => buttonVisible;
            set => this.RaiseAndSetIfChanged(ref buttonVisible, value);
        }

        public CreateCustomerViewModel()
        {
            Locator.CurrentMutable.Register(() => new HomePage(), typeof(IViewFor<HomePageViewModel>));
            GoHome = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new HomePageViewModel()));
        }

        /// <summary>
        /// Onclick event for creating employee.
        /// </summary>
        public void RegisterCommand()
        {
            //if(FirstName != null && FirstName!="" && LastName!=null && LastName!="" && Email!=null && Email!="" && PhoneNumber!=null && PhoneNumber != "")
            //Makes sure that required fields have values
            if(FirstName != null && FirstName!="" && LastName!=null && LastName!="" && PhoneNumber!=null && PhoneNumber != "")
            {
                var client = new Client.ClientClient(Program.gRPCChannel);

                //Initializing the client
                var clientInfo = new ClientInfo
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNumber = PhoneNumber,
                    Email = Email
                };
                //Creating the client
                var createResponse = client.newClient(clientInfo);

                GoHome.Execute();
            }
            else
            {
                ButtonVisible = true;
            }
        }
        /// <summary>
        /// Takes user to the home page
        /// </summary>
        public void ToHomeScreenCommand()
        {
            GoHome.Execute();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
