﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Couponel.Entities.Institutions
{
    public sealed class University : Entity
    {
        public University(string name, string email, string phoneNumber)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Faculties = new List<Faculty>();
        }

        [Required]
        public string Name { get; private set; }
        [Required]
        public string Email { get; private set; }
        [Required]
        public string PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public ICollection<Faculty> Faculties { get; private set; }
        public void AddFaculty(Faculty faculty)
        {
            Faculties.Add(faculty);
        }
        public void RemoveFaculty(Guid facultyId)
        {
            var faculty = this.Faculties.FirstOrDefault(rc => rc.Id == facultyId);
            if (faculty != null)
            {
                Faculties.Remove(faculty);
            }
        }
        public void UpdateFaculty(Guid facultyId, Faculty faculty)
        {
            var facultyToUpdate = this.Faculties.FirstOrDefault(rc => rc.Id == facultyId);
            if(faculty != null)
            {
                facultyToUpdate = faculty;
            }
        }
        public void Update(string name, string email, string phoneNumber, Address address)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }
    }
}
