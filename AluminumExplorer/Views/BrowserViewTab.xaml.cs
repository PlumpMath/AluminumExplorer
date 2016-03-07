using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using AluminumExplorer.Tools;

namespace AluminumExplorer
{
    /// <summary>
    /// Interaction logic for BrowserViewTab.xaml
    /// </summary>
    public partial class BrowserViewTab : UserControl
    {
        NavigationHelper navHelper;
        public BrowserViewTab()
        {
            try
            {
                if (Settings.AESettings.Default.CachePath == null)
                    Settings.AESettings.Default.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AluminumExplorer";
                if (!System.IO.Directory.Exists(Settings.AESettings.Default.CachePath))
                    System.IO.Directory.CreateDirectory(Settings.AESettings.Default.CachePath);
                if (!Cef.IsInitialized)
                    Cef.Initialize(new CefSettings() { CachePath = Settings.AESettings.Default.CachePath});
                InitializeComponent();
                navHelper = new NavigationHelper();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Browser Core Load Error");
            }
        }

        private void ChromiumWebBrowser_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            webViewController.RenderSize = new Size(webViewController.ActualWidth,webViewController.ActualHeight);
        }

        private void urlBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                NavigateToPage(urlBar.Text);
            }
        }

        private void NavigateToPage(string url)
        {
            try
            {
                webViewController.Load(navHelper.ProcessURL(new Uri(url)));
            }
            catch (Exception)
            {
                webViewController.LoadHtml(Properties.Resources.couldnotload, "http://aluminumerror");
            }
        }
    }
}
