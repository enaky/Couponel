using AutoMapper;
using Couponel.API.Extensions;
using Couponel.Business.Authentications;
using Couponel.Business.Authentications.Services.Implementations;
using Couponel.Business.Authentications.Services.Interfaces;
using Couponel.Business.Coupons;
using Couponel.Business.Coupons.Comments.Services.Implementations;
using Couponel.Business.Coupons.Comments.Services.Interfaces;
using Couponel.Business.Coupons.Coupons.Services.Implementations;
using Couponel.Business.Coupons.Coupons.Services.Interfaces;
using Couponel.Business.Identities;
using Couponel.Business.Identities.Students.Services.Implementations;
using Couponel.Business.Identities.Students.Services.Interfaces;
using Couponel.Business.Institutions;
using Couponel.Business.Institutions.Faculties.Services.Implementations;
using Couponel.Business.Institutions.Faculties.Services.Interfaces;
using Couponel.Business.Institutions.Universities.Services.Implementations;
using Couponel.Business.Institutions.Universities.Services.Interfaces;
using Couponel.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Couponel.Business.Authentications.Models;
using Couponel.Business.Coupons.Comments.Models;
using Couponel.Business.Coupons.Comments.Validators;
using Couponel.Business.Coupons.Coupons.Models.CouponsModels;
using Couponel.Business.Coupons.Coupons.Validators;
using Couponel.Business.Coupons.Photos.Services.Implementations;
using Couponel.Business.Coupons.Photos.Services.Interfaces;
using Couponel.Business.Coupons.RedeemedCoupons.Models;
using Couponel.Business.Coupons.RedeemedCoupons.Services.Implementations;
using Couponel.Business.Coupons.RedeemedCoupons.Services.Interfaces;
using Couponel.Business.Coupons.RedeemedCoupons.Validators;
using Couponel.Business.Identities.Users.Models;
using Couponel.Business.Identities.Users.Services.Implementations;
using Couponel.Business.Identities.Users.Services.Interfaces;
using Couponel.Business.Identities.Users.Validators;
using Couponel.Business.Institutions.Faculties.Models;
using Couponel.Business.Institutions.Universities.Models;
using Couponel.Business.Institutions.Validators;
using Couponel.Persistence.Repositories.CouponsRepository;
using Couponel.Persistence.Repositories.RedeemedCouponsRepositoriy;
using Couponel.Persistence.Repositories.UniversitiesRepository;
using Couponel.Persistence.Repositories.UsersRepository;
using FluentValidation;

namespace Couponel.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers();

            services
                .AddScoped<IFacultyService, FacultyService>()
                .AddScoped<IUniversityService, UniversityService>()
                .AddScoped<ICouponService, CouponService>()
                .AddScoped<ICommentsService, CommentsService>()
                .AddScoped<IPhotosService, PhotosService>()
                .AddScoped<IStudentService, StudentService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IUsersService, UsersService>()
                .AddScoped<IRedeemedCouponsService, RedeemedCouponsService>()

                .AddScoped<IPasswordHasher, PasswordHasher>()

                .AddScoped<ICouponsRepository, CouponsRepository>()
                .AddScoped<IRedeemedCouponsRepository, RedeemedCouponsRepository > ()
                .AddScoped<IUniversitiesRepository, UniversitiesRepository>()
                .AddScoped<IUsersRepository, UsersRepository>();

            AddAuthentication(services);

            services
                .AddDbContext<CouponelContext>(config =>
                    config.UseSqlServer(Configuration.GetConnectionString("CouponelConnection")));

            services
                .AddAutoMapper(c =>
                {
                    c.AddProfile<InstitutionMappingProfile>();
                    c.AddProfile<IdentityMappingProfile>();
                    c.AddProfile<CouponMappingProfile>();
                    c.AddProfile<AuthenticationMappingProfile>();
                })
                .AddHttpContextAccessor()
                .AddSwagger()
                .AddControllers()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


            services
                .AddMvc()
                .AddFluentValidation();
            
            services
                .AddTransient<IValidator<UserRegisterModel>, UserRegisterModelValidator>()
                .AddTransient<IValidator<CreateCommentModel>, CommentModelValidator>()
                .AddTransient<IValidator<CreateCouponModel>, CreateCouponModelValidator>()
                .AddTransient<IValidator<UpdateCouponModel>, UpdateCouponModelValidator>()
                .AddTransient<IValidator<CreateRedeemedCouponModel>, CreateRedeemedCouponModelValidator>()
                .AddTransient<IValidator<CreateFacultyModel>, CreateFacultyModelValidator>()
                .AddTransient<IValidator<CreateUniversityModel>, CreateUniversityModelValidator>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSwagger()
                .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Couponel API"));

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(options => options
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader())
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
        private void AddAuthentication(IServiceCollection services)
        {
            var jwtOptions = Configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.Configure<JwtOptions>(Configuration.GetSection("JwtOptions"));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Key)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience
                    };
                });
        }
    }
}
