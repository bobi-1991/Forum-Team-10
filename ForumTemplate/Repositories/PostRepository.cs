using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Repositories.DTO_s;

namespace ForumTemplate.Repositories
{
    public class PostRepository : IPostRepository
    {

        private readonly List<PostDTO> posts;

        public PostRepository()
        {
            this.posts = new List<PostDTO>()
            {
                new PostDTO { Id = 1, Title = "Post1", Description = "First Post", UserId = 1 },
                new PostDTO { Id = 2, Title = "Post2", Description = "Second Post", UserId = 2 },
                new PostDTO { Id = 3, Title = "Post3", Description = "Third Post",  UserId = 2 }
            };
        }

        public List<PostDTO> GetAll()
        {
            return this.posts;
        }

        public PostDTO GetById(int id)
        {
            PostDTO post = this.posts.Where(p => p.Id == id).FirstOrDefault();

            return post ?? throw new EntityNotFoundException($"Post with id={id} doesn't exist.");
        }

        public PostDTO GetByTitle(string title)
        {
            PostDTO post = this.posts.Where(p => p.Title == title).FirstOrDefault();

            return post ?? throw new EntityNotFoundException($"Post with title={title} doesn't exist.");
        }

        public List<PostDTO> GetByUserId(int id)
        {
            return posts.Where(p => p.UserId == id).ToList();
        }

        public PostDTO Create(PostDTO postDTO)
        {
            postDTO.Id = this.posts.Count+1;
            this.posts.Add(postDTO);

            return postDTO;
        }

        public PostDTO Update(int id, PostDTO post)
        {
            PostDTO postToUpdate = this.GetById(id);
            postToUpdate.Description = post.Description;

            return postToUpdate;
        }

        public string Delete(int id)
        {
            PostDTO existingPost = GetById(id);
            posts.Remove(existingPost);

            return "Post was successfully deleted.";
        }

        public bool Exist(int id)
        {
            return this.posts.Any(p => p.Id == id);
        }

    }
}
