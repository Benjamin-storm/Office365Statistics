using System.Threading.Tasks;
using Microsoft.Graph;

namespace Office365Statistics.Services.Contracts
{
    public interface IAuthenticationService
    {
        GraphServiceClient GetAuthenticatedClient();

        Task<string> GetTokenForUserAsync();

        void SignOut();
    }
}