
$(function () {

    var noteIdList = [];

    $("div[data-note-id]").each(function (i, e) {

        noteIdList.push($(e).data("note-id"));
    });

    $.ajax({
        url: "/Note/GetLiked",
        method: "POST",
        data: { "noteIdList": noteIdList }
    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {

            for (var i = 0; i < data.result.length; i++) {

                var id = data.result[i];
                var likedNote = $("div[data-note-id=" + id + "]");

                var btn = likedNote.find("button[data-liked]");
                btn.data("liked", true);

                var icon = btn.find("i.like-icon");
                icon.removeClass("fa-heart-o");
                icon.addClass("fa-heart");
            }

        }

    }).fail(function () {
        alert("Sunucu ile bağlantı kurulamadı. Lütfen tekrar giriş yapınız.");
    });



    $("button[data-liked]").click(function () {

        var btn = $(this);
        var noteId = btn.data("note-id");
        var liked = btn.data("liked");

        var icon = btn.find("i.like-icon");
        var likeCountSpan = btn.find("span.like-count");

        $.ajax({
            url: "/Note/SetLikeStatus",
            method: "POST",
            data: { "noteId": noteId, liking: !liked }
        }).done(function (data) {

            if (!data.hasError) {
                if (liked) {
                    btn.data("liked", false);

                    icon.removeClass("fa-heart");
                    icon.addClass("fa-heart-o");
                }
                else {
                    btn.data("liked", true);

                    icon.removeClass("fa-heart-o");
                    icon.addClass("fa-heart");
                }

                likeCountSpan.text(data.result);
            }
            else {
                alert(data.errorMessage);
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı. Lütfen tekrar giriş yapınız.");
        });

    });


});

