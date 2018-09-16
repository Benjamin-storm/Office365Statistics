using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Office365Statistics.Services;
using Office365Statistics.Services.Contracts;

namespace Office365Statistics.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();
            SimpleIoc.Default.Register<IStatisticsService, StatisticsService>();

            SimpleIoc.Default.Register<AuthViewModel>(true);
            SimpleIoc.Default.Register<ReportViewModel>(true);
        }

        public AuthViewModel Auth => ServiceLocator.Current.GetInstance<AuthViewModel>();

        public ReportViewModel Report => ServiceLocator.Current.GetInstance<ReportViewModel>();
    }
}
