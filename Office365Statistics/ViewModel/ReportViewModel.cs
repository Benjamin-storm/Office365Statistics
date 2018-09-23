using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Graph;
using Office365Statistics.Model;
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

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value);
                this.UpdateFilesChartData.RaiseCanExecuteChanged();
            }
        }

        private GraphServiceClient _client;
        public GraphServiceClient Client
        {
            get { return _client; }
            set { Set(ref _client, value); }
        }

        private int _numberOfFiles;
        public int NumberOfFiles
        {
            get { return _numberOfFiles; }
            set { Set(ref _numberOfFiles, value); }
        }

        private int _numberOfRecentFiles;
        public int NumberOfRecentFiles
        {
            get { return _numberOfRecentFiles; }
            set { Set(ref _numberOfRecentFiles, value); }
        }

        private int _numberOfSharedWithMeFiles;
        public int NumberOfSharedWithMeFiles
        {
            get { return _numberOfSharedWithMeFiles; }
            set { Set(ref _numberOfSharedWithMeFiles, value); }
        }

        private List<NameValueItem> _filesChartData;
        public List<NameValueItem> FilesChartData
        {
            get { return _filesChartData; }
            set { Set(ref _filesChartData, value); }
        }

        private RelayCommand _updateFilesChartData;
        public RelayCommand UpdateFilesChartData
        {
            get
            {
                if (_updateFilesChartData == null)
                {
                    _updateFilesChartData = new RelayCommand(async () =>
                    {
                        this.IsLoading = true;

                        if (Client != null)
                        {
                            this.NumberOfRecentFiles = await this._statisticsService.GetNumberOfRecentFiles(Client);
                            this.NumberOfSharedWithMeFiles = await this._statisticsService.GetNumberOfSharedWithMeFiles(Client);
                            this.NumberOfFiles = (int)await this._statisticsService.GetNumberOfFiles(Client);
                        }

                        this.FilesChartData = new List<NameValueItem>
                        {
                            new NameValueItem
                            {
                                Name = "Recent files",
                                Value = this.NumberOfRecentFiles
                            },
                            new NameValueItem
                            {
                                Name = "Shared with me files",
                                Value = this.NumberOfSharedWithMeFiles
                            }, new NameValueItem
                            {
                                Name = "All files",
                                Value = this.NumberOfFiles
                            }
                        };

                        this.IsLoading = false;
                    }, () => !this.IsLoading);
                }

                return _updateFilesChartData;
            }
        }
    }
}
