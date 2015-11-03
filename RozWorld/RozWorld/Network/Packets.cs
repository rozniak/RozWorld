/**
 * RozWorld.Network.Packets -- RozWorld Packet Management
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;


namespace RozWorld.Network
{
    public static class Packets
    {
        // Server --> Client Packet Codes
        public const byte SERVER_PONG_CODE = 2;
        public const byte SERVER_ROZENCODED_CODE = 32;
        public const byte SERVER_BOOL_CODE = 33;
        public const byte SERVER_FLOOR_UPDATE_CODE = 68;
        public const byte SERVER_BLOCK_UPDATE_CODE = 69;
        public const byte SERVER_CHAT_MESSAGE_CODE = 70;
        public const byte SERVER_TELEPORT_CODE = 71;
        public const byte SERVER_STAT_UPDATE_CODE = 72;
        public const byte SERVER_POSITION_UPDATE_CODE = 73;
        public const byte SERVER_FAILURE_CODE = 250;
        public const byte SERVER_LOGIN_CODE = 251;

        // Client --> Server Packet Codes
        public const byte CLIENT_PING_CODE = 2;
        public const byte CLIENT_GET_QUEUE_LENGTH_CODE = 64;
        public const byte CLIENT_GET_NEXT_CODE = 65;
        public const byte CLIENT_GET_COORDS_CODE = 66;
        public const byte CLIENT_GET_TILE_CODE = 67;
        public const byte CLIENT_GET_MOTD_CODE = 100;
        public const byte CLIENT_GET_CLIENTS_CODE = 101;
        public const byte CLIENT_GET_ONLINE_MODE_CODE = 102;
        public const byte CLIENT_LOGIN_CODE = 150;

        // Network Failure Codes
        public const byte FAILURE_ISLIVE_CODE = 0;
        public const byte FAILURE_PING_CODE = 1;
        public const byte FAILURE_NO_CONNECTION_CODE = 2;
        public const byte FAILURE_FULL_SERVER_CODE = 50;
        public const byte FAILURE_WHITELISTED_CODE = 51;
        public const byte FAILURE_INVALID_MPKEY_CODE = 52;
        public const byte FAILURE_BANNED_CODE = 53;
        public const byte FAILURE_DUPE_NICK_CODE = 54;

        // Player Stat Update Codes
        public const byte STAT_HEALTH_CODE = 1;
        public const byte STAT_MAX_HEALTH_CODE = 2;
        public const byte STAT_EXPERIENCE_CODE = 3;
        public const byte STAT_MAX_EXPERIENCE_CODE = 4;
        public const byte STAT_LEVEL_CODE = 5;
        public const byte STAT_SKILLPOINTS_CODE = 6;


        /// <summary>
        /// Constructs a message packet from a message code, byte arrays, and a seperator byte.
        /// </summary>
        /// <param name="data">The array of byte arrays to concatenate.</param>
        /// <param name="messageCode">The packet message code.</param>
        /// <param name="separator">The separator byte value.</param>
        /// <returns>The concatenated message.</returns>
        public static byte[] ConstructMessage(byte[][] data, byte messageCode, byte separator = 71)
        {
            if (data.Length == 0)
            {
                return new byte[] { 0 };
            }

            int messageLength = data.Length - 1;

            for (int i = 0; i <= data.Length - 1; i++)
            {
                messageLength += data[i].Length;
            }

            byte[] message = new byte[messageLength + 1];
            message[0] = messageCode;
            int writeToIndex = 1;

            for (int arrayIndex = 0; arrayIndex <= data.Length - 1; arrayIndex++)
            {
                Array.Copy(data[arrayIndex], 0, message, writeToIndex, data[arrayIndex].Length);

                if (arrayIndex != data.Length - 1)
                {
                    writeToIndex += data[arrayIndex].Length;

                    message[writeToIndex] = separator;
                }
                writeToIndex++;
            }

            return message;
        }
    }
}
