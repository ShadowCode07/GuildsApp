using GuildsApp.Application.Interfaces;
using GuildsApp.Application.Interfaces.Repository;
using GuildsApp.Application.Interfaces.Security;
using GuildsApp.Application.MappingProfiles;
using GuildsApp.Application.Services;
using GuildsApp.Core.Interfaces;
using GuildsApp.Infrastructure;
using GuildsApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.CookiePolicy;

namespace GuildsApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ICommunityMemberRepository, CommunityMemberRepository>();
            builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IPostVoteRepository, PostVoteRepository>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFeedRepository, FeedRepository>();

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ICommunityService, CommunityService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IFeedService, FeedService>();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<PostProfile>();
                cfg.AddProfile<CommunityProfile>();
                cfg.AddProfile<FeedProfile>();
            });


            builder.Services.AddControllersWithViews();

            builder.Services
                .AddAuthentication("AuthCookie")
                .AddCookie("AuthCookie", options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Denied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;

                    options.Events.OnValidatePrincipal = async context =>
                    {
                        var sessionToken = context.Principal?
                            .Claims.FirstOrDefault(c => c.Type == "Session_Token")?.Value;
                        var userIdClaim = context.Principal?
                            .Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                        if (string.IsNullOrWhiteSpace(sessionToken) ||
                            !int.TryParse(userIdClaim, out var userId))
                        {
                            context.RejectPrincipal();
                            await context.HttpContext.SignOutAsync("AuthCookie");
                            return;
                        }

                        var sessionRepository = context.HttpContext.RequestServices
                            .GetRequiredService<ISessionRepository>();

                        var session = await sessionRepository.GetByTokenAsync(sessionToken);

                        if (session == null || session.UserId != userId)
                        {
                            context.RejectPrincipal();
                            await context.HttpContext.SignOutAsync("AuthCookie");
                            return;
                        }
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Feed}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
