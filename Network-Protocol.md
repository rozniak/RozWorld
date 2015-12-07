#RozWorld Network Protocol (or RozNet if you'd like)
This is a basic working draft on the RozWorld networking protocol. Everything in here is in-progress (like the game), it'll develop alongside the server library.

The formatting of this document takes closely from http://minecraft.gamepedia.com/Classic_server_protocol since it's a nicely laid out representation of this stuff.

##Notes
###Data Formats
The *Local Chunk* values are in format: `xx.xxx`

The *Angle* values are in format: `0xxx.x` (Angles are done anticlockwise, with 0 being directly east)

Where the client sends them as whole numbers, the server divides them appropriately into their real decimal values.

For example, if the player moved, and the client sent a *Local Chunk X* value of `13260`, the server would then store that value as `13.26`.

###Log-In Response Codes
<table>
    <tr>
        <th>Response Code</th>
        <th>Meaning</th>
    </tr>

    <tr>
        <td>0x00</td>
        <td>General Success</td>
    </tr>

    <tr>
        <td>0x01</td>
        <td>Invalid Credentials</td>
    </tr>

    <tr>
        <td>0x02</td>
        <td>Invalid Server Password</td>
    </tr>

    <tr>
        <td>0x03</td>
        <td>Banned</td>
    </tr>

    <tr>
        <td>0x04</td>
        <td>Not on Whitelist</td>
    </tr>

    <tr>
        <td>0x05</td>
        <td>Compatibility Mismatch (Implementation)</td>
    </tr>

    <tr>
        <td>0x06</td>
        <td>Compatibility Mismatch (Client Version)</td>
    </tr>

    <tr>
        <td>0x07</td>
        <td>Server Maintenance</td>
    </tr>

    <tr>
        <td>0x08</td>
        <td>Server Full</td>
    </tr>

    <tr>
        <td>0xFF</td>
        <td>Unspecified Denial</td>
    </tr>
</table>

##Client to Server Packets

