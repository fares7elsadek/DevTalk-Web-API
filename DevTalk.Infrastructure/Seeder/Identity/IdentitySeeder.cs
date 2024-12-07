
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace DevTalk.Infrastructure.Seeder.Identity;

public class IdentitySeeder(AppDbContext db) : IIdentitySeeder
{
    public async Task Seed()
    {
        if(await db.Database.CanConnectAsync())
        {
            if (!db.Roles.Any())
            {
                var roles = GetRoles();
                db.Roles.AddRange(roles);
                await db.SaveChangesAsync();
            }
        }   
    }
    private IEnumerable<IdentityRole> GetRoles()
    {
        return [
            new(UserRoles.Admin)
            {
                NormalizedName= UserRoles.Admin.ToUpper()
            },
            new(UserRoles.User)
            {
                NormalizedName= UserRoles.User.ToUpper()
            }
        ];
    }
    
}
