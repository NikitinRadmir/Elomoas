﻿using Elomoas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        ClaimsPrincipal User { get; }
        string IdentityId { get; }
        int? UserId { get; }

        Task<AppUser> GetCurrentAppUserAsync();
    }
}
