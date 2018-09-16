using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Graph;
using Office365Statistics.Services;
using Office365Statistics.Services.Contracts;

namespace Office365Statistics.ViewModel
{
    public class ReportViewModel : ViewModelBase
    {
        private readonly IStatisticsService _statisticsService;

        public ReportViewModel(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;

            MessengerInstance.Register<PropertyChangedMessage<GraphServiceClient>>(this, (property) => this.Client = property.NewValue);
        }

        private GraphServiceClient _client;
        public GraphServiceClient Client
        {
            get { return _client; }
            set { Set(ref _client, value); }
        }

        private long _numberOfFiles;
        public long NumberOfFiles
        {
            get { return _numberOfFiles; }
            set { Set(ref _numberOfFiles, value); }
        }

        private RelayCommand _getNumberOfFiles;
        public RelayCommand GetNumberOfFiles
        {
            get
            {
                if (_getNumberOfFiles == null)
                {
                    _getNumberOfFiles = new RelayCommand(async () =>
                    {
                        if (Client != null)
                        {
                            this.NumberOfFiles = await this._statisticsService.GetNumberOfFiles(Client);
                        }
                        else
                        {
                            this.NumberOfFiles = 0;
                        }
                    });
                }

                return _getNumberOfFiles;
            }
        }
    }
}
