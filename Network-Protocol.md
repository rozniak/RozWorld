#RozWorld Network Protocol (or RozNet if you'd like)
This is a basic working draft on the RozWorld networking protocol. Everything in here is in-progress (like the game), it'll develop alongside the server library.

The formatting of this document takes closely from http://minecraft.gamepedia.com/Classic_server_protocol since it's a nicely laid out representation of this stuff.

##Notes
###Data Formats
The *Local Chunk* values are in format: `xx.xxx`

The *Angle* values are in format: `0xxx.x` (Angles are done anticlockwise, with 0 being directly east)

Where the client sends them as whole numbers, the server divides them appropriately into their real decimal values.

For example, if the player moved, and the client sent a *Local Chunk X* value of `13260`, the server would then store that value as `13.26`.

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
        <td>0x0001</td>
        <td>Request Server Info</td>
        <td>Packet ID</td>
        <td>UShort</td>
        <td>Sent by broadcast (for finding LAN games) or directly (for WAN games) for getting server information.</td>
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
        <td rowspan=2>Request Mod Info</td>
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
        <td rowspan>Request Player Position</td>
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
</table>