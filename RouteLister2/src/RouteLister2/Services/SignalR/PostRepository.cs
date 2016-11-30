using RouteLister2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteLister2.SignalR
{
    public class PostRepository : IPostRepository
    {
        private List<Post> _posts = new List<Post>()
        {
            //new Post(1, "Obi-Wan Kenobi","These are not the droids you're looking for","10:22:12"),
            //new Post(2, "Darth Vader","I find your lack of faith disturbing","11:31:56")
        };
        public void AddPost(Post post)
        {
            post.Time = DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss");
            _posts.Add(post);
        }

        public List<Post> GetAll()
        {
            return _posts.OrderBy(x => x.Id).ToList();
        }

        public Post GetPost(int id)
        {
            return _posts.FirstOrDefault(p => p.Id == id);
        }
    }
}