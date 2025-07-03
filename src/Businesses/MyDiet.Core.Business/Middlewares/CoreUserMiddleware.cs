using BaseUtility;
using Microsoft.AspNetCore.Http;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Enums;
using MyDiet.Auth.Infrastructure.Models;
using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.Middlewares
{
    public class CoreUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CoreUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IService<CoreUserDto, CoreUser, Guid> coreUserService, IService<AuthUserDto, AuthUser, Guid> authUserService, IMapper<UserClaims, CoreUserDto> userMapper)
        {
            var user = context.User;
            var userIdClaim = user.FindFirst("userId");
            var usernameClaim = user.FindFirst("username");
            var roleClaim = user.FindFirst(ClaimTypes.Role);

            if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var coreUser = await coreUserService.GetByIdAsync(userId);

                if (coreUser.Data is null)
                {
                    var isFirstLogin = await authUserService.FindAsync(au => au.CoreUserId == userId);

                    if (isFirstLogin.Data is not null)
                    {
                        var userClaims = new UserClaims
                        {
                            UserId = userId,
                            Username = usernameClaim?.Value,
                            Role = Enum.TryParse<UserRole>(roleClaim?.Value, out var role) ? role : UserRole.User
                        };
                        await coreUserService.CreateAsync(userMapper.Map(userClaims));
                    }
                    else
                    {
                        // Not registered, block request
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("User not registered.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
