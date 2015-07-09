//
// RozWorld.Network.ServerOptions -- RozWorld Server Options
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using System.Collections.Generic;

using RozWorld.Network.Chat;

namespace RozWorld.Network
{
    public class ServerOptions
    {
        public int Difficulty
        {
            get { return this.Difficulty; }
            set { if (value >= 0 && value <= 3) { this.Difficulty = value; } }
        }

        public ChatFormatting ChatFormat
        {
            get;
            private set;
        }

        public Dictionary<string, PermissionGroup> PermissionGroups
        {
            get;
            private set;
        }

        private string _DefaultGroup;
        public string DefaultGroup
        {
            get { return this._DefaultGroup; }
            set { if (PermissionGroups.ContainsKey(value)) { this._DefaultGroup = value; } }
        }

        public string MessageOfTheDay;

        private int _MaxClients;
        public int MaxClients
        {
            get { return this._MaxClients; }
            set { if (value >= 0 && value >= ParentServer.GetPlayerCount()) { this.MaxClients = value; } }
        }

        public readonly bool OnlineMode;
        public ushort SkillpointReward;

        private RozServer ParentServer;


        public ServerOptions(RozServer parentServer, bool onlineMode)
        {
            ParentServer = parentServer;
            OnlineMode = onlineMode;
            Difficulty = 2;
            ChatFormat = new ChatFormatting();
            PermissionGroups = new Dictionary<string, PermissionGroup>();
            MessageOfTheDay = "";
            MaxClients = 0;
        }
    }
}
