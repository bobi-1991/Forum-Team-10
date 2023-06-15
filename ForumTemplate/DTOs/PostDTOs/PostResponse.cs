using ForumTemplate.DTOs.CommentDTOs;

namespace ForumTemplate.DTOs.PostDTOs
{
    //public record PostResponse(
    //    string Content,
    //     string Title,
    //    List<CommentResponse> Comments );

    public record PostResponse(
        Guid Id,
        string Title,
        string Content,
        Guid UserId,
        List<CommentResponse> Comments,
        DateTime CreatedDate,
        DateTime LastEditedDate);


}
