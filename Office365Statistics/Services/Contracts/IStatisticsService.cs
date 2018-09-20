using System.Threading.Tasks;
using Microsoft.Graph;

namespace Office365Statistics.Services
{
    public interface IStatisticsService
    {
        Task<int> GetNumberOfRecentFiles(GraphServiceClient client);
        Task<int> GetNumberOfSharedWithMeFiles(GraphServiceClient client);
        Task<long> GetNumberOfFiles(GraphServiceClient client);
    }
}