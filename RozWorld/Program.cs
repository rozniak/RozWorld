/**
 * Oddmatics.RozWorld.Client.Program -- RozWorld Desktop Version Entry-Point
 *
 * This source-code is part of the client program for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace Oddmatics.RozWorld.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RwClient();

            client.Run(); // This will throw an exception for now
        }
    }
}
