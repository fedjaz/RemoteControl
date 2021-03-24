using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControlPage : ContentPage
    {
        App mainPage;
        AppSettings appSettings;
        ServerConnector.Connection Connection;

        double x, y;
        Task touchWatcher;
        CancellationTokenSource cancelTokenSource;
        CancellationToken token;
        public ControlPage(App mainPage, AppSettings appSettings)
        {
            this.mainPage = mainPage;
            this.appSettings = appSettings;
            Connection = new ServerConnector.Connection(appSettings.serverURI);
            Connection.Connected += () => MainThread.BeginInvokeOnMainThread(() => statusCircle.BackgroundColor = Color.Green);
            Connection.ConnectionLost += () => MainThread.BeginInvokeOnMainThread(() => statusCircle.BackgroundColor = Color.Red);
            InitializeComponent();       
            PanGestureRecognizer touchBarRecognizer = new PanGestureRecognizer();
            touchBarRecognizer.PanUpdated += Hover;
            TouchBar.GestureRecognizers.Add(touchBarRecognizer);
            Connect();
        }
        
        public async Task Connect()
        {
            await Connection.SetupConnection();
            if(Connection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Disconnected)
            {
                await Connection.Reconnect();
            }
        }

        public void SendKey(int key, bool isKeyboard)
        {
            if(Connection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
            {
                Connection.SendKey(new ServerConnector.KeyData(new ServerConnector.User(appSettings.User.Login, appSettings.User.Password), key, isKeyboard));
            }
        }


        void SendHover(double dx, double dy)
        {
            if(Connection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
            {
                double sens = 5;
                int x = (int)(sens * dx), y = (int)(sens * dy);
                Connection.SendMouse(new ServerConnector.MouseData(new ServerConnector.User(appSettings.User.Login, appSettings.User.Password), x, y));
            }
        }

        void Hover(object sender, PanUpdatedEventArgs e)
        {
            if(e.StatusType == GestureStatus.Started)
            {
                cancelTokenSource = new CancellationTokenSource();
                token = cancelTokenSource.Token;
                x = e.TotalX;
                y = e.TotalY;
                touchWatcher = new Task(() => 
                {
                    double dx = 0;
                    double dy = 0;
                    while(!token.IsCancellationRequested)
                    {
                        if(dx != x && dy != y)
                        {
                            SendHover(x - dx, y - dy);
                        }
                        dx = x;
                        dy = y;
                        
                    }
                    Thread.Sleep(100);
                });
                touchWatcher.Start();
                
            }
            else if(e.StatusType == GestureStatus.Running)
            {
                x = e.TotalX;
                y = e.TotalY;
            }
            else if(e.StatusType == GestureStatus.Completed)
            {
                cancelTokenSource.Cancel();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            mainPage.BackToLogin();
            Connection.Close();
            return true;
        }

        private void Reload_Clicked(object sender, EventArgs e)
        {
            Connection.HubConnection.StopAsync();
        }

        private void Space_Clicked(object sender, EventArgs e)
        {
            SendKey(0x20, true);
        }

        private void Right_Clicked(object sender, EventArgs e)
        {
            SendKey(39, true);
        }

        private void LeftMouse_Pressed(object sender, EventArgs e)
        {
            SendKey(0x0002, false);
        }

        private void LeftMouse_Released(object sender, EventArgs e)
        {
            SendKey(0x0004, false);
        }

        private void RightMouse_Pressed(object sender, EventArgs e)
        {
            SendKey(0x0008, false);
        }

        private void RightMouse_Released(object sender, EventArgs e)
        {
            SendKey(0x0010, false);
        }
        private void Left_Clicked(object sender, EventArgs e)
        {
            SendKey(37, true);
        }
    }
}