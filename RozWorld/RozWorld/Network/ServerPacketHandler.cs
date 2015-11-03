/**
 * RozWorld.Network.ServerPacketHandler -- RozWorld Server Packet Handler
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Net;


namespace RozWorld.Network
{
    public class ServerPacketHandler
    {
        public byte[] HandlePacket(byte[] packet, IPAddress ip)
        {
            if (packet.Length > 0)
            {
                switch (packet[0])
                {
                    case Packets.CLIENT_PING_CODE: break; // Handle ping packet
                    case Packets.CLIENT_GET_QUEUE_LENGTH_CODE: break; // Handle retrieve packet queue length
                    case Packets.CLIENT_GET_NEXT_CODE: break; // Handle retrieve next packet in queue
                    case Packets.CLIENT_GET_COORDS_CODE: break; // Handle request for player coords
                    case Packets.CLIENT_GET_TILE_CODE: break; // Handle request for tile data
                    case Packets.CLIENT_GET_MOTD_CODE: break; // Handle request for server MOTD
                    case Packets.CLIENT_GET_CLIENTS_CODE: break; // Handle request for the server's client info (clientsconnected;maxclients)
                    case Packets.CLIENT_GET_ONLINE_MODE_CODE: break; // Handle request for the server's online mode status
                    case Packets.CLIENT_LOGIN_CODE: break; // Handle request for login
                }
            }
            
            return new byte[] { 0 };
        }


        private byte[] HandlePingPacket(byte[] packet, IPAddress ip)
        {
            if (packet.Length > 1)
            {
                // Handle player sent packet here
            }

            return new byte[] { 2 };
        }
    }
}
