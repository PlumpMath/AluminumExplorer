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

namespace AluminumExplorer
{
    /// <summary>
    /// Interaction logic for BrowserViewTab.xaml
    /// </summary>
    public partial class BrowserViewTab : UserControl
    {
        public BrowserViewTab()
        {
            try
            {
                Cef.Initialize(new CefSettings());
                InitializeComponent();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Browser Core Load Error");
            }
        }
    }
}
