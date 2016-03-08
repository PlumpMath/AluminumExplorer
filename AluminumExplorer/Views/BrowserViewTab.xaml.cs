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
        string smartDomainView = "";
        string actualUrl = "";
        Dictionary<string, string> ReservedUrls = new Dictionary<string, string>()
        {
            {"aluminumerror", "http://aluminumerror/"}
        };

        public BrowserViewTab()
        {
            try
            {
                if (Settings.AESettings.Default.CachePath == null)
                    Settings.AESettings.Default.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AluminumExplorer";
                if (!System.IO.Directory.Exists(Settings.AESettings.Default.CachePath))
                    System.IO.Directory.CreateDirectory(Settings.AESettings.Default.CachePath);
                if (!Cef.IsInitialized)
                    Cef.Initialize(new CefSettings() { CachePath = Settings.AESettings.Default.CachePath });
                InitializeComponent();
                navHelper = new NavigationHelper();
                webViewController.LoadingStateChanged += WebViewController_LoadingStateChanged;
                var scope = FocusManager.GetFocusScope(urlBar);
                FocusManager.SetFocusedElement(scope, urlBar);
                AddHandler(GotFocusEvent, new RoutedEventHandler(SelectAllText), true);
                RegisterCommands();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Browser Core Load Error");
            }
        }
        public RoutedCommand backButtonShortcut = new RoutedCommand();
        private void RegisterCommands()
        {
            backButtonShortcut.InputGestures.Add(new KeyGesture(Key.Left, ModifierKeys.Alt));
        }
        void GoBack() { if (webViewController.CanGoBack){ webViewController.Back(); } }

        private void WebViewController_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            webViewController.Dispatcher.Invoke(() =>
            {
                string addressUrl = webViewController.Address;
                if (!ReservedUrls.ContainsValue(addressUrl))
                {
                    actualUrl = addressUrl;
                    smartDomainView = new Uri(actualUrl).Host;
                    urlBar.Text = smartDomainView;
                }
            });
        }

        private void ChromiumWebBrowser_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            webViewController.RenderSize = new Size(webViewController.ActualWidth, webViewController.ActualHeight);
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
                string processedUrl = navHelper.ProcessURL(url);
                Uri tResult;
                bool validUrl = Uri.TryCreate(processedUrl, UriKind.Absolute, out tResult)
                    && tResult.Scheme == Uri.UriSchemeHttp;
                if (validUrl)
                    webViewController.Load(processedUrl);
                else
                    throw (new UriFormatException("Invalid URL."));
            }
            catch (Exception)
            {
                smartDomainView = "";
                actualUrl = urlBar.Text;
                webViewController.LoadHtml(Properties.Resources.couldnotload, ReservedUrls["aluminumerror"]);
            }
        }

        private void urlBar_LostFocus(object sender, RoutedEventArgs e)
        {
            urlBar.HorizontalContentAlignment = HorizontalAlignment.Center;
            urlBar.Text = smartDomainView;
        }

        private void urlBar_GotFocus(object sender, RoutedEventArgs e)
        {
            urlBar.HorizontalContentAlignment = HorizontalAlignment.Left;
            urlBar.Text = actualUrl;
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                Keyboard.Focus(textBox);
                textBox.SelectAll();
            }
        }

        private void urlBar_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            urlBar.SelectAll();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            urlBar.HorizontalContentAlignment = HorizontalAlignment.Center;
        }


    }
}