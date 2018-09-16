using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Graph;
using Office365Statistics.Services.Contracts;

namespace Office365Statistics.ViewModel
{
    public class AuthViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;

        public AuthViewModel(IAuthenticationService authService)
        {
            _authService = authService;
        }

        private GraphServiceClient _client;
        public GraphServiceClient Client
        {
            get { return _client; }
            set { Set(ref _client, value, true); }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }

        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
            set
            {
                Set(ref _isAuthenticated, value);
                AuthenticateUser.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _authenticateUser;
        public RelayCommand AuthenticateUser
        {
            get
            {
                if (_authenticateUser == null)
                {
                    _authenticateUser = new RelayCommand(async () =>
                    {
                        try
                        {
                            Client = _authService.GetAuthenticatedClient();

                            if (Client != null)
                            {
                                User = await Client.Me.Request().GetAsync();
                                IsAuthenticated = true;
                            }
                        }
                        catch (Microsoft.Identity.Client.MsalException)
                        {
                            IsAuthenticated = false;
                        }

                    },
                    () => !this.IsAuthenticated);
                }

                return _authenticateUser;
            }
        }
    }
}
