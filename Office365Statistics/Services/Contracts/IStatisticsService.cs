using System.Threading.Tasks;
using Microsoft.Graph;

namespace Office365Statistics.Services
{
    public interface IStatisticsService
    {
        Task<long> GetNumberOfFiles(GraphServiceClient client);
    }
}