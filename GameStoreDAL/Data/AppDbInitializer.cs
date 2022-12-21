using GameStoreDAL.Data.Helpers;
using GameStoreDAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDAL.Data
{
    public class AppDbInitializer
    {
        public static async Task SeedData(IApplicationBuilder builder)
        {
            await SeedRolesToDb(builder);
            await SeedDataToDb(builder);
        }

        public static async Task SeedDataToDb(IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<GameStoreDbContext>();
                var mmorpg = new Genre { Name = "MMORPG" };
                var racing = new Genre { Name = "Racing" };
                var fps = new Genre { Name = "FPS" };
                var moba = new Genre { Name = "MOBA" };
                var action = new Genre { Name = "Action" };
                var shooter = new Genre { Name = "Shooter" };
                var strategy = new Genre { Name = "Strategy" };

                if (!context.Genres.Any())
                {
                    context.Genres.AddRange(
                        mmorpg,
                        racing,
                        fps,
                        moba,
                        action,
                        shooter,
                        strategy
                    );
                }

                if (!context.Games.Any())
                {
                    context.Games.AddRange(
                    new Game
                    {
                        Title = "Half Life 2",
                        Description = "Half-Life 2 is a 2004 first-person shooter game developed by Valve. It was published by Valve through its distribution service Steam.",
                        ImageUrl = "https://www.mobygames.com/images/covers/large/1101679693-00.jpg",
                        Price = 85.0,
                        Genres = new List<Genre>()
                        {
                            fps,
                            action,
                            shooter
                        }
                    },
                    new Game
                    {
                        Title = "Borderlands",
                        Description = "Borderlands is a 2009 open world action role-playing first-person looter shooter video game. It is the first game in the Borderlands series.",
                        ImageUrl = "https://www.mobygames.com/images/covers/l/239301-borderlands-windows-front-cover.jpg",
                        Price = 65.0,
                        Genres = new List<Genre>()
                        {
                            fps,
                            action,
                            shooter
                        }
                    },
                    new Game
                    {
                        Title = "Need For Speed Most Wanted",
                        Description = "Need for Speed: Most Wanted is a 2005 open-world racing video game, and the ninth installment in the Need for Speed series.",
                        ImageUrl = "https://www.mobygames.com/images/covers/l/327127-need-for-speed-most-wanted-black-edition-xbox-front-cover.jpg",
                        Price = 70.0,
                        Genres = new List<Genre>()
                        {
                            racing
                        }
                    },
                    new Game
                    {
                        Title = "Devil May Cry 4",
                        Description = "Devil May Cry 4 is a 2008 action-adventure game developed and published by Capcom. It was released for the PlayStation 3, Xbox 360, and Microsoft Windows platforms.",
                        ImageUrl = "https://www.mobygames.com/images/covers/l/106399-devil-may-cry-4-playstation-3-front-cover.jpg",
                        Price = 75.0,
                        Genres = new List<Genre>()
                        {
                            action
                        }
                    },
                    new Game
                    {
                        Title = "League Of Legends",
                        Description = "A multiplayer online battle arena (MOBA) game in which the player controls a character (champion) with a set of unique abilities from an isometric perspective.",
                        ImageUrl = "https://cdkeyprices.com/images/games/823660/cover.jpg",
                        Price = 55.0,
                        Genres = new List<Genre>()
                        {
                            moba,
                            strategy
                        }
                    },
                    new Game
                    {
                        Title = "World of Warcraft",
                        Description = "A massively multiplayer online role-playing game (MMORPG) released in 2004 by Blizzard Entertainment.",
                        ImageUrl = "https://www.mobygames.com/images/covers/l/216725-world-of-warcraft-macintosh-front-cover.jpg",
                        Price = 60.0,
                        Genres = new List<Genre>()
                        {
                            mmorpg,
                            strategy
                        }
                    }
                    );
                }


                    await context.SaveChangesAsync();

            }
        }

        public static async Task SeedRolesToDb(IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }
        }
    }
}
