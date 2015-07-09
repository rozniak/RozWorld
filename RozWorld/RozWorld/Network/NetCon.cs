//
// RozWorld.Network.NetCon -- RozWorld Network Connections
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using System;
using System.Net;
using System.Net.NetworkInformation;

namespace RozWorld.Network
{
    public static class NetCon
    {
        // Variable to hold the IP to test ping on
        public static IPAddress PingIP;


        // Function to check if the test IP is ping-able
        public static bool IsConnected()
        {
            if (PingIP != null)
            {
                return TryPing(PingIP);
            }
            else
            {
                return false;
            }
        }


        public static bool TryPing(IPAddress ip)
        {
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(ip);

            return reply.Status == IPStatus.Success;
        }
    }
}