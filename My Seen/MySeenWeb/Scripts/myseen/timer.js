///////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////           ТАЙМЕР
///////////////////////////////////////////////////////////////////////
function getTime(value) {
    var estimatedTo = value;
    var years = 0;
    var days = 0;
    var hours = 0;
    var minutes = 0;
    var seconds = 0;
    var isMinus = false;
    if (estimatedTo[0] === '-') { //Накапливаем
        isMinus = true;
        estimatedTo = estimatedTo.slice(2);
    }

    if (estimatedTo.split(":").length === 5) {
        years = estimatedTo.split(":")[0];
        days = estimatedTo.split(":")[1];
        hours = estimatedTo.split(":")[2];
        minutes = estimatedTo.split(":")[3];
        seconds = estimatedTo.split(":")[4];
    }
    if (estimatedTo.split(":").length === 4) {
        days = estimatedTo.split(":")[0];
        hours = estimatedTo.split(":")[1];
        minutes = estimatedTo.split(":")[2];
        seconds = estimatedTo.split(":")[3];
    }
    if (estimatedTo.split(":").length === 3) {
        hours = estimatedTo.split(":")[0];
        minutes = estimatedTo.split(":")[1];
        seconds = estimatedTo.split(":")[2];
    }
    if (estimatedTo.split(":").length === 2) {
        minutes = estimatedTo.split(":")[0];
        seconds = estimatedTo.split(":")[1];
    }
    if (estimatedTo.split(":").length === 1) {
        seconds = estimatedTo.split(":")[0];
    }
    if (years === 0 && days === 0 && hours === 0 && minutes === 0 && (seconds === 0 || seconds === '00')) {
        return $scope.translation.Ready;
    }

    if (isMinus) seconds++;
    else seconds--;

    //скорректировать
    //60 сек => 0 сек +1 мин
    if (seconds >= 60) {
        seconds -= 60;
        minutes++;
    }
    if (seconds < 0) {
        seconds = 59;
        minutes--;
    }

    if (minutes >= 60) {
        minutes -= 60;
        hours++;
    }
    if (minutes < 0) {
        minutes = 59;
        hours--;
    }

    if (hours >= 24) {
        hours -= 24;
        days++;
    }
    if (hours < 0) {
        hours = 23;
        days--;
    }

    if (days >= 365) {
        days -= 365;
        years++;
    }

    return (isMinus === true ? "- " : "")
        + (years === 0 ? "" : (years + ":"))
        + (years === 0 && days === 0 ? "" : (days + ":"))
        + (years === 0 && days === 0 && hours === 0 ? "" : (hours.toString().length < 2 ? ("0" + hours + ":") : (hours + ":")))
        + (years === 0 && days === 0 && hours === 0 && minutes === 0 ? "" : (minutes.toString().length < 2 ? ("0" + minutes + ":") : (minutes + ":")))
        + (seconds < 10 ? "0" + seconds : seconds);
};