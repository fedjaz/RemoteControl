using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class RemoteControlHub : Hub
    {
        Services.Connections Connections;
        public RemoteControlHub(Services.Connections connections)
        {
            Connections = connections;
        }

        public string BeginHandshake()
        {
            Services.ConnectionInfo connectionInfo = Connections.FindConnection(Context.ConnectionId);

            if(connectionInfo != null)
            {
                if(connectionInfo.Status == Services.ConnectionInfo.Statuses.Connected)
                {
                    connectionInfo.Status = Services.ConnectionInfo.Statuses.Handshaking;
                    connectionInfo.Rsa = new Encryption.CustomRsa();
                    return Newtonsoft.Json.JsonConvert.SerializeObject(connectionInfo.Rsa.Public);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public void EndHandshake(byte[] encryptedAESKey)
        {
            Services.ConnectionInfo connectionInfo = Connections.FindConnection(Context.ConnectionId);
            if(connectionInfo != null && connectionInfo.Status == Services.ConnectionInfo.Statuses.Handshaking)
            {
                byte[] decryptedAESKey;
                try
                {
                    decryptedAESKey = connectionInfo.Rsa.Decrypt(encryptedAESKey);
                    connectionInfo.Aes = new Encryption.CustomAes();
                    connectionInfo.Aes.Key = decryptedAESKey;
                    connectionInfo.Status = Services.ConnectionInfo.Statuses.Handshaked;
                }
                catch
                {

                }
            }
        } 

        public void SendKey(byte[] encryptedKey, byte[] iv)
        {
            SendDataToClient<Services.KeyData>("RecieveKey", encryptedKey, iv);
        }

        public void SendMouse(byte[] encryptedMouse, byte[] iv)
        {
            SendDataToClient<Services.KeyData>("RecieveMouse", encryptedMouse, iv);
        }

        void SendDataToClient<T>(string methodName, byte[] encryptedData, byte[] iv) where T : Services.IDataInfo
        {
            Services.ConnectionInfo connectionInfo = Connections.FindConnection(Context.ConnectionId);
            if(connectionInfo != null && connectionInfo.Status == Services.ConnectionInfo.Statuses.Handshaked)
            {
                string json = connectionInfo.Aes.Decrypt(encryptedData, iv);
                T key = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                connectionInfo = Connections.FindUser(key.User);
                if(connectionInfo != null)
                {
                    encryptedData = connectionInfo.Aes.Encrypt(json);
                    Clients.Client(connectionInfo.ConnectionID).SendAsync(methodName, encryptedData, connectionInfo.Aes.IV);
                }
            }
        }

        public void Login(byte[] encryptedUser, byte[] iv)
        {
            Services.ConnectionInfo connectionInfo = Connections.FindConnection(Context.ConnectionId);
            if(connectionInfo != null && connectionInfo.Status == Services.ConnectionInfo.Statuses.Handshaked)
            {
                string json = connectionInfo.Aes.Decrypt(encryptedUser, iv);
                Services.User userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Services.User>(json);
                Connections.Login(Context.ConnectionId, userInfo);
            }
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            Connections.AddConnection(Context.ConnectionId);

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Connections.RemoveConnections(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
