$(document).ready(function () {

    var isConnected = $('#isConnected').val();

    if (isConnected == "True") {
        $('#parameter2').prop("disabled", false);
        $('#parameter3').prop("disabled", false);
        $('#parameter4').prop("disabled", false);
        $('#submitButton').prop("disabled", false);     
    };

    if (isConnected == "False") {
        $('#parameter2').prop("disabled", true);
        $('#parameter3').prop("disabled", true);
        $('#parameter4').prop("disabled", true);
        $('#submitButton').prop("disabled", true);
    };    

});