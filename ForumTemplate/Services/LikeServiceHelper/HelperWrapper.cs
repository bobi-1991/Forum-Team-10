using ForumTemplate.Authorization;

namespace ForumTemplate.Services.LikeServiceHelper
{
    public class HelperWrapper : IHelperWrapper
    {
        public Guid GetCurrentUserId()
        {
            return CurrentLoggedUser.LoggedUser.UserId;
        }
    }
}
