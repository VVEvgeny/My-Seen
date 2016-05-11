window.onerror = function(errorMsg, url, lineNumber, column, errorObj) {
    JL("onerrorLogger")
        .fatalException({
                "msg": "Exception!",
                "errorMsg": errorMsg,
                "url": url,
                "line number": lineNumber,
                "column": column
            },
            errorObj);
    return false;
}