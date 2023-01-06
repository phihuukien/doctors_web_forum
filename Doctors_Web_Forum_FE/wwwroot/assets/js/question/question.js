$(document).on('ready', function () {
    $('.btn-post-question').click(function () {
        //  get data from  form into  FormData
        var formData = new FormData();
        formData.append("title", $('#title').val());
        formData.append("topicId", $('#topicId').val());
        formData.append("detail", myEditor.getData());
        //  call ajax insert new question
        $.ajax({
            url: "http://localhost:18039/question/insert",
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                window.location.href = 'http://localhost:18039/question/' + data.id
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
});