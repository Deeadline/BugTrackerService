// Write your JavaScript code.
$("#Employee").on("click", function () {
    if ($("#Employee").prop("checked")) {
        $("#WorkerCardNumber").show();
    } else {
        $("#WorkerCardNumber").hide();
    }
})