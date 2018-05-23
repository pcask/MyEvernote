
var noteId = -1;

$(function () {

    $('#commentModal').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget); // e.relatedTarget hangi nesneye tıklandıysa onu getirir.
        noteId = btn.data("note-id");

        $("#commentModal_body").load("/Comment/ShowNoteComments/" + noteId);
    });

});

function doComment(btn, e, commentId, commentTextId) {

    var button = $(btn);
    var mode = button.data("edit-mode");
    var commentText = $("#" + commentTextId);

    if (e === "edit_clicked") {

        if (!mode) {

            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            var btnIcon = button.find("i");
            btnIcon.removeClass("fa-edit");
            btnIcon.addClass("fa-check");

            commentText.attr("contenteditable", true);
            commentText.addClass("editable");
            commentText.focus();
        }
        else {

            button.data("edit-mode", false);
            button.removeClass("btn-success");
            button.addClass("btn-warning");
            var btnIcon = button.find("i");
            btnIcon.removeClass("fa-check");
            btnIcon.addClass("fa-edit");

            commentText.attr("contenteditable", false);
            commentText.removeClass("editable");

            $.ajax({
                url: "/Comment/Edit/" + commentId,
                method: "POST",
                data: { text: commentText.text() }
            }).done(function (data) {

                if (data.result) {
                    // Yorumlar tekrar yüklenir
                    $("#commentModal_body").load("/Comment/ShowNoteComments/" + noteId);
                }
                else {
                    alert("Yorum güncellenemedi");
                }

            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı. Lütfen tekrar giriş yapınız.");
            });
        }

    }
    else if (e === "delete_clicked") {

        if (!confirm("Yorumu Silmek İstediğinize Emin Misiniz?"))
            return false;

        $.ajax({

            url: "/Comment/Delete/" + commentId,
            method: "GET"
        }).done(function (data) {

            if (data.result) {
                // Yorumlar tekrar yüklenir
                $("#commentModal_body").load("/Comment/ShowNoteComments/" + noteId);
            }
            else {
                alert("Yorum silinemedi");
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı. Lütfen tekrar giriş yapınız.");
        });

    }
    else if (e === "new_clicked") {

        $.ajax({

            url: "/Comment/Create",
            method: "POST",
            data: { text: commentText.val(), "noteId": noteId }
        }).done(function (data) {

            if (data.result) {
                // Yorumlar tekrar yüklenir
                $("#commentModal_body").load("/Comment/ShowNoteComments/" + noteId);
            }
            else {
                alert("Yorum eklenemedi");
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı. Lütfen tekrar giriş yapınız.");
        });

    }

}