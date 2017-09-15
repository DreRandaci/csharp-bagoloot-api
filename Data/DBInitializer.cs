using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BagoLootAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BagoLootAPI.Data
{
    public static class DbInitializer
    {
        public async static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var userstore = new UserStore<User>(context);

                if (!context.Roles.Any(r => r.Name == "Administrator"))
                {
                    var role = new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" };
                    await roleStore.CreateAsync(role);
                }

                if (!context.User.Any(u => u.FirstName == "admin"))
                {
                    //  This method will be called after migrating to the latest version.
                    User user = new User {
                        FirstName = "admin",
                        LastName = "admin",
                        UserName = "admin@admin.com",
                        NormalizedUserName = "ADMIN@ADMIN.COM",
                        Email = "admin@admin.com",
                        NormalizedEmail = "ADMIN@ADMIN.COM",
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    };
                    var passwordHash = new PasswordHasher<User>();
                    user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
                    await userstore.CreateAsync(user);
                    await userstore.AddToRoleAsync(user, "Administrator");
                }

                // Look for any products.
                if (context.Child.Any())
                {
                    return;   // DB has been seeded
                }

                var children = new Child[]
                {
                    new Child { 
                        Name = "Svetlana"
                    },
                    new Child { 
                        Name = "Nigel"
                    },
                    new Child { 
                        Name = "Sequina"
                    },
                };

                foreach (Child i in children)
                {
                    context.Child.Add(i);
                }
                context.SaveChanges();

                var toys = new Toy[]
                {
                    new Toy { 
                        Name = "Marbles",
                        ChildId = children.Single(s => s.Name == "Svetlana").ChildId
                    },
                    new Toy { 
                        Name = "Jump rope",
                        ChildId = children.Single(s => s.Name == "Svetlana").ChildId
                    },
                    new Toy { 
                        Name = "Transformers",
                        ChildId = children.Single(s => s.Name == "Svetlana").ChildId
                    },
                    new Toy { 
                        Name = "Silly Putty",
                        ChildId = children.Single(s => s.Name == "Nigel").ChildId
                    },
                    new Toy { 
                        Name = "Yo-yo",
                        ChildId = children.Single(s => s.Name == "Nigel").ChildId
                    },
                    new Toy { 
                        Name = "Wonder Woman",
                        ChildId = children.Single(s => s.Name == "Sequina").ChildId
                    },
                    new Toy { 
                        Name = "Batman",
                        ChildId = children.Single(s => s.Name == "Sequina").ChildId
                    },
                    new Toy { 
                        Name = "Army men",
                        ChildId = children.Single(s => s.Name == "Sequina").ChildId
                    },
                };

                foreach (Toy i in toys)
                {
                    context.Toy.Add(i);
                }
                context.SaveChanges();


                var reindeer = new Reindeer[]
                {
                    new Reindeer { 
                        Name = "Dasher"
                    },
                    new Reindeer { 
                        Name = "Dancer"
                    },
                    new Reindeer { 
                        Name = "Prancer"
                    },
                    new Reindeer { 
                        Name = "Vixen"
                    },
                };

                foreach (Reindeer i in reindeer)
                {
                    context.Reindeer.Add(i);
                }
                context.SaveChanges();

                var favorites = new FavoriteReindeer[]
                {
                    new FavoriteReindeer { 
                        ReindeerId = reindeer.Single(s => s.Name == "Dasher").ReindeerId,
                        ChildId = children.Single(s => s.Name == "Svetlana").ChildId
                    },
                    new FavoriteReindeer { 
                        ReindeerId = reindeer.Single(s => s.Name == "Dasher").ReindeerId,
                        ChildId = children.Single(s => s.Name == "Nigel").ChildId
                    },
                    new FavoriteReindeer { 
                        ReindeerId = reindeer.Single(s => s.Name == "Dasher").ReindeerId,
                        ChildId = children.Single(s => s.Name == "Sequina").ChildId
                    },
                    new FavoriteReindeer { 
                        ReindeerId = reindeer.Single(s => s.Name == "Vixen").ReindeerId,
                        ChildId = children.Single(s => s.Name == "Svetlana").ChildId
                    },
                    new FavoriteReindeer { 
                        ReindeerId = reindeer.Single(s => s.Name == "Dancer").ReindeerId,
                        ChildId = children.Single(s => s.Name == "Nigel").ChildId
                    },
                    new FavoriteReindeer { 
                        ReindeerId = reindeer.Single(s => s.Name == "Prancer").ReindeerId,
                        ChildId = children.Single(s => s.Name == "Sequina").ChildId
                    },
                    new FavoriteReindeer { 
                        ReindeerId = reindeer.Single(s => s.Name == "Prancer").ReindeerId,
                        ChildId = children.Single(s => s.Name == "Nigel").ChildId
                    },
                };

                foreach (FavoriteReindeer i in favorites)
                {
                    context.FavoriteReindeer.Add(i);
                }
                context.SaveChanges();
            }
       }
    }
}