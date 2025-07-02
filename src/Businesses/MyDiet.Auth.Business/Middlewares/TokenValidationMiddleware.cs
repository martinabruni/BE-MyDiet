//using BaseUtility;
//using Microsoft.AspNetCore.Http;
//using MyDiet.Auth.Domain.Managers;

//namespace MyDiet.Auth.Api.Middleware
//{
//    public class TokenValidationMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public TokenValidationMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context, ITokenManager tokenManager)
//        {
//            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
//            if (authHeader is null || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
//            {
//                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                await context.Response.WriteAsync("Missing or invalid Authorization header.");
//                return;
//            }

//            var token = authHeader.Substring("Bearer ".Length).Trim();

//            var validationResponse = await tokenManager.ValidateTokenAsync(token);
//            if (validationResponse.StatusCode != BusinessCode.Ok)
//            {
//                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                await context.Response.WriteAsync("Invalid or expired token.");
//                return;
//            }

//            await _next(context);
//        }
//    }
//}
