$(document).ready(function () {

    var isConnected = $('#isConnected').val();
    var errorMessage = $('#errorMessage').val();

    if (isConnected == "True") {
        $('#dataSnifferInputs').show();
        $('#submitButton').prop("disabled", false);
        $('#reportButton').prop("disabled", false);
        $('#exportButton').prop("disabled", false);
        $('#parameter2').prop("disabled", false);
    };

    if (isConnected == "False") {
        $('#submitButton').prop("disabled", true);
        $('#reportButton').prop("disabled", true);
        $('#exportButton').prop("disabled", true);          
    };
    
    $("#reportButton").click(function () {
        $('#runProcedure').val('false');          

    });
    $("#submitButton").click(function () {
        $('#runProcedure').val('true');
        
    });



 //----------------------------------------------------------------------------------- Data Sniffer Modal
        $(document).on("click", ".modalPopup", function (e) {

        e.preventDefault();
        var _self = $(this);

        var tableName = _self.data('1');
        var alertType = _self.data('2');

        var url = $('#myModal').data('url');
        var serverSelectedName = $('#serverSelectedName').val();
        var databaseSelectedName = $('#databaseSelectedName').val();
 
        $.ajax({
            url: url,
            type: "POST",
            dataType: "html",
            data: { serverSelectedName: serverSelectedName, databaseSelectedName: databaseSelectedName, tableName: tableName, alertType: alertType },
            success: function(data){ 
                $('#modalContent').html(data);
                $('#myModal').modal('show');
            }
        });     
        });


    });