using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class Connections
    {
        Dictionary<string, ConnectionInfo> connections;
        Dictionary<User, ConnectionInfo> users;
        public Connections()
        {
            connections = new Dictionary<string, ConnectionInfo>();
            users = new Dictionary<User, ConnectionInfo>();
        }

        public void AddConnection(string connectionID)
        {
            ConnectionInfo connectionInfo = new ConnectionInfo(connectionID);
            connectionInfo.Status = ConnectionInfo.Statuses.Connected;
            connections.Add(connectionID, connectionInfo);
        }

        public void Login(string connectionID, User userInfo)
        {
            ConnectionInfo connectionInfo = FindConnection(connectionID);
            if(connectionInfo != null)
            {
                connectionInfo.UserInfo = userInfo;
                users.Add(userInfo, connectionInfo);
            }
        }

        public ConnectionInfo FindConnection(string connectionID)
        {
            if(connections.ContainsKey(connectionID))
            {
                return connections[connectionID];
            }
            else
            {
                return null;
            }
        }

        public ConnectionInfo FindUser(User userInfo)
        {
            if(users.ContainsKey(userInfo))
            {
                return users[userInfo];
            }
            else
            {
                return null;
            }
        }

        public void RemoveConnections(string connectionID)
        {
            if(connections.ContainsKey(connectionID))
            {
                ConnectionInfo connectionInfo = connections[connectionID];
                if(connectionInfo.UserInfo != null)
                {
                    users.Remove(connectionInfo.UserInfo);
                }
                connections.Remove(connectionID);
            }
        }
    }
}