<table>
    <tr>
        <th>Packet ID</th>
        <th>Purpose</th>
        <th>Field Description</th>
        <th>Field Type</th>
        <th>Notes</th>
    </tr>


    <tr>
        <td rowspan=2>0x0000</td>
        <td rowspan=2>Ping</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Sent to maintain connection every 500ms</td>
    </tr>
    <tr>
        <td>Ping Datetime</td>
        <td>String</td>
    </tr>


    <tr>
        <td rowspan=2>0x0001</td>
        <td rowspan=2>Request Server Info</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Sent by broadcast (for finding LAN games) or directly (for WAN games) for getting server information.<br><br>Client implementation should be <code>0xFF</code> for vanilla, if it's anything else, a string value may be sent after this byte for further identification.</td>
    </tr>
    <tr>
        <td>Client Implementation</td>
        <td>Byte</td>
    </tr>


    <tr>
        <td rowspan=5>0x0002</td>
        <td rowspan=5>Player Log In</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=5>Sent to notify and authorise a log in attempt onto a player account on the server.</td>
    </tr>
    <tr>
        <td>Username</td>
        <td>String</td>
    </tr>
    <tr>
        <td>Password (Hashed)</td>
        <td>String</td>
    </tr>
    <tr>
        <td>Server Password (Hashed)</td>
        <td>String</td>
    </tr>
    <tr>
        <td>Hash Datetime</td>
        <td>String</td>
    </tr>


    <tr>
        <td>0x0003</td>
        <td>Request Mod Status/Count</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td>Sent when the client joins the server and needs to know if there are any mods/how many mods there are.</td>
    </tr>


    <tr>
        <td rowspan=2>0x0004</td>
        <td rowspan=2>Request Mod Verification</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Sent when the client is verifying mods, each mod it checks it will send this packet asking the server for the next mod's information.</td>
    </tr>
    <tr>
        <td>Mod Number</td>
        <td>UShort</td>
    </tr>


    <tr>
        <td rowspan=2>0x0005</td>
        <td rowspan=2>Request Mod Details</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Sent when the client doesn't have a mod, and will request the name/URL of said mod (only viewable upon request with warning about exploitation).</td>
    </tr>
    <tr>
        <td>Mod Number</td>
        <td>UShort</td>
    </tr>


    <tr>
        <td rowspan=2>0x0006</td>
        <td rowspan=2>Request Item ID Update</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Sent when the client is joining, so that the server can send a short as to refer to an item's ID. This packet makes sure the client has its item IDs as the same ones as the server does.</td>
    </tr>
    <tr>
        <td>Item Name</td>
        <td>String</td>
    </tr>


    <tr>
        <td rowspan>0x0007</td>
        <td rowspan>Request Player Information</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td>Sent when the client is joining, in order to tell where the player is.</td>
    </tr>

    <tr>
        <td rowspan=3>0x0008</td>
        <td rowspan=3>Request Chunk Data</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=3>Sent whenever the client needs to load more chunk information as the player moves through the world, or when they join the server.</td>
    </tr>
    <tr>
        <td>Chunk X</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Chunk Y</td>
        <td>Integer</td>
    </tr>


    <tr>
        <td>0x0009</td>
        <td>Request Playerlist</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td>Sent when the client is joining in order to obtain the full list of players currently on the server.</td>
    </tr>


    <tr>
        <td>0x000A</td>
        <td>Request Quick Inventory</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td>Sent whenever the client loads into the game or otherwise needs to know their quick inventory.</td>
    </tr>


    <tr>
        <td>0x000B</td>
        <td>Request Full Inventory</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td>Sent whenever the player views their full inventory (a way of verifying they know what they have).</td>
    </tr>


    <tr>
        <td rowspan=6>0x000C</td>
        <td rowspan=6>Player Move</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=6>Sent whenever the player starts moving in a direction, and every 500ms whilst moving.</td>
    </tr>
    <tr>
        <td>Local Chunk X</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Local Chunk Y</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Chunk X</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Chunk Y</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Angle (Degrees)</td>
        <td>Integer</td>
    </tr>


    <tr>
        <td rowspan=3>0x000D</td>
        <td rowspan=3>Change Selected Item</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=3>Sent when the player switches item in their quick inventory.</td>
    </tr>
    <tr>
        <td>Slot Number</td>
        <td>Byte</td>
    </tr>
    <tr>
        <td>Expected Item ID</td>
        <td>UShort</td>
    </tr>


    <tr>
        <td rowspan=3>0x000E</td>
        <td rowspan=3>Swap Inventory Slots</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=3>Sent when the player swaps the positions of items in their inventory.</td>
    </tr>
    <tr>
        <td>Initial Slot</td>
        <td>Byte</td>
    </tr>
    <tr>
        <td>Target Slot</td>
        <td>Byte</td>
    </tr>


    <tr>
        <td>0x000F</td>
        <td>Use Item/Consumable</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td>Sent when the player uses an item or consumable that functions on them.</td>
    </tr>


    <tr>
        <td rowspan=5>0x0010</td>
        <td rowspan=5>Use Item on World</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=5>Sent when the player uses an item that functions on a target in the world.</td>
    </tr>
    <tr>
        <td>Local Chunk X</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Local Chunk Y</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Chunk X</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Chunk Y</td>
        <td>Integer</td>
    </tr>


    <tr>
        <td rowspan=2>0x0011</td>
        <td rowspan=2>Drop Item</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Sent when the player attempts to drop an item in the world.</td>
    </tr>
    <tr>
        <td>Angle (Degrees)</td>
        <td>Integer</td>
    </tr>


    <tr>
        <td rowspan=5>0x0012</td>
        <td rowspan=5>Set Tile</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=5>Sent when the player attempts to set a tile in the world.</td>
    </tr>
    <tr>
        <td>Local Chunk X</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Local Chunk Y</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Chunk X</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Chunk Y</td>
        <td>Integer</td>
    </tr>


    <tr>
        <td rowspan=5>0x0013</td>
        <td rowspan=5>Set Tile Object</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=5>Sent when the player attempts to set a tile object in the world.</td>
    </tr>
    <tr>
        <td>Local Chunk X</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Local Chunk Y</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Chunk X</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Chunk Y</td>
        <td>Integer</td>
    </tr>


    <tr>
        <td rowspan=6>0x0014</td>
        <td rowspan=6>Set Tile (Absolute)</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=6>Sent when the player attempts to set a tile in the world whilst in omnipotent (infinite build/godmode) mode.</td>
    </tr>
    <tr>
        <td>Local Chunk X</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Local Chunk Y</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Chunk X</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Chunk Y</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Item ID</td>
        <td>UShort</td>
    </tr>


    <tr>
        <td rowspan=6>0x0015</td>
        <td rowspan=6>Set Tile Object (Absolute)</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=6>Sent when the player attempts to set a tile object in the world whilst in omnipotent (infinite build/godmode) mode.</td>
    </tr>
    <tr>
        <td>Local Chunk X</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Local Chunk Y</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Chunk X</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Chunk Y</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Item ID</td>
        <td>UShort</td>
    </tr>


    <tr>
        <td rowspan=5>0x0016</td>
        <td rowspan=5>Player Attack</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=5>Sent whenever the player attempts to perform an attack.</td>
    </tr>
    <tr>
        <td>Local Chunk X</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Local Chunk Y</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Chunk X</td>
        <td>Integer</td>
    </tr>
    <tr>
        <td>Chunk Y</td>
        <td>Integer</td>
    </tr>


    <tr>
        <td rowspan=2>0x0017</td>
        <td rowspan=2>Chat Message</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Sent whenever the player sends a chat message or command within the chat.</td>
    </tr>
    <tr>
        <td>Message</td>
        <td>String</td>
    </tr>
    
    
    <tr>
        <td>0x0018</td>
        <td>Abandon Pet</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td>Sent whenever the player decides to abandon their current pet.</td>
    </tr>
    
    
    <tr>
        <td rowspan=3>0x0019</td>
        <td rowspan=3>Set Pet Entity Target</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=3>Sent whenever the player sets a target on an entity for their pet to attack.</td>
    </tr>
    <tr>
        <td>Entity Instance ID</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Generated Verify ID</td>
        <td>UShort</td>
    </tr>


    <tr>
        <td rowspan=2>0xFFFF</td>
        <td rowspan=2>Verify Important Packet</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Sent whenever an important packet is recieved in order to ensure that the server knows it was recieved.</td>
    </tr>
    <tr>
        <td>Confirm Verify ID</td>
        <td>UShort</td>
    </tr>
