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

| **Packet ID** | **Purpose** | **Field Description** | **Field Type ** | **Notes**                                |
| ------------- | ----------- | --------------------- | --------------- | ---------------------------------------- |
| 0x0000        | Ping        | Packet ID             | Byte            | Sent to maintain connection every 500ms. |