﻿using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Services.Authentication;

public interface IAuthenticationService
{
    public AuthenticationResponse Register(RegisterRequest request);
    public AuthenticationResponse Login(LoginRequest request);
}
