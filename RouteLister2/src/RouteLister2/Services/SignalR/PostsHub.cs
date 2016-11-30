using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace RouteLister2.SignalR
{
    public class PostsHub : Hub
    {
        //public void TestPost()
        //{
        //    Post post = new Post();
        //    post.Id = 1;
        //    post.Text = post.Text + "Servers added text";
        //    post.UserName = "System";
        //    Clients.Caller.publishPost(post);
        //}
        public static List<string> ConnectedUsers;


        public void Send(string userName, string text, string time)
        {
            //Clients.Caller.publishPost(userName, text, time);

            Clients.All.publishPost(userName, text, time);
        }

        public void Connect(string newUser)
        {
            if (ConnectedUsers == null)
                ConnectedUsers = new List<string>();
            ConnectedUsers.Add(newUser);
            Clients.Caller.getConnectedUsers(ConnectedUsers);
            Clients.Others.newUserAdded(newUser);
        }
    }
}