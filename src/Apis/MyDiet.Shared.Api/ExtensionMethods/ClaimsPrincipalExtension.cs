namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtension
    {
        public static Claim? GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "userId");
        }
    }
}
