using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Integrations.TriviaApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Data
{
    public class Seed
    {
        protected Seed() { }

        public static async Task SeedGroups(DataContext dataContext)
        {
            if (await dataContext.Groups.AnyAsync())
            {
                return;
            }

            var groups = new List<Group>
            {
                new Group{ GroupName = "OFFLINE", GroupType = GroupType.OFFLINE, IsUnique = true },
                new Group{ GroupName = "MAIN_LOBBY", GroupType = GroupType.MAIN_LOBBY, IsUnique = true },
                new Group{ GroupName = "SOLO_LOBBY", GroupType = GroupType.SOLO_LOBBY, IsUnique = true },
                new Group{ GroupName = "PUBLIC_LOBBY", GroupType = GroupType.PUBLIC_LOBBY, IsUnique = true },
                new Group{ GroupName = "PRIVATE_LOBBY", GroupType = GroupType.PRIVATE_LOBBY, IsUnique = true },
            };

            foreach (var group in groups)
            {
                await dataContext.Groups.AddAsync(group);
            }

            await dataContext.SaveChangesAsync();
        }

        public static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            if (await roleManager.Roles.AnyAsync())
            {
                return;
            }

            var roles = new List<Role>
            {
                new Role{ Name = "GUEST" },
                new Role{ Name = "USER" },
                new Role{ Name = "HOST_BOT" }
            };

            foreach (var r in roles)
            {
                await roleManager.CreateAsync(r);
            }
        }

        public static async Task SeedUsers(UserManager<User> userManager)
        {
            if (await userManager.Users.AnyAsync())
            {
                return;
            }

            var userOne = new User { UserName = "UserOne", Email = "userone@mail.net" };
            var userTwo = new User { UserName = "UserTwo", Email = "usertwo@mail.net" };

            await userManager.CreateAsync(userOne, "UserOne1$");
            await userManager.AddToRoleAsync(userOne, "USER");

            await userManager.CreateAsync(userTwo, "UserTwo2$");
            await userManager.AddToRoleAsync(userTwo, "USER");
        }

        public static async Task SeedConnections(DataContext dataContext)
        {
            if (await dataContext.Connections.AnyAsync())
            {
                return;
            }

            var group = await dataContext.Groups.SingleOrDefaultAsync(g => g.Id.Equals(GroupType.OFFLINE) && g.Active.Equals(true));

            var users = await dataContext.Users.ToListAsync();

            foreach (var u in users)
            {
                u.Connection = new Connection { Group = group, User = u };
                dataContext.Users.Update(u);
            }

            await dataContext.SaveChangesAsync();
        }

        public static async Task SeedTAQuestionCategories(DataContext dataContext)
        {
            if (await dataContext.TAQuestionCategories.AnyAsync())
            {
                return;
            }

            var categories = new List<TAQuestionCategory>
            {
                new TAQuestionCategory("Arts & Literature", "arts_and_literature"),
                new TAQuestionCategory("Film & TV", "films_and_tv"),
                new TAQuestionCategory("Food & Drink", "food_and_drink"),
                new TAQuestionCategory("General Knowledge", "general_knowledge"),
                new TAQuestionCategory("Geography", "geography"),
                new TAQuestionCategory("History", "history"),
                new TAQuestionCategory("Music", "music"),
                new TAQuestionCategory("Science", "science"),
                new TAQuestionCategory("Society & Culture", "society_and_culture"),
                new TAQuestionCategory("Sport & Leisure", "sport_and_leisure")
            };

            foreach (var category in categories)
            {
                await dataContext.TAQuestionCategories.AddAsync(category);
            }

            await dataContext.SaveChangesAsync();
        }

        public static async Task SeedTAQuestionDifficulties(DataContext dataContext)
        {
            if (await dataContext.TAQuestionDifficulty.AnyAsync())
            {
                return;
            }

            var difficulties = new List<TAQuestionDifficulty>
            {
              new TAQuestionDifficulty("easy", "easy"),
              new TAQuestionDifficulty("medium", "medium"),
              new TAQuestionDifficulty("hard", "hard")
            };

            foreach (var difficulty in difficulties)
            {
                await dataContext.TAQuestionDifficulty.AddAsync(difficulty);
            }

            await dataContext.SaveChangesAsync();
        }
    }
}
