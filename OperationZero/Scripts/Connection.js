$(document).ready(function () {

    var isConnected = $('#isConnected').val();
    var numberOfOptions = $('select#DatabaseDropDownList option').length

    $(function () {
            $('#ServerDropDownList').change(function () {
            $('#isConnected').val(false);
            $('#DatabaseDropDownList').prop("disabled", true);          
            $('#DatabaseSelected').empty();            
            this.form.submit();
        });
    });   

    if (isConnected == "True") {
        $('#isConnectedText').show();
        $('#DatabaseDropDownList').prop("disabled", true);
        $('#ServerDropDownList').prop("disabled", true);
        $('#CoreMeasuresMenuLink').prop("disabled", false);
        $('#submitButton').prop("disabled", true);
    };

    if (isConnected == "False") {
        $('#submitButton').show();
        $('#isConnectedText').hide();        
        $('#ServerDropDownList').prop("disabled", false);
        $('#CoreMeasuresMenuLink').prop("disabled", true);
        $('#disconnectButton').prop("disabled", true);
    };

    $(function () {
        $('#disconnectButton').click(function () {
            $('#isConnected').val(false);
            $('#submitButton').show();
            $('#isConnectedText').hide();
            $('#disconnectButton').hide();
            $('#DatabaseDropDownList').prop("disabled", false);
            $('#ServerDropDownList').prop("disabled", false);
            $('#DatabaseDropDownList').val('');
            $('#ServerDropDownList').val('');
            $('#clearSession').val(true);
            this.form.submit();
        });
    }); 
 });