﻿namespace CodeAndCuisine.Web.Models
{
    public class LoginResponseDto
    {
        public UserDto UserDto{ get; set; }
        public string Token { get; set; }
    }
}
