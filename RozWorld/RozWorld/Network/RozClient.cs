/**
 * RozWorld.Network.RozClient -- RozWorld Client
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

/**
 * [ NOTICE! ]
 * 
 * This code is subject to massive changes, do not expect a lot of the established code in here to remain as is!
 */

using RozWorld.IO;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;


namespace RozWorld.Network
{
    public class RozClient
    {
        public const double CLIENT_VERSION = 0.1;
        public const string CLIENT_IMPLEMENTATION = "RozWorld Vanilla";

        // Tell the game if this is live or not
        public readonly bool IsLive;

        // Authentication variables
        public readonly string AuthNick;
        private string AuthPass;
        private string AuthSession;
        private bool AuthOnlineMode;
        private string AuthMPKey;

        // Network client and ports
        private int NetPort;
        private UdpClient NetClient;
        private IPAddress NetServerIP;
        private int NetServerPort;
        private IPEndPoint NetServerEndPoint;
        private Timer NetPingDaemon;

        private bool IsConnected;
        

        public RozClient(string nick, string pass, string session)
        {
            if (nick.Length > 0 && nick.Length <= 16 && pass.Length > 0 && pass.Length <= 20 && session.Length == 20)
            {
                AuthNick = nick;
                AuthPass = pass;
                AuthSession = session;
                AuthOnlineMode = true;
                AuthMPKey = "";
                NetPingDaemon = new Timer(10);

                bool successfulPort = false;

                do
                {
                    //NetPort = Globals.Rand.Next(49152, 65536);
                    try
                    {
                        NetClient = new UdpClient(NetPort);
                        successfulPort = true;
                    }
                    catch
                    {
                        // Do nothing
                    }
                } while (!successfulPort);

                IsLive = true;
            }
        }

        public RozClient(string nick)
        {
            if (nick.Length >= 0 && nick.Length <= 16)
            {
                AuthNick = nick;
                AuthPass = "";
                AuthSession = "";
                AuthOnlineMode = false;
                AuthMPKey = "";
                NetPingDaemon = new Timer(10);

                bool successfulPort = false;

                do
                {
                    //NetPort = Globals.Rand.Next(49152, 65536);
                    try
                    {
                        NetClient = new UdpClient(NetPort);
                        successfulPort = true;
                    }
                    catch
                    {
                        // Do nothing
                    }
                } while (!successfulPort);

                IsLive = true;
            }
        }


        /// <summary>
        /// Retrieves the message of the day from a server with the specified IP and port.
        /// </summary>
        /// <param name="targetIP">The IP of the server.</param>
        /// <param name="targetPort">The port on the server.</param>
        /// <returns>The message of the day if the server responded with it.</returns>
        public string RetrieveServerMOTD(IPAddress targetIP, int targetPort)
        {
            byte[] messageResponse = SendRawMessageRemote(new byte[] { Packets.CLIENT_GET_MOTD_CODE }, targetIP, targetPort);

            if (messageResponse[0] == Packets.SERVER_ROZENCODED_CODE)
            {
                byte[] charBytes = new byte[messageResponse.Length - 1];

                Array.Copy(messageResponse, 1, charBytes, 0, messageResponse.Length - 1);

                return RozEncoding.GetString(charBytes);
            }
            else
            {
                return "Failure retrieving server MOTD.";
            }
        }


        /// <summary>
        /// Retrieves the client information from a server with the specified IP and port.
        /// </summary>
        /// <param name="targetIP">The IP of the server.</param>
        /// <param name="targetPort">The port on the server.</param>
        /// <returns>The client information if the server responded with it.</returns>
        public int[] RetrieveServerClientInfo(IPAddress targetIP, int targetPort)
        {
            byte[] messageResponse = SendRawMessageRemote(new byte[] { Packets.CLIENT_GET_CLIENTS_CODE }, targetIP, targetPort);

            if (messageResponse[0] == Packets.SERVER_ROZENCODED_CODE)
            {
                byte[] charBytes = new byte[messageResponse.Length - 1];

                Array.Copy(messageResponse, 1, charBytes, 0, messageResponse.Length - 1);

                string clientInfoString = RozEncoding.GetString(charBytes);

                int maxClients = 0;
                int connectedClients = 0;
                bool successfulParse = false;

                if (clientInfoString.Split(';').Length == 2)
                {
                    successfulParse = int.TryParse(clientInfoString.Split(';')[0], out connectedClients);
                    successfulParse = int.TryParse(clientInfoString.Split(';')[1], out connectedClients);
                }

                if (successfulParse)
                {
                    return new int[] { connectedClients, maxClients };
                }
                else
                {
                    return new int[] { 0, 0 };
                }                
            }
            else
            {
                return new int[] { 0, 0 };
            }
        }


