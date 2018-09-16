namespace Office365Statistics.Services
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using Office365Statistics.Services.Contracts;

    public class AuthenticationService : IAuthenticationService
    {
        // The Client ID is used by the application to uniquely identify itself to the v2.0 authentication endpoint.
        private static readonly string clientId = App.Current.Resources["ida:ClientID"].ToString();
        public static readonly string[] Scopes = { "User.Read", "Mail.Send", "Files.ReadWrite" };

        public static PublicClientApplication IdentityClientApp = new PublicClientApplication(clientId);

        public static string TokenForUser = null;
        public static DateTimeOffset Expiration;

        private static GraphServiceClient graphClient = null;

        // Get an access token for the given context and resourceId. An attempt is first made to 
        // acquire the token silently. If that fails, then we try to acquire the token by prompting the user.
        public GraphServiceClient GetAuthenticatedClient()
        {
            if (graphClient == null)
            {
                // Create Microsoft Graph client.
                try
                {
                    graphClient = new GraphServiceClient(
                        "https://graph.microsoft.com/v1.0",
                        new DelegateAuthenticationProvider(
                            async (requestMessage) =>
                            {
                                var token = await GetTokenForUserAsync();
                                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                            }));
                    return graphClient;
                }

                catch (Exception ex)
                {
                    Debug.WriteLine("Could not create a graph client: " + ex.Message);
                }
            }

            return graphClient;
        }


        /// <summary>
        /// Get Token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        public async Task<string> GetTokenForUserAsync()
        {
            AuthenticationResult authResult;
            try
            {
                var accounts = await IdentityClientApp.GetAccountsAsync();
                authResult = await IdentityClientApp.AcquireTokenSilentAsync(Scopes, accounts.First());
                TokenForUser = authResult.AccessToken;
            }

            catch (Exception)
            {
                if (TokenForUser == null || Expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
                {
                    authResult = await IdentityClientApp.AcquireTokenAsync(Scopes);

                    TokenForUser = authResult.AccessToken;
                    Expiration = authResult.ExpiresOn;
                }
            }

            return TokenForUser;
        }

        /// <summary>
        /// Signs the user out of the service.
        /// </summary>
        public async void SignOut()
        {
            foreach (var account in await IdentityClientApp.GetAccountsAsync())
            {
                await IdentityClientApp.RemoveAsync(account);
            }

            graphClient = null;
            TokenForUser = null;
        }
    }
}
