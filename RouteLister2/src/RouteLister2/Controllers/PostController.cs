using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using RouteLister2.Models;
using RouteLister2.SignalR;
using System.Collections.Generic;

public class PostsController : Controller
{
    private IPostRepository _postRepository { get; set; }
    private IConnectionManager _connectionManager { get; set; }

    public PostsController(IPostRepository postRepository, IConnectionManager connectionManager)
    {
        _postRepository = postRepository;
        _connectionManager = connectionManager;
    }

    [HttpGet]
    public List<Post> GetPosts()
    {
        _connectionManager.GetHubContext<PostsHub>();
        return _postRepository.GetAll();
    }

    [HttpGet]
    public Post GetPost(int id)
    {
        return _postRepository.GetPost(id);
    }


    [HttpPost]
    public void AddPost(Post post)
    {
        _postRepository.AddPost(post);
        _connectionManager.GetHubContext<PostsHub>()
            .Clients.All.publishPost(post);
    }

}