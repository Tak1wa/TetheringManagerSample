using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Networking.NetworkOperators;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace TetheringManagerSample
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private DispatcherTimer _timer;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (_1, _2) =>
            {
                UpdateTetheringStatus();
            };
            _timer.Start();
        }

        /// <remarks>
        /// The required device capability has not been declared in the manifest.
        /// </remarks>
        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            var tetheringManager = GetCurrentTetheringManage();
            
            string apSsid = txtSSID.Text;
            string apPass = txtPASS.Text;
            var accessPointConfig = new NetworkOperatorTetheringAccessPointConfiguration()
            {
                Ssid = apSsid, Passphrase = apPass
            };
            await tetheringManager.ConfigureAccessPointAsync(accessPointConfig);
            await tetheringManager.StartTetheringAsync();

            UpdateTetheringStatus();
        }

        private async void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            var tetheringManager = GetCurrentTetheringManage();

            tetheringManager?.GetCurrentAccessPointConfiguration();
            await tetheringManager?.StopTetheringAsync();

            UpdateTetheringStatus();
        }
        
        #region Private Method

        private NetworkOperatorTetheringManager GetCurrentTetheringManage()
        {
            return NetworkOperatorTetheringManager.CreateFromConnectionProfile(
                                        NetworkInformation.GetInternetConnectionProfile()
                                        );
        }

        private int currentClientCount;
        private int maxClientCount;

        private void UpdateTetheringStatus()
        {
            var tetheringManager = GetCurrentTetheringManage();
            lblState.Text = tetheringManager?.TetheringOperationalState.ToString();

            maxClientCount = (int)tetheringManager.MaxClientCount;
            currentClientCount = (int)tetheringManager.ClientCount;
            lblClients.Text = $"{currentClientCount} / {maxClientCount}";

            listClients.Items.Clear();
            foreach(var currentClient in tetheringManager.GetTetheringClients().Select(c => $"{c.HostNames.ToString()} {c.MacAddress.ToString()}"))
            {
                listClients.Items.Add(currentClient);
            }
        }

        #endregion
    }
}
