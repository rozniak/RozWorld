﻿//
// RozWorld.Network.JoiningClient -- RozWorld Joining Client
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using System;
using System.Net;

public class JoiningClient
{
    public readonly DateTime RequestTime;
    public readonly string RequestMPKey;
    public readonly IPAddress RequestIP;


    public JoiningClient(DateTime time, string mpKey, IPAddress ip)
    {
        time = RequestTime;
        RequestMPKey = mpKey;
        RequestIP = ip;
    }
}