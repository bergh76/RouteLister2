//$.ajax({
//    url: '/api/Posts/GetPosts',
//    method: 'GET',
//    dataType: 'JSON',
//    success: addPostsList
//});

//function addPostsList(posts) {
//    $.each(posts, function (index) {
//        var post = posts[index];
//        addPost(post);
//    });
//}

//function addPost(post) {
//    $("#postsList").append(
//        '<div><span class="pull-left"><i class="fa fa-circle me"></i><strong>' + post.userName + '</strong><span class="text-muted">' + post.time + ' </span></span></span><br>'
//            + '<div class="message-data">'
//                + '<span class="message other-message float-right">' + post.text + '</span>'
//            + ' </div>'
//            + '<span><button class="btn btn-success btn-margin-20">OK</button></span>'
//        + '</div>'
//        );
//}


//signalRClient.client.publishPost = addPost;

//$("#publishPostButton").click(function () {
//    var post = {
//        userName: $("#userNameInput").val() || "Guest",
//        text: $("#textInput").val(),
//        time: $("#time").val(),

//    };
//    console.log(time)

//    $.ajax({
//        url: '/api/Posts/AddPost',
//        method: 'POST',
//        data: post
//    });
//});