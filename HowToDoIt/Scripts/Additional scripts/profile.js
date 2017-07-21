$(document).ready(function () {
    $('input, textarea').on('change', function () {
        // отправка на сервер
        SendData();
    });
    $('img').on('load', function () {
        SendData();
    });

    function SendData() {
        var object = new Object();


        object.FirtstName = $("#firstName").val();
        object.LastName = $("#lastName").val();
        var inp = document.getElementsByName('optradio');
        for (var i = 0; i < inp.length; i++) {
            if (inp[i].type == "radio" && inp[i].checked) {
                object.Sex = inp[i].id;
            }
        }
        object.City = $("#city").val();
        object.Contacts = $("#contact").val();
        object.Interests = $("#interest").val();
        object.Avatar = $('#profile-avatar').attr('src');
        object.Id = $("#info").val();
        $.ajax({
            type: "POST",
            url: '@Url.Action("SaveProfileData", "Profile")',

            data: object,
            success: function (response) {

            },
        });
    };
});