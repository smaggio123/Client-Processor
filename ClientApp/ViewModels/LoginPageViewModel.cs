using Server;
using ReactiveUI;
using System.Reactive;
using System.ComponentModel;
using Splat;
using ClientApp.Views;

namespace ClientApp.ViewModels
{
    public class LoginPageViewModel : ReactiveObject, INotifyPropertyChanged,IRoutableViewModel
    {
        //Holds current user
        public static string GlobalUserName { get; set; } = string.Empty;
        public static int GlobalEmployeeID{ get; set; }

        //Holds whether the user has admin privilages
        public static bool GlobalIsAdmin { get; set; }

        //Holds the username input
        public string UserName { get; set; } = string.Empty;
        
        //Holds the password input
        public string Password {get;set;} = string.Empty;

        //Tracks number of invalid login attempts
        public static int InvalidCredentialsCount { get; set; }

        private int loginAttempts = 0;
        public int LoginAttempts
        {
            get => loginAttempts;
            set => this.RaiseAndSetIfChanged(ref loginAttempts, value);
        }
        private string warningMessage=string.Empty;
        public string WarningMessage
        {
            get => warningMessage;
            set => this.RaiseAndSetIfChanged(ref warningMessage, value);
        }

        private bool loggingIn = true;
        public bool LoggingIn
        {
            get => loggingIn;
            set
            {
                this.RaiseAndSetIfChanged(ref loggingIn, value);
            }
        }



        // Required by the IScreen interface.
        public RoutingState Router { get; } = new RoutingState();

        // The command that navigates a user to first view model.
        public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get;}


        /// <summary>
        /// Constructor for the viewmodel. initializes the routing
        /// </summary>
        public LoginPageViewModel()
        {
            loginAttempts = 0;
            //Resets count every time this page loads
            InvalidCredentialsCount = 0;

            Locator.CurrentMutable.Register(() => new HomePage(), typeof(IViewFor<HomePageViewModel>));
            //Function for going to home page
            GoNext = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new HomePageViewModel()));
        }


        /// <summary>
        /// onclick event for logging in
        /// </summary>
        public void LoginCommand()
        {
            //Only runs code if username and password fields are filled
            if (UserName != null && UserName != "" && Password != null && Password != "")
            {
                var ClientApp = new Employee.EmployeeClient(Program.gRPCChannel);

                var credentials = new LoginCredentials
                {
                    Username = UserName,
                    Password = Password,
                };

                var serviceResponse = ClientApp.doLogin(credentials);
                //If the credentials are valid
                if (serviceResponse.IsSuccessfulLogin)
                {
                    GlobalUserName = UserName;
                    GlobalIsAdmin = serviceResponse.IsAdmin;

                    //Takes user to home page
                    GoNext.Execute();

                    GlobalEmployeeID = serviceResponse.EmployeeId;

                    //Uncomment if you want a message that lets the user know that they logged in
                    //MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Success", "User: " + GlobalUserName + ", logged in successfully.").Show();
                }
                else
                {
                    ++LoginAttempts;
                    WarningMessage = "Invalid credentials (" + LoginAttempts + ")";
                    LoggingIn = false;
                }
            }

        }
        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
