
$(function () {

    $('#noteTextModal').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget); // e.relatedTarget hangi nesneye tıklandıysa onu getirir.
        noteId = btn.data("note-id");

        $("#noteTextModal_body").load("/Note/ShowNoteText/" + noteId);
    });

});

