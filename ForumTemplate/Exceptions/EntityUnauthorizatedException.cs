namespace ForumTemplate.Exceptions
{
    public class EntityUnauthorizatedException:ApplicationException
    {
        public EntityUnauthorizatedException(string message)
            :base(message)
        {
        }
    }
}
