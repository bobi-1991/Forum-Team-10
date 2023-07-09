namespace ForumTemplate.Exceptions
{
	public class EntityAuthenticationException:ApplicationException
	{
        public EntityAuthenticationException(string message)
            :base(message)
        {
        }
    }
}
