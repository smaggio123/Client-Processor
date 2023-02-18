using ClientApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using Server;
using Avalonia.Controls.Selection;
using System.Reactive;
using System.ComponentModel;
using Splat;
using ClientApp.Views;

namespace ClientApp.ViewModels
{

    public class HomePageViewModel : ReactiveObject, IRoutableViewModel
    {
        //Holds whether the user has admin privilages or not
        public bool ShowAdminButton { get; set; }

        
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

        private string searchNameTextInput = string.Empty;
        public string SearchNameTextInput
        {
            get
            {
                return searchNameTextInput;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref searchNameTextInput, value);
                //Search for the updated string
                SearchForClientCommand();
            }
        }

        //Holds the client name
        public static string ClientName { get; set; } = string.Empty;

        //Holds the client Id
        public static int Client_ID { get; set; }



        //The client list
        public ObservableCollection<Customer> CustomerItems { get; set; } = new();
        
        //Holds the Ids of the clients that are listed
        public List<int> ListOfClientIDs { get; set; } = new();

        //Holds the selected client
        public SelectionModel<Customer> ClientSelection { get; } = new();

        public RoutingState Router{ get; } = new RoutingState();


        // The command that navigates a user to a view model.
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToCreateCustomer { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToAdminHome { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToClientProcedures { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToClientInformation { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToLogin { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToUpdateClientInfo { get; }

        

        /// <summary>
        /// Constructor for the viewmodel. initializes the list of employees
        /// </summary>
        public HomePageViewModel()
        {
            //Subscribes the selection event listener
            ClientSelection.SelectionChanged += SelectionChanged;
            //Sets button to false when page is loaded
            SelectButtonEnabled = false;
            //Determines whether or not to show the admin button
            ShowAdminButton = LoginPageViewModel.GlobalIsAdmin;

            Locator.CurrentMutable.Register(() => new ClientProcedureListingView(), typeof(IViewFor<ClientProcedureListingViewModel>));
            NavigateToClientProcedures = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ClientProcedureListingViewModel()));

            Locator.CurrentMutable.Register(() => new CreateCustomerPage(), typeof(IViewFor<CreateCustomerViewModel>));
            NavigateToCreateCustomer = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new CreateCustomerViewModel()));

            Locator.CurrentMutable.Register(() => new AdminHomeView(), typeof(IViewFor<AdminHomeViewModel>));
            NavigateToAdminHome = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new AdminHomeViewModel()));

            Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LoginPageViewModel>));
            NavigateToLogin = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new LoginPageViewModel()));

            Locator.CurrentMutable.Register(() => new ClientInformationView(), typeof(IViewFor<ClientInformationViewModel>));
            NavigateToClientInformation = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ClientInformationViewModel()));
            
            Locator.CurrentMutable.Register(() => new UpdateClientView(), typeof(IViewFor<UpdateClientViewModel>));
            NavigateToUpdateClientInfo = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new UpdateClientViewModel()));

        }

        /* Event Handlers Below */
        public void SearchForClientCommand()
        {
            //Clear displayed list
            if(CustomerItems!=null)CustomerItems.Clear();
            //Clear Id list associated with displayed clients
            ListOfClientIDs.Clear();

            //If there is a name to search
            if (SearchNameTextInput != null && SearchNameTextInput.Length > 0)
            {
                var client = new Client.ClientClient(Program.gRPCChannel);
                AllClients info = client.searchClientsByName(new ClientName() { CName = SearchNameTextInput });
                if (info.Clients.Count > 0)
                {
                    foreach (var clientInformation in info.Clients)
                    {
                        CustomerItems?.Add(new Customer(clientInformation.ClientId, clientInformation.FirstName, clientInformation.LastName, clientInformation.PhoneNumber, clientInformation.Email));

                        ListOfClientIDs.Add(clientInformation.ClientId);
                    }
                }
                else
                {
                    //There are no clients to pick from, so disables buttons
                    SelectButtonEnabled = false;
                }
            }
            else
            {
                //There are no clients to pick from, so disables buttons
                SelectButtonEnabled = false;
            }
        }

        /// <summary>
        /// On click event to creating customer button
        /// </summary>
        public void CreateCustomerCommand()
        {
            NavigateToCreateCustomer.Execute();
        }


        /// <summary>
        /// Takes user to admin home view
        /// </summary>
        public void GoToAdminHomeCommand()
        {
            NavigateToAdminHome.Execute();
        }

        public void GoGoToClientInformationCommand()
        {
                NavigateToClientInformation.Execute();
        }

        public void SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs e)
        {
            // ... handle selection changed
            SelectButtonEnabled = true;
            Client_ID = ListOfClientIDs[ClientSelection.SelectedIndex];
            if(ClientSelection.SelectedItem!=null)ClientName = ClientSelection.SelectedItem.FirstName;
        }

        /// <summary>
        /// Logs out user
        /// </summary>
        public void LogoutCommand()
        {
            NavigateToLogin.Execute();
        }

        /// <summary>
        /// Once client is selected, goes to the procedure listing page
        /// </summary>
        public void GoToClientProceduresCommand()
        {
            //If a client is selected
            if (ClientSelection != null)
            {
                NavigateToClientProcedures.Execute();
            }
        }

        public void GoToUpdateClientCommand()
        {
            NavigateToUpdateClientInfo.Execute();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
