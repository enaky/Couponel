﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Couponel.Business.Authentications.Models;
using Couponel.Business.Authentications.Services.Interfaces;
using Couponel.Entities.Identities;
using Couponel.Persistence.Repositories.IdentitiesRepositories.UsersRepository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Couponel.Business.Authentications.Services.Implementations
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly JwtOptions _config;

        public AuthenticationService(
            IUsersRepository userRepository,
            IPasswordHasher passwordHasher,
            IMapper mapper,
            IOptions<JwtOptions> config)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _config = config.Value;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest userAuthenticationModel)
        {
            var user = await _userRepository.GetByUsername(userAuthenticationModel.Username);

            return user == null || !_passwordHasher.Check(user.PasswordHash, userAuthenticationModel.Password) ? null : GenerateToken(user);
        }

        public async Task<UserModel> Register(UserRegisterModel userRegisterModel)
        {
            var user = await _userRepository.GetByUsername(userRegisterModel.UserName);
            if (user != null)
            {
                return null;
            }

            var newUser = new User(
                userRegisterModel.UserName,
                userRegisterModel.Email,
                _passwordHasher.CreateHash(userRegisterModel.Password),
                userRegisterModel.Role,
                userRegisterModel.FirstName,
                userRegisterModel.LastName,
                userRegisterModel.PhoneNumber);
            await _userRepository.Add(newUser);

            // create new student/offerer/admin 
            switch (userRegisterModel.Role)
            {
                case UserRole.Admin:

                    break;
                case UserRole.Offerer:

                    break;
                case UserRole.Student:

                    break;     
            }
            await _userRepository.SaveChanges();

            return _mapper.Map<UserModel>(newUser);
        }

        private AuthenticationResponse GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var hours = int.Parse(_config.TokenExpirationInHours);

            var token = new JwtSecurityToken(_config.Issuer,
                _config.Audience,
                new List<Claim>()
                {
                    new Claim("userId", user.Id.ToString())
                },
                expires: DateTime.Now.AddHours(hours),
                signingCredentials: credentials);

            return new AuthenticationResponse(user.UserName, new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
