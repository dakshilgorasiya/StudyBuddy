using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Interfaces;
using StudyBuddy.Models;
using StudyBuddy.Repositories;
using StudyBuddy.Services;
using StudyBuddy.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StudyBuddy.Helpers;
using Microsoft.OpenApi.Models;
using StudyBuddy.Middlewares;
using StudyBuddy.Common;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;

namespace StudyBuddy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtSection = builder.Configuration.GetSection("JwtSettings");

            var cloudinarySection = builder.Configuration.GetSection("Cloudinary");

            var jwtKey = jwtSection["Secret"];

            builder.Services.AddSingleton(
                new JwtHelper(
                    jwtKey,
                    jwtSection["Issuer"],
                    jwtSection["Audience"],
                    int.Parse(jwtSection["ExpirationMinutes"])
                )
            );

            builder.Services.AddSingleton(
                new CloudinaryHelper(
                        cloudinarySection["CloudName"],
                        cloudinarySection["ApiKey"],
                        cloudinarySection["ApiSecret"]
                    )
                );

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer {your JWT token}'"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

            // Dependect Injection
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IFollowRepository, FollowRepository>();
            builder.Services.AddScoped<IFollowService, FollowService>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddScoped<ILikeService, LikeService>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IReportService,  ReportService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(UserMapping));
            builder.Services.AddAutoMapper(typeof(PostMapping));
            builder.Services.AddAutoMapper(typeof(FollowMapping));
            builder.Services.AddAutoMapper(typeof(CommentMapping));
            builder.Services.AddAutoMapper(typeof(LikeMapping));
            builder.Services.AddAutoMapper(typeof(ReportMapping));
            builder.Services.AddAutoMapper(typeof(AdminMapping));

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true; // Disable automatic validation response
                });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidIssuer = jwtSection["Issuer"],
                                ValidAudience = jwtSection["Audience"],
                                ValidateLifetime = true,
                            };
                            options.Events = new JwtBearerEvents
                            {
                                OnChallenge = async context =>
                                {
                                    context.HandleResponse(); // Prevents default response

                                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                    context.Response.ContentType = "application/json";

                                    var response = new
                                    {
                                        StatusCode = 401,
                                        Message = "Unauthorized"
                                    };

                                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                                },
                                OnForbidden = async context =>
                                {
                                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                    context.Response.ContentType = "application/json";

                                    var response = new
                                    {
                                        StatusCode = 403,
                                        Message = "Forbidden"
                                    };

                                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                                }
                            };
                        });

            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DataBaseSeeder.Seed(dbContext);
            }

            app.UseMiddleware<ExceptionMiddleware>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
