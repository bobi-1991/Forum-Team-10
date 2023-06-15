using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Repositories.PostPersistence;
using ForumTemplate.Validation;
using System.Collections.Generic;

namespace ForumTemplate.Repositories.PostPersistence
{
    public class PostRepository : IPostRepository
    {

        private readonly List<Post> posts = new();

        public PostRepository()
        {
            this.posts.Add(Post.CreatePost("Post1", "First Post", Guid.NewGuid()));
            this.posts.Add(Post.CreatePost("Post1", "First Post", Guid.NewGuid()));
            this.posts.Add(Post.CreatePost("Post1", "First Post", Guid.NewGuid()));
        }

        public List<Post> GetAll()
        {
            return this.posts;
        }

        public Post GetById(Guid id)
        {
            return posts.Where(p => p.Id == id).FirstOrDefault();
        }

        //Not Configured for any EndPoint yet
        public Post GetByTitle(string title)
        {
            Post post = posts.Where(p => p.Title == title).FirstOrDefault();

            return post ?? throw new EntityNotFoundException($"Post with title={title} doesn't exist.");
        }

        public List<Post> GetByUserId(Guid id)
        {
            return posts.Where(p => p.UserId == id).ToList();
        }

        public Post Create(Post post)
        {
          

            posts.Add(post);
            return post;
        }

        public Post Update(Guid id, Post post)
        {
            Post postToUpdate = GetById(id);
            postToUpdate.Update(post);

            return postToUpdate;
        }

        public string Delete(Guid id)
        {
            Post existingPost = GetById(id);
            posts.Remove(existingPost);

            return "Post was successfully deleted.";
        }

        public bool Exist(Guid id)
        {
            return posts.Any(p => p.Id == id);
        }
    }

}

