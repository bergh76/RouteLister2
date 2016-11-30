using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;

namespace RouteLister2.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public string Time { get; set; }

       public Post(int id, string userName, string text, string time)
        {
            Id = id;
            UserName = userName;
            Text = text;
            Time = time;
        }

        public Post() { }
    
    }
}