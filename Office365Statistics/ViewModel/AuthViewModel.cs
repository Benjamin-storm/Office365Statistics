using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Graph;
using Office365Statistics.Services.Contracts;
using System;

namespace Office365Statistics.ViewModel
{
    public class AuthViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;

        public AuthViewModel(IAuthenticationService authService)
        {
            _authService = authService;

            AttemptAuthentication();
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
                AuthenticateUser.RaiseCanExecuteChanged();
                SignOutUser.RaiseCanExecuteChanged();
            }
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
                SignOutUser.RaiseCanExecuteChanged();
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
                        this.IsLoading = true;

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

                        this.IsLoading = false;

                    },
                    () => !this.IsAuthenticated && !this.IsLoading);
                }

                return _authenticateUser;
            }
        }

        private RelayCommand _signOutUser;
        public RelayCommand SignOutUser
        {
            get
            {
                if (_signOutUser == null)
                {
                    _signOutUser = new RelayCommand(() =>
                    {
                        this.IsLoading = true;

                        if (Client != null)
                        {
                            _authService.SignOut();
                            IsAuthenticated = false;
                        }

                        this.IsLoading = false;

                    },
                    () => this.IsAuthenticated && !this.IsLoading);
                }

                return _signOutUser;
            }
        }

        private async void AttemptAuthentication()
        {
            this.IsLoading = true;

            try
            {
                Client = _authService.GetAuthenticatedClient(false);

                if (Client != null)
                {
                    User = await Client.Me.Request().GetAsync();
                    IsAuthenticated = true;
                }
            }
            catch (Exception e) when (e is Microsoft.Identity.Client.MsalException || e is ServiceException)
            {
                IsAuthenticated = false;
            }

            this.IsLoading = false;
        }
    }
}
