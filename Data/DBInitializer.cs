using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BagAPI.Models;
using System.Threading.Tasks;

namespace BagAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BagAPIContext(serviceProvider.GetRequiredService<DbContextOptions<BagAPIContext>>()))
            {
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
                        Name = "Silly Putty",
                        ChildId = children.Single(s => s.Name == "Nigel").ChildId
                    },
                    new Toy { 
                        Name = "Wonder Woman",
                        ChildId = children.Single(s => s.Name == "Sequina").ChildId
                    },
                };

                foreach (Toy i in toys)
                {
                    context.Toy.Add(i);
                }
                context.SaveChanges();
            }
       }
    }
}