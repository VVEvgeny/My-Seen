if (DEBUG) {
    console.log('context work');
}

function send_mem(info) {
    if (DEBUG) {
        console.log('send_mem=');
        console.log(info);
    }

    sendMemToServer(key, info.srcUrl,
        function (result) {
            if (DEBUG) {
                console.log('result=');
                console.log(result);
            }
            // Now create the notification
            chrome.notifications.create('reminder', {
                type: 'basic',
                iconUrl: 'img/icon.png',
                title: 'MySeen message!',
                message: result.Ok ? "Added" : ("Error" + " " + result.text)
            }, function (notificationId) { });
        }
    );
}

function showNotification(storedData) {

}

var key;
chrome.storage.sync.get({ Key: '' }, function(items) {
    key = items.Key;

    chrome.contextMenus.create({
        title: "Add to MySeen.by",
        contexts: ["image"],
        //"contexts": ["page", "selection", "image", "link"],
        onclick: send_mem
    });
});



