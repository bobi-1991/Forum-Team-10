namespace ForumTemplate.DTOs.PostDTOs
{
    //public record PostRequest( 
    //    string Title ,
    //    string Content);
    public record PostRequest(
       string Title,
       string Content,
       Guid UserId);

}
