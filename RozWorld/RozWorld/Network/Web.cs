/**
 * RozWorld.Network.Web -- RozWorld HTTP GET and POST
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
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace RozWorld.Network
{
    public static class Web
    {
        // Web client for handling POST and GETs
        static WebRequest WebRequest;


        // Function for the HTTP GET response
        public static string[] Get(string URL)
        {
            if (NetCon.IsConnected())
            {
                WebRequest = WebRequest.Create(URL);
                WebRequest.Proxy = WebRequest.GetSystemWebProxy();

                List<string> responseList = new List<string>();
                return GetResponseFromStream(WebRequest.GetResponse().GetResponseStream());
            }
            else
            {
                return null;
            }
        }


        // Function for the HTTP POST action and response
        public static string[] Post(string URL, string postArgs)
        {
            if (NetCon.IsConnected())
            {
                byte[] postData = Encoding.ASCII.GetBytes(postArgs);

                WebRequest = WebRequest.Create(URL);
                WebRequest.Proxy = WebRequest.GetSystemWebProxy();
                WebRequest.Method = "POST";
                WebRequest.ContentType = "application/x-www-form-urlencoded";
                WebRequest.ContentLength = postData.Length;

                using (Stream w = WebRequest.GetRequestStream())
                {
                    w.Write(postData, 0, postData.Length);
                }

                return GetResponseFromStream(WebRequest.GetResponse().GetResponseStream());
            }
            else
            {
                return null;
            }
        }


        // Function for returning the HTTP response stream into an array (tidying)
        private static string[] GetResponseFromStream(Stream responseStream)
        {
            List<string> ResponseList = new List<string>();

            using (StreamReader r = new StreamReader(responseStream))
            {
                do
                {
                    ResponseList.Add(r.ReadLine());
                } while (r.Peek() > -1);
            }

            return ResponseList.ToArray();
        }
    }
}