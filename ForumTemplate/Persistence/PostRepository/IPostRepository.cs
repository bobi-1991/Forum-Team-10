
using ForumTemplate.Models;
using ForumTemplate.Models.Pagination;

namespace ForumTemplate.Persistence.PostRepository
{
    public interface IPostRepository
    {
        List<Post> GetAll();
        IQueryable<Post> GetAllToQueriable();
        List<Post> FilterBy(PostQueryParameters filterParameters);
        PaginatedList<Post> SearchBy(PostQueryParameters search);
        Post GetById(Guid id);
        Post GetByTitle(string title);
        Post Create(Post post);
        Post Update(Guid id, Post post);
        string Delete(Guid id);
       void DeletePosts(List<Post> postsToDelete);
        List<Post> GetByUserId(Guid id);
        bool Exist(Guid id);
        List<Post> GetTopCommentedPosts(int count);
        List<Post> GetRecentlyCreatedPosts(int count);
    }
}
