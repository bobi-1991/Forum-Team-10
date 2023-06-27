﻿using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models;

namespace ForumTemplate.Authorization
{
    public interface IAuthManager
    {
        string TrySetCurrentLoggedUser(string credentials);

        string LogoutUser(string username);

        string TryRegisterUser(RegisterUserRequestModel user);

        User GetCurrentLoggedUser();

    }
}