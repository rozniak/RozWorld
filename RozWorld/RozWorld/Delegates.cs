/**
 * RozWorld.Delegates -- RozWorld Delegates
 * 
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System.Collections.Generic;


/// <summary>
/// Generic event handler delegate.
/// </summary>
public delegate void GenericEventHandler();


/// <summary>
/// Generic event handler delegate with the caller's object reference.
/// </summary>
public delegate void SenderEventHandler(object sender);


/// <summary>
/// Standard key event handler delegate for key down and up triggers by the client.
/// </summary>
public delegate void KeyEventHandler(object sender, byte key);


/// <summary>
/// Standard command handler delegate for registered commands within plugins and the server.
/// </summary>
/// <param name="sender">The name of the entity sending the command ("SERVER" if sent by the server).</param>
/// <param name="args">The arguments supplied with the command.</param>
public delegate void ServerCommandHandler(string sender, IList<string> args);