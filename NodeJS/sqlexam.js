const sqlite3 = require('sqlite3').verbose();

let db = new sqlite3.Database('./database/chatDB.db', sqlite3.OPEN_CREATE | sqlite3.OPEN_READWRITE,(err)=>
{
    if(err) throw err;

    console.log('Connected');

    var datafromClient = {
        eventName:"Login",
        data:"test5#33333#test5"
    }
    
    var splitStr = datafromClient.data.split('#');
    var userID = splitStr[0];
    var password = splitStr[1];
    var name = splitStr[2];
    var sqlSelect = "SELECT * FROM UserData WHERE UserID='"+userID+"' AND Password='"+password+"'";//Login
    var sqlInsert = "INSERT INTO UserData (UserID, Password, Name) VALUES ('"+userID+"','"+password+"','"+name+"')";


    db.all(sqlInsert,(err,rows)=>{
        if(err)
        {
            var callbackMsg = {
                eventName:"Register",
                data:"Fail."
             }
             var toJsonStr = JSON.stringify(callbackMsg);
             console.log("[0]" + toJsonStr);
        }
        
            else
            {
                var callbackMsg = {
                    eventName:"Register",
                    data:"Success."
                 }
                 var toJsonStr = JSON.stringify(callbackMsg);
                 console.log("[1]" + toJsonStr);
            }
        
    })


    /*db.all(sqlSelect,(err,rows)=>
    {
        if(err)
        {
            console.log("[0]" + err);
        }
        else
        {
            if(rows.length > 0)
            {
                console.log("[1]");
                console.log(rows);
                console.log("[1]");
                var callbackMsg = {
                   eventName:"Login",
                   data:rows[0].Name
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                console.log("[2]" + toJsonStr);
            }
            else
            {
                var callbackMsg = {
                   eventName:"Login",
                   data:"Fail."
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                console.log("[3]" + toJsonStr);
            }
        }
    });*/
});