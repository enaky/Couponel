﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CouponelServices.Entities
{
    public sealed class Student: Entity
    {
        public Student(string firstName, string lastName, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        [Required]
        public string FirstName { get; private set; }
        [Required]
        public string LastName { get; private set; }
        [Required]
        public string Email { get; private set; }
        [Required]
        public string PhoneNumber { get; private set; }
    }
}