using ForumTemplate.Models.Input;
using ForumTemplate.Models.Result;
using ForumTemplate.Repositories.DTO_s;

namespace ForumTemplate.Mappers
{
    public static class PostMapper
    {
        public static PostDTO MapToPostDTO(this PostInputModel postInputModel, int userId)
        {
            return new PostDTO()
            {
                Description = postInputModel.Description,
                Title = postInputModel.Title,
                UserId = userId
            };
        }

        public static PostDTO MapToPostDTO(this PostInputModel postInputModel)
        {
            return new PostDTO()
            {
                Description = postInputModel.Description,
                Title = postInputModel.Title
            };
        }

        public static PostResultModel MapToPostResultModel(this PostDTO postDto)
        {
            return new PostResultModel
            {
                Description= postDto.Description,
                Title = postDto.Title
            };
        }

        public static PostResultModel MapToPostResultModel(this PostDTO postDto, List<CommentResultModel> comments)
        {
            return new PostResultModel
            {
                Description = postDto.Description,
                Title = postDto.Title,
                Comments = comments
            };
        }
    }
}
