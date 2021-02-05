var websocket = require('ws');

var callbackinitServer = () =>
{
    console.log("Server is running");
}
var wss = new websocket.Server({ port: 65222 }, callbackinitServer);
var wslist = [];


wss.on("connection", (ws) =>
{
    console.log("Client connected. ");
    wslist.push(ws);

    ws.on("message" , (data) =>
    {
        
        for (var i = 0; i < wslist.length; i++)
        {
            if(wslist[i] == ws)
            {
                wslist[i].send(data);
                console.log("Send from client :" + data);
            }
            else
            {
                wslist[i].send(data+"                                                  ");
                
            }
        }
    });

    ws.on("close", () =>
    {
        console.log("Client disconnected. ");
    })
});
    function Arrayremove(arr, value) 
    {
    return arr.filter((element) => 
    {
        return element != value;
    });
}
/*function Broadcast(data)
{
    for (var i = 0; i < wslist.length; i++)
    {
        wslist[i].send(data);
    }
}*/








