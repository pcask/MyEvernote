

$(function () {
    $("footer").hide();

    var countDown = parseInt($("#counter").text());

    var counterFunc = setInterval(function () {

        if (countDown == 1) {
            clearInterval(counterFunc);
            window.location.href = "/Home/Login";
        }

        countDown--;
        $("#counter").text(countDown);

    }, 1000);

});