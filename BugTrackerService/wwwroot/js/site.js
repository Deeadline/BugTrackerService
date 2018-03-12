// Write your JavaScript code.


$('#employee').on('click', function () {
    if ($('#employee').prop('checked')) {
        $('#WorkerCardNumber').show();
    } else {
        $('#WorkerCardNumber').hide();
    }
})