</table>

##Server to Client Packets

<table>
    <tr>
        <th>Packet ID</th>
        <th>Purpose</th>
        <th>Verifies?</th>
        <th>Drop on Update?</th>
        <th>Field Description</th>
        <th>Field Type</th>
        <th>Notes</th>
    </tr>


    <tr>
        <td rowspan=2>0x0000</td>
        <td rowspan=2>Pong</td>
        <td rowspan=2>No</td>
        <td rowspan=2>No</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Response to client ping to keep the connection alive.</td>
    </tr>
    <tr>
        <td>Pong Datetime</td>
        <td>String</td>
    </tr>
    
    
    <tr>
        <td rowspan=7>0x0001</td>
        <td rowspan=7>Respond Server Info</td>
        <td rowspan=7>No</td>
        <td rowspan=7>No</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=7>Response to client for the server's information.<br><br>Server implementation should be <code>0xFF</code> for vanilla, if it's anything else, a string value may be sent after this byte for further identification.<br><br>Restriction type is <code>0x00</code> for <code>no restriction</code>, <code>0x01</code> for <code>whitelist</code>, <code>0x02</code> for <code>password</code>, and <code>0x03</code> for <code>password and whitelist</code>.</td>
    </tr>
    <tr>
        <td>Name</td>
        <td>String</td>
    </tr>
    <tr>
        <td>Current Players</td>
        <td>Byte</td>
    </tr>
    <tr>
        <td>Max Players</td>
        <td>Byte</td>
    </tr>
    <tr>
        <td>Server Implementation</td>
        <td>Byte</td>
    </tr>
    <tr>
        <td>Vanilla Compatible Version</td>
        <td>Byte</td>
    </tr>
    <tr>
        <td>Restriction Type</td>
        <td>Byte</td>
    </tr>
    
    
    <tr>
        <td rowspan=2>0x0002</td>
        <td rowspan=2>Respond Player Log In</td>
        <td rowspan=2>Sort of (Client can request a resend)</td>
        <td rowspan=2>No</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Response to the client's attempt at logging in, returning the status of success or reason for failure of the attempt.<br><br>Refer to the <i>Log-In Reponse Codes</i> section of the Notes above.</td>
    </tr>
    <tr>
        <td>Response Code</td>
        <td>Byte</td>
    </tr>


    <tr>
        <td rowspan=2>0x0003</td>
        <td rowspan=2>Respond Mod Status/Count</td>
        <td rowspan=2>Sort of (Client can resend)</td>
        <td rowspan=2>No</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=2>Response to the client's request for the loaded mod count (if any).</td>
    </tr>
    <tr>
        <td>Mod Count</td>
        <td>UShort</td>
    </tr>


    <tr>
        <td rowspan=3>0x0004</td>
        <td rowspan=3>Respond Mod Verification</td>
        <td rowspan=3>Sort of (Client can resend)</td>
        <td rowspan=3>No</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=3>Response to the client's request for a mod's verifiable details.</td>
    </tr>
    <tr>
        <td>Mod Number</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Mod File Hash</td>
        <td>String</td>
    </tr>


    <tr>
        <td rowspan=4>0x0005</td>
        <td rowspan=4>Respond Mod Details</td>
        <td rowspan=4>Sort of (Client can resend)</td>
        <td rowspan=4>No</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=4>Response to the client's request for a mod's information.</td>
    </tr>
    <tr>
        <td>Mod Number</td>
        <td>UShort</td>
    </tr>
    <tr>
        <td>Name</td>
        <td>String</td>
    </tr>
    <tr>
        <td>URL</td>
        <td>String</td>
    </tr>


    <tr>
        <td rowspan=3>0x0006</td>
        <td rowspan=3>Respond Item ID Update</td>
        <td rowspan=3>Sort of (Client can resend)</td>
        <td rowspan=3>No</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=3>Response to the client's request to sync up item ids.</td>
    </tr>
    <tr>
        <td>Item Name</td>
        <td>String</td>
    </tr>
    <tr>
        <td>Item ID</td>
        <td>UShort</td>
    </tr>


    <tr>
        <td rowspan=3>0x0007</td>
        <td rowspan=3>Respond Player Information</td>
        <td rowspan=3>Yes</td>
        <td rowspan=3>No</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td rowspan=3>Response to the client's request for a mod's verifiable details.</td>
    </tr>
    <tr>
        <td>Player ID</td>
        <td>Byte</td>
    </tr>
    <tr>
        <td>Local</td>
        <td>String</td>
    </tr>
</table>