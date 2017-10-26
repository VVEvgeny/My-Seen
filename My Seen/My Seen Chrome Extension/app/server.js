var DEBUG = false;

function checkUser(userKey, callback) {
    server("Users/CheckKey",userKey, null, callback);
}
function sendMemToServer(userKey, memUri, callback) {
    server("PortalMemes/AddMem", userKey, memUri, callback);
}

function server(action, userKey, data, callback)
{
    if (DEBUG) {
        console.log(" Server");
        console.log("userKey=" + userKey);
        console.log("data=" + data);
    }

    var xhr = new XMLHttpRequest();
    //var url = "http://localhost:44301/api/" + action + "/?" + "apiVersion=1&userKey=" + userKey;
    var url = "http://myseen.by/api/" + action + "/?" + "apiVersion=1&userKey=" + userKey;

    xhr.open("POST", url, true);
    xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

    if (action === "PortalMemes/AddMem") {
        xhr.send(data);

    } else {
        xhr.send();
    }


    //result 
    //result.Ok - true/false
    //result.text - text

    //async
    xhr.onload = function () {

        if (DEBUG) {
            console.log("onload");
            console.log("status=" + xhr.status);
            console.log("statusText=" + xhr.statusText);
            console.log("responseText=" + xhr.responseText);
        }

        if (callback) {
            var obj = JSON.parse(xhr.responseText);
            callback({ Ok: xhr.status === 200 && obj.Value === 1, text : obj.Data });
        }
    };
    xhr.onerror = function () {
        console.log("error" + " status=" + xhr.status);
        if (callback) {
            var obj = JSON.parse(xhr.responseText);
            callback({ Ok: false, text: obj.Data });
        }
    };
}