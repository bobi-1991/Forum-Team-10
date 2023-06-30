namespace ForumTemplate.Exceptions
{
    public class EntityLoginException:ApplicationException
    {
        public EntityLoginException(string message)
            :base(message)
        {
        }
    }
}
