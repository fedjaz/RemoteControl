using Encryption;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace ServerConnector
{
    public class Connection
    {
        public HubConnection HubConnection;
        CustomAes aes;
        CustomRsa rsa;
        public delegate void RecievedKeyArgs(Connection sender, KeyData keyData);
        public event RecievedKeyArgs RecievedKey;
        public delegate void RecievedMouseArgs(Connection sender, MouseData mouseData);
        public event RecievedMouseArgs RecievedMouse;
        public delegate void ConnectedArgs();
        public event ConnectedArgs Connected;
        public event ConnectedArgs ConnectionLost;
        CancellationTokenSource ReconnectToken;
        bool closed;
        public HubConnectionState State { get => HubConnection.State; }

        public Connection(string URI)
        {
            HubConnection = new HubConnectionBuilder().WithUrl(URI).Build();
            HubConnection.On<byte[], byte[]>("RecieveKey", (encrypted, iv) => RecieveKey(encrypted, iv));
            HubConnection.On<byte[], byte[]>("RecieveMouse", (encrypted, iv) => RecieveMouse(encrypted, iv));
            rsa = new CustomRsa();
            aes = new CustomAes();
            HubConnection.Closed += Closed;
        }

        async Task Closed(Exception ex)
        {
            ConnectionLost?.Invoke();
            if(!closed)
            {
                await Reconnect();
            }
            
        }

        public async Task Reconnect()
        {
            ReconnectToken?.Cancel();
            ReconnectToken = new CancellationTokenSource();
            CancellationToken token = ReconnectToken.Token;
            while(!await SetupConnection() && !token.IsCancellationRequested)
            {
                Thread.Sleep(1000);
            }
        }

        public void Close()
        {
            closed = true;
            ReconnectToken?.Cancel();
            HubConnection.StopAsync();
        }

        public async Task<bool> SetupConnection()
        {
            if(HubConnection.State == HubConnectionState.Disconnected && !await AttempToConnect(3000))
            {
                return false;
            }
            string json = await HubConnection.InvokeAsync<string>("BeginHandshake");
            rsa.Public = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(json);

            byte[] encryptedAESKey = rsa.Encrypt(aes.Key);

            await HubConnection.InvokeAsync("EndHandshake", encryptedAESKey);
            Connected?.Invoke();
            return true;
        }

        public async Task<bool> AttempToConnect(int ms)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            new Task(() =>
            {
                Thread.Sleep(ms);
                cancelTokenSource.Cancel();
            }).Start();

            try
            {
                await HubConnection.StartAsync(token);
            }
            catch(Exception ex)
            {
                return false;
            }
            

            return true;
        }

        public async Task SendKey(KeyData keyData)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(keyData);
            byte[] encrypted = aes.Encrypt(json);

            await HubConnection.InvokeAsync("SendKey", encrypted, aes.IV);
        }

        public async Task SendMouse(MouseData mouseData)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(mouseData);
            byte[] encrypted = aes.Encrypt(json);

            await HubConnection.InvokeAsync("SendMouse", encrypted, aes.IV);
        }

        public async Task Login(User user)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            byte[] encrypted = aes.Encrypt(json);

            await HubConnection.InvokeAsync("Login", encrypted, aes.IV);
        }

        public void RecieveKey(byte[] encrypted, byte[] iv)
        {
            string json = aes.Decrypt(encrypted, iv);
            KeyData keyData = Newtonsoft.Json.JsonConvert.DeserializeObject<KeyData>(json);

            RecievedKey?.Invoke(this, keyData);
        }

        public void RecieveMouse(byte[] encrypted, byte[] iv)
        {
            string json = aes.Decrypt(encrypted, iv);
            MouseData mouseData = Newtonsoft.Json.JsonConvert.DeserializeObject<MouseData>(json);

            RecievedMouse?.Invoke(this, mouseData);
        }
    }
}