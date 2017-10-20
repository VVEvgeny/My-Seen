// Saves options to chrome.storage
function save_options() {
    chrome.storage.sync.set({
        Key: document.getElementById('key').value
    }, function () {
        // Update status to let user know options were saved.
        var status = document.getElementById('status');
        status.textContent = 'Options saved.';
        setTimeout(function () {
            status.textContent = '';
        }, 750);
    });
}
// Restores select box and checkbox state using the preferences
// stored in chrome.storage.
function restore_options() {
    chrome.storage.sync.get({
        Key: ''
    }, function (items) {
        document.getElementById('key').value = items.Key;
    });
}

//result 
//result.Ok - true/false
//result.text - text
function check_key() {
    checkUser(document.getElementById('key').value,
        function (result) {
            var status = document.getElementById('status');
            status.textContent = (result.Ok ? "OK" : "ERROR") + " " + result.text;
            setTimeout(function () {
                status.textContent = '';
            }, 2000);
        }
    );
}


document.addEventListener('DOMContentLoaded', restore_options);
document.getElementById('save').addEventListener('click', save_options);
document.getElementById('check').addEventListener('click', check_key);
