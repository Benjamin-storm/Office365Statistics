using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using Office365Statistics.Services.Contracts;

namespace Office365Statistics.Services
{
    public class StatisticsService : IStatisticsService
    {
        public async Task<long> GetNumberOfFiles(GraphServiceClient client)
        {
            var request = client.Me.Drive.Recent().Request();
            var result = await request.GetAsync();

            return result.Count;
        }
    }
}
