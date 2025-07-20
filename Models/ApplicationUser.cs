﻿using Microsoft.AspNetCore.Identity;

namespace TaskManagerApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
    }
}