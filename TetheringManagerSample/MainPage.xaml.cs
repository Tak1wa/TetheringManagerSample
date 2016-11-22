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
        
        /// <summary>
        /// アクセスポイントの構成を行いテザリングを開始する
        /// </summary>
        /// <remarks>
        /// The required device capability has not been declared in the manifest.
        /// </remarks>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var tetheringManager = NetworkOperatorTetheringManager.CreateFromConnectionProfile(
                                        NetworkInformation.GetInternetConnectionProfile()
                                        );
            
            string AP_SSID = txtSSID.Text;
            string AP_PASS = txtPASS.Text;
            var accessPointConfig = new NetworkOperatorTetheringAccessPointConfiguration()
            {
                Ssid = AP_SSID, Passphrase = AP_PASS
            };
            
            await tetheringManager.ConfigureAccessPointAsync(accessPointConfig);
            
            await tetheringManager.StartTetheringAsync();
        }
    }
}
