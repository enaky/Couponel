﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couponel.Business.Identities.Offerors.Models;

namespace Couponel.Business.Identities.Offerors.Services.Interfaces
{
    public interface IOffererService
    {
        Task<OffererModel> GetById(Guid addressId);

        Task<OffererModel> Add(CreateOffererModel model);

        Task Delete(Guid addressId);

        Task<IEnumerable<OffererModel>> GetAll();
    }
}