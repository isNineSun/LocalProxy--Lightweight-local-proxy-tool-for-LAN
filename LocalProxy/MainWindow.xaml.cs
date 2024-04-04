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
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using HandyControl.Controls;
using System.Configuration;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Diagnostics;
using System.ComponentModel;

namespace LocalProxy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const int INTERNET_OPTION_REFRESH = 37;

        private bool isConnected = false;
        private bool isHidden = false;

        public MainWindow()
        {
            InitializeComponent();

            string ServerIP_Saved = ConfigurationManager.AppSettings["ServerIP"];
            string ServerPort_Saved = ConfigurationManager.AppSettings["ServerPort"];
            string Exclude_Saved = ConfigurationManager.AppSettings["Exclude"];

            ServerIP.Text = ServerIP_Saved;
            ServerPort.Text = ServerPort_Saved;
            Exclude.Text = Exclude_Saved;
        }

        private void SendMsg2Desktop(string MsgTitle, string MsgContent)
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText(MsgTitle)
                .AddText(MsgContent)
                .Show();
        }

        private void Set_Proxy_hdlr(object sender, RoutedEventArgs e)
        {
            Color StartColorHex = (Color)ColorConverter.ConvertFromString("#326CF3");
            Color TerminateColorHex = (Color)ColorConverter.ConvertFromString("#DB3340");
            SolidColorBrush StartColor = new SolidColorBrush(StartColorHex);
            SolidColorBrush TerminateColor = new SolidColorBrush(TerminateColorHex);

            Geometry StartGeometry = (Geometry)FindResource("AudioGeometry");
            Geometry TeminateGeometry = (Geometry)FindResource("AlignBottomGeometry");

            if ((!string.IsNullOrEmpty(ServerIP.Text)) && (!string.IsNullOrEmpty(ServerPort.Text)))
            {
                if (TestConnectivity(ServerIP.Text))
                {
                    string proxyhost = $"http://{ServerIP.Text}:{ServerPort.Text}";
                    string excluderules = Exclude.Text;

                    Parameters_Saved();

                    try
                    {
                        const string userRoot = "HKEY_CURRENT_USER";
                        const string subkey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
                        const string keyName = userRoot + @"\" + subkey;
                        Registry.SetValue(keyName, "ProxyServer", isConnected ? "":proxyhost);
                        Registry.SetValue(keyName, "ProxyOverride", "<local>" + excluderules);
                        Registry.SetValue(keyName, "ProxyEnable", isConnected ? "0":"1", RegistryValueKind.DWord);
                        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
                        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);

                        ProxyButton.Background = isConnected ? StartColor : TerminateColor;
                        ProxyButton.ToolTip = isConnected ? "关闭服务器代理" : "开启服务器代理";
                        ProxyButton.Content = isConnected ? "Proxy" : "Terminate";
                        ProxyButton.SetCurrentValue(IconElement.GeometryProperty, isConnected ? StartGeometry : TeminateGeometry);

                        ServerIP.IsEnabled = isConnected ? true: false;
                        ServerPort.IsEnabled = isConnected ? true : false;

                        SendMsg2Desktop((isConnected ? "关闭代理成功": "开启代理成功"), 
                            isConnected ? $"已关闭指向{ServerIP.Text}服务器的代理服务" : $"代理服务器连接已建立：{proxyhost}");

                        //更新连接状态
                        isConnected = !isConnected;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("proxy_error");
                    }
                }
                
            }
        }

        private bool TestConnectivity(string ipAddress)
        {
            bool serverOn = false;
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(ipAddress);

                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine($"检测到服务器! Roundtrip time: {reply.RoundtripTime} ms");
                    serverOn = true;
                }
                else
                {
                    Console.WriteLine($"无法连接到服务器: {reply.Status}");
                    SendMsg2Desktop("无法连接到服务器", $"服务器IP有误或服务器不在线，无法获取连接：{reply.Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接服务器失败，Erroe: {ex.Message}");
                SendMsg2Desktop("连接服务器失败", $"服务器IP有误或服务器不在线，无法获取连接：{ex.Message}");
            }

            return serverOn;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Parameters_Saved();

            e.Cancel = true;
            Hide();
            isHidden = true;

            //ExitWindow exitWindow = new ExitWindow();
            //exitWindow.Show();
        }

        private void Parameters_Saved()
        { 
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["ServerIP"].Value = ServerIP.Text;
            cfa.AppSettings.Settings["ServerPort"].Value = ServerPort.Text;
            cfa.AppSettings.Settings["Exclude"].Value = Exclude.Text;
            cfa.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMsg2Desktop("test", "hello");
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void OpenPanel_Click(object sender, RoutedEventArgs e)
        {
            if (isHidden == true)
            {
                Show();
                Activate();
                isHidden = false;
            }
        }

        private void ExitAPP_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
