using Microsoft.AspNetCore.Http;
using PhyGen.Infrastructure.Persistence.DbContexts;
using System.Security.Claims;
using PhyGen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class ActiveGuard(AppDbContext context)
{
    public async Task<bool> EnsureActiveAsync(HttpContext ctx)
    {
        // Chỉ kiểm tra khi đã xác thực (Bearer)
        if (!(ctx.User?.Identity?.IsAuthenticated ?? false))
            return true;

        var userId = ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? ctx.User.FindFirstValue("sub")
                   ?? ctx.User.FindFirstValue("uid");

        if (!Guid.TryParse(userId, out var uid))
            return true;

        var isActive = await context.Users.AsNoTracking()
                          .Where(u => u.Id == uid)
                          .Select(u => u.IsActive)
                          .FirstOrDefaultAsync();

        if (!isActive)
        {
            // KHÔNG gọi SignOutAsync ở JWT
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync("""
            {"StatusCode":401,"Message":"Tài khoản đã bị khóa."}
            """);
            return false;
        }

        return true;
    }
}
