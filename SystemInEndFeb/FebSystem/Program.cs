using ADOHelper.Db;
using Dapper.Db;
using EFCore.Db;
using EntityClass;
using FebSystem.Filters;
using FluentValidation.AspNetCore;
using IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using System.Reflection;
using System.Text;

namespace FebSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // configure token using component
            builder.Services.AddSwaggerGen(c =>
            {
                var scheme = new OpenApiSecurityScheme()
                {
                    Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    },
                    Scheme = "oauth2",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                };
                c.AddSecurityDefinition("Authorization", scheme);
                var requirement = new OpenApiSecurityRequirement();
                requirement[scheme] = new List<string>();
                c.AddSecurityRequirement(requirement);
            });

            // ef core
            builder.Services.AddTransient<IBookEFRepository, BookEFRepository>();
            builder.Services.AddTransient<ILoginEFRepository, LoginEFRepository>();
            builder.Services.AddTransient<EFProvider>();
            builder.Services.AddDbContext<BookEFDbContext>(opt =>
            {
                // user secrets
                string? connStr = builder.Configuration.GetConnectionString("ConnStr");
                opt.UseSqlServer(connStr);
                Console.WriteLine("sql connection success");

            });

            // jwt and identity framework
            builder.Services.AddDbContext<IdEFDbContext>(opt =>
            {
                // user secrets
                string? connStr = builder.Configuration.GetConnectionString("ConnStr");
                opt.UseSqlServer(connStr);
                Console.WriteLine("sql connection success");

            });

            builder.Services.AddDataProtection();
            builder.Services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit= true;
                opt.Password.RequireLowercase= false ;
                opt.Password.RequireUppercase= false ;
                opt.Password.RequireLowercase= false ;
                opt.Password.RequireNonAlphanumeric= false;
                opt.Password.RequiredLength= 5;
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            });
            var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
            idBuilder.AddEntityFrameworkStores<IdEFDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<Role>>()
                .AddUserManager<UserManager<User>>();
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(x =>
                {
                    var jwtOpt = builder.Configuration.GetSection("JWT").Get<JwtOptions>();
                    byte[] keyBytes = Encoding.UTF8.GetBytes(jwtOpt.SigningKey);
                    var secKey = new SymmetricSecurityKey(keyBytes);
                    x.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = secKey
                    };
                });

            // dapper 
            builder.Services.AddTransient<IBookDapperRepository, BookDapperRepository>();
            builder.Services.AddTransient<DapperProvider>();

            // ado
            builder.Services.AddTransient<IBookAdoRepository, BookAdoRepository>();
            builder.Services.AddTransient<AdoProvider>();

            //filter
            //global exception filter
            //action filters
            builder.Services.Configure<MvcOptions>(opt =>
            {
                opt.Filters.Add<GlobalExceptionFilter>();
                opt.Filters.Add<RateLimitFilter>();
                opt.Filters.Add<TransactionScopFilter>();
            });

            // memory cache
            builder.Services.AddMemoryCache();

            //fluent validate data
            builder.Services.AddFluentValidation(fv =>
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                fv.RegisterValidatorsFromAssemblies(new Assembly[] {assembly});
            });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}