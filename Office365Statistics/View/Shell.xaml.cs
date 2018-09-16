using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Office365Statistics.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public Shell(Frame frame)
        {
            this.InitializeComponent();
            ShellSplitView.Content = frame; // ShellSplitView is the SplitView we put in Shell.xaml
            (ShellSplitView.Content as Frame).Navigate(typeof(MainPage));
        }

        // TODO: Switch navigation to service
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            ShellSplitView.IsPaneOpen = !ShellSplitView.IsPaneOpen;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            (ShellSplitView.Content as Frame).Navigate(typeof(MainPage));
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            (ShellSplitView.Content as Frame).Navigate(typeof(AuthView));
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            (ShellSplitView.Content as Frame).Navigate(typeof(ReportView));
        }
    }
}
