namespace ForumTemplate.Exceptions
{
    public class EntityBannedException:ApplicationException
    {
        public EntityBannedException(string message)
            :base(message)
        {
        }
    }
}
