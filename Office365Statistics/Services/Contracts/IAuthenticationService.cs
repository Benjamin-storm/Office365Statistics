using System.Threading.Tasks;
using Microsoft.Graph;

namespace Office365Statistics.Services.Contracts
{
    public interface IAuthenticationService
    {
        GraphServiceClient GetAuthenticatedClient(bool displayAuthPopup = true);

        Task<string> GetTokenForUserAsync(bool displayAuthPopup);

        void SignOut();
    }
}