using ClientApp.Views;
using ReactiveUI;
using Server;
using Splat;
using System.Reactive;

namespace ClientApp.ViewModels
{
    public class ClientInformationViewModel : ReactiveObject, IRoutableViewModel
    {
        //public ObservableCollection<string> ListOfInformationFields { get; set; } = new()
        //{ "First name: ","Last name:","Phone number: ", "Email: "};

        public string ClientFirstNameInfo { get; set; } = string.Empty;
        public string ClientLastNameInfo { get; set; } = string.Empty;
        public string ClientPhoneNumberInfo { get; set; } = string.Empty;
        public string ClientEmailInfo { get; set; } = string.Empty;

        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> GoToHomePage { get; }


        public ClientInformationViewModel()
        {
            Locator.CurrentMutable.Register(() => new HomePage(), typeof(IViewFor<HomePageViewModel>));
            GoToHomePage = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new HomePageViewModel()));
            //HomePageViewModel.Client_ID

            var client = new Client.ClientClient(Program.gRPCChannel);
            AllClients info = client.searchClientsByName(new ClientName() { CName = HomePageViewModel.ClientName });
            
            if (info.Clients.Count > 0)
            {
                var c = info.Clients[0];
                ClientFirstNameInfo = c.FirstName;
                ClientLastNameInfo = c.LastName;
                ClientPhoneNumberInfo = c.PhoneNumber;
                ClientEmailInfo = c.Email;
            }
        }
        public void goToHomePageCommand()
        {
            GoToHomePage.Execute();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
