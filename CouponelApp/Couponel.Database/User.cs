﻿using System.ComponentModel.DataAnnotations;

namespace Couponel.Entities
{
    public sealed class User: Entity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string UserType { get; set; }
    }
}