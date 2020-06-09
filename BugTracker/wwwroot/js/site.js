// Write your JavaScript code.

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

Date.prototype.toShortDateString = function (){
    var date = new Date(this.valueOf());
    date.toISOString().split('T')[0];
}

const saveComment = function {
    $("#saveButton").on("click", function () {
        var commentText = $("#newComment").val().trim();
        var count = commentText.length;
        if (count !== 0) {
            var vm = {
                bugId: "@Model.Bug.Id",
                text: commentText,
                submittedBy: "@Model.UserName"
            };

            bootbox.confirm("Save Comment?", function (result) {
                if (result) {
                    $.ajax({
                        url: "/api/comments",
                        method: "POST",
                        data: JSON.stringify(vm),
                        dataType: "json",
                        contentType: "application/json",
                        success: function (data) {
                            var now = Date.now();
                            var date = new Date(now);
                            var offset = date.getTimezoneOffset();
                            date = new Date(date.getTime() + (offset * 60 * 1000));
                            date = date.toISOString().split('T')[0];

                            var newCommentHtml =
                                "<div class='card border-secondary mb-4 ml-3' style='max-width: 30rem'>"
                                + "<div class='card-header py-0 delete-icon-container'>"
                                + "<span class='text-muted mt-2'>" + date + "</span>"
                                + "<span class='float-right'>"
                                + "<button id='deleteButton' data-comment-id='" + data.id + "' class='js-delete m-0 btn btn-link'>"
                                + "<img src='/lib/images/delete-bin-32.png' class='delete-comment-icon' />"
                                + "</button>"
                                + "</span>"
                                + "</div>"
                                + "<div class='card-body'>"
                                + "<b class='card-title'>" + vm.submittedBy + "</b>"
                                + "<p class='card-text'>" + $.trim($("#newComment").val()) + "</p>"
                                + "</div>"
                                + "</div>";

                            $("#comments").append(newCommentHtml);
                            document.getElementById("newComment").value = "";
                        },
                        error: function () {
                            bootbox.alert("Comment Posting Failed")
                        }
                    });
                }
            });
        }
    }
}

