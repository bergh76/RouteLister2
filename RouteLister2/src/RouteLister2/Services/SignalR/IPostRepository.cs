using RouteLister2.Models;
using System.Collections.Generic;
namespace RouteLister2.SignalR
{
    public interface IPostRepository
    {
        List<Post> GetAll();
        Post GetPost(int id);
        void AddPost(Post post);

    }
}