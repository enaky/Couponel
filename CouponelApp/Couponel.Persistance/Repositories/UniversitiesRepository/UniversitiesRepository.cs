﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couponel.Entities.Institutions;
using Couponel.Persistence.Repositories.Repository;
using Microsoft.EntityFrameworkCore;

namespace Couponel.Persistence.Repositories.UniversitiesRepository
{
    public sealed class UniversitiesRepository : Repository<University>, IUniversitiesRepository
    {
        public UniversitiesRepository(CouponelContext context) : base(context)
        {
        }

        public async Task<University> GetByIdWithAddressFacultiesAndStudents(Guid id)
            => await this.context.Universities
                .Include(university => university.Faculties)
                    .ThenInclude(faculty => faculty.Students)
                .Include(university => university.Address)
                .FirstOrDefaultAsync(university => university.Id == id);

        public async Task<University> GetByIdWithFaculties(Guid id)
        => await this.context.Universities
            .Include(university => university.Faculties)
            .FirstOrDefaultAsync(university => university.Id == id);


        public async Task<University> GetAllDependenciesById(Guid id)
            => await this.context.Universities
                // get address for students
                .Include(university => university.Faculties)
                    .ThenInclude(faculty => faculty.Students)
                    .ThenInclude(student=> student.User.Address)

                //get user for students
                .Include(university => university.Faculties)
                    .ThenInclude(faculty => faculty.Students)
                    .ThenInclude(student => student.User)

                //get redeemedCoupons for students
                .Include(university => university.Faculties)
                    .ThenInclude(faculty => faculty.Students)
                    .ThenInclude(student => student.RedeemedCoupons)
                    .ThenInclude(rc => rc.Coupon)
                    .ThenInclude(c => c.Comments)

                .Include(university => university.Faculties)
                    .ThenInclude(faculty => faculty.Address)

                .Include(university => university.Address)

                .FirstOrDefaultAsync(university => university.Id == id);


        public async Task<University> GetByIdWithFacultyAndFacultyAddress(Guid universityId, Guid facultyId)
            => await this.context.Universities
                .Include(university => university.Address)
                .Include(university => university.Faculties)
                    .ThenInclude(faculty => faculty.Address)

                .FirstOrDefaultAsync((university => university.Id == universityId &&
                                                    university.Faculties.Any(faculty => faculty.Id == facultyId)));

        public async Task<IEnumerable<University>> GetAllWithFaculties() =>
            await this.context.Universities
                .Include(u => u.Faculties)
                .ToListAsync();


        public async Task<University> GetByIdWithFacultiesAndStudents(Guid universityId,Guid facultyId)
            => await this.context.Universities
                .Include(university => university.Faculties)
                    .ThenInclude(faculty=> faculty.Students)
                    .ThenInclude(student=>student.User)
                .FirstOrDefaultAsync((university => university.Id == universityId &&
                                                    university.Faculties.Any(faculty => faculty.Id == facultyId)));

        public async Task<University> GetByStudentId(Guid studentId)
            => await this.context.Universities
                .Include(university => university.Faculties)
                .ThenInclude(faculty => faculty.Students)
                .ThenInclude(student => student.User.Address)

                .Include(university => university.Faculties)
                .ThenInclude(faculty => faculty.Students)
                .ThenInclude(student => student.User)

                .FirstOrDefaultAsync(
                    university => university.Faculties.Any(
                        faculty => faculty.Students.Any(student => student.Id == studentId)));
    }
}
