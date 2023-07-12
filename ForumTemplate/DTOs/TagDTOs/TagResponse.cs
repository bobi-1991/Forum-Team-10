namespace ForumTemplate.DTOs.TagDTOs
{
    public record TagResponse(
       Guid Id,
       string Content,
       Guid UserId,
       Guid PostId,
       DateTime CreatedDate,
       DateTime UpdatedDate);

}