        /// <summary>
        /// Attempts to connect to a server with the specified IP and port.
        /// </summary>
        /// <param name="serverIP">The IP of the server.</param>
        /// <param name="serverPort">The port on the server.</param>
        /// <returns>The connection result relevant to whether the connection was established or not.</returns>
        public ConnectResult Connect(IPAddress serverIP, int serverPort)
        {
            byte[] serverResponse = SendRawMessageRemote(new byte[] { Packets.CLIENT_GET_ONLINE_MODE_CODE }, serverIP, serverPort);

            if (serverResponse[0] == Packets.SERVER_BOOL_CODE)
            {
                bool onlineMode = serverResponse[1] == 1;

                byte[] nickBytes = RozEncoding.GetBytes(AuthNick);

                if (onlineMode && AuthOnlineMode)
                {
                    byte[] mpKeyBytes = RozEncoding.GetBytes(AuthMPKey);
                    
                    serverResponse = SendRawMessageRemote(Packets.ConstructMessage(new byte[][] { nickBytes, mpKeyBytes }, Packets.CLIENT_LOGIN_CODE), serverIP, serverPort);         

                    if (serverResponse[0] == Packets.SERVER_LOGIN_CODE)
                    {
                        NetServerIP = serverIP;
                        NetServerPort = serverPort;
                        NetServerEndPoint = new IPEndPoint(NetServerIP, NetServerPort);

                        IsConnected = true;
                        return ConnectResult.Success;
                    }
                    else
                    {
                        IsConnected = false;

                        switch (serverResponse[1])
                        {
                            case Packets.FAILURE_FULL_SERVER_CODE: return ConnectResult.ServerFull;
                            case Packets.FAILURE_WHITELISTED_CODE: return ConnectResult.NotWhitelisted;
                            case Packets.FAILURE_INVALID_MPKEY_CODE: return ConnectResult.InvalidMPKey;
                            case Packets.FAILURE_BANNED_CODE: return ConnectResult.Banned;
                            case Packets.FAILURE_DUPE_NICK_CODE: return ConnectResult.DuplicateNick;
                            default: return ConnectResult.Unknown;
                        }
                    }
                }
                else if (!onlineMode)
                {
                    serverResponse = SendRawMessageRemote(Packets.ConstructMessage(new byte[][] { nickBytes }, Packets.CLIENT_LOGIN_CODE), serverIP, serverPort);

                    if (serverResponse[0] == Packets.SERVER_LOGIN_CODE)
                    {
                        NetServerIP = serverIP;
                        NetServerPort = serverPort;
                        NetServerEndPoint = new IPEndPoint(NetServerIP, NetServerPort);

                        IsConnected = true;
                        return ConnectResult.Success;
                    }
                    else
                    {
                        IsConnected = false;

                        switch (serverResponse[1])
                        {
                            case Packets.FAILURE_FULL_SERVER_CODE: return ConnectResult.ServerFull;
                            case Packets.FAILURE_WHITELISTED_CODE: return ConnectResult.NotWhitelisted;
                            case Packets.FAILURE_INVALID_MPKEY_CODE: return ConnectResult.InvalidMPKey;
                            case Packets.FAILURE_BANNED_CODE: return ConnectResult.Banned;
                            case Packets.FAILURE_DUPE_NICK_CODE: return ConnectResult.DuplicateNick;
                            default: return ConnectResult.Unknown;
                        }
                    }
                }
                else
                {
                    IsConnected = false;
                    return ConnectResult.MustBeAuthorised;
                }
            }
            else if (serverResponse[0] == Packets.SERVER_FAILURE_CODE)
            {
                IsConnected = false;

                switch (serverResponse[1])
                {
                    case Packets.FAILURE_ISLIVE_CODE: return ConnectResult.IsLiveFalse;
                    case Packets.FAILURE_PING_CODE: return ConnectResult.PingFailure;
                    default: return ConnectResult.Unknown;
                }
            }

            return ConnectResult.Unknown;
        }


        /// <summary>
        /// Sends a raw message to the connected server.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>The response packet from the server.</returns>
        public byte[] SendRawMessage(byte[] message)
        {
            if (IsLive)
            {
                if (NetCon.TryPing(NetServerIP))
                {
                    if (IsConnected)
                    {
                        NetClient.Send(message, message.Length, NetServerEndPoint);

                        return NetClient.Receive(ref NetServerEndPoint);
                    }

                    return new byte[] { Packets.SERVER_FAILURE_CODE, Packets.FAILURE_NO_CONNECTION_CODE };
                }

                return new byte[] { Packets.SERVER_FAILURE_CODE, Packets.FAILURE_PING_CODE };
            }

            return new byte[] { Packets.SERVER_FAILURE_CODE, Packets.FAILURE_ISLIVE_CODE };
        }


        /// <summary>
        /// Sends a raw message to a remote server.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="targetIP">The IP of the server.</param>
        /// <param name="targetPort">The port on the server.</param>
        /// <returns>The response packet from the server.</returns>
        public byte[] SendRawMessageRemote(byte[] message, IPAddress targetIP, int targetPort)
        {
            if (IsLive)
            {
                if (NetCon.TryPing(targetIP))
                {
                    IPEndPoint serverEndPoint = new IPEndPoint(targetIP, targetPort);
                    NetClient.Send(message, message.Length, serverEndPoint);

                    return NetClient.Receive(ref serverEndPoint);
                }
                
                return new byte[] { Packets.SERVER_FAILURE_CODE, Packets.FAILURE_PING_CODE };
            }

            return new byte[] { Packets.SERVER_FAILURE_CODE, Packets.FAILURE_ISLIVE_CODE };
        }
    }
}
