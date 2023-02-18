using ClientApp.Views;
using ReactiveUI;
using Splat;

namespace ClientApp.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IRoutableViewModel
    {

        public RoutingState Router{ get; } = new RoutingState();

        


        public string Greeting => "Welcome to the Business";
        /// <summary>
        /// GoToLogin goes to the login page
        /// </summary>
        public MainWindowViewModel()
        {
            Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LoginPageViewModel>));
            Router.Navigate.Execute(new LoginPageViewModel());
            
            
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();

    }
}
