using Snagged.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Snagged.Infrastructure.Database.Seeders
{
    public static class StaticDataSeeder
    {
        public static async Task SeedAsync(DatabaseContext context)
        {
            await SeedRolesAsync(context);
            await SeedCountriesAndCitiesAsync(context);
            await SeedCategoriesAndSubcategoriesAsync(context);
        }

        private static async Task SeedRolesAsync(DatabaseContext context)
        {
            if (await context.Set<Role>().AnyAsync()) return;

            var roles = new List<Role>
            {
                new Role { RoleName = "Admin", RoleDescription = "Administrator" },
                new Role { RoleName = "User", RoleDescription = "Standard User" }
            };

            context.AddRange(roles);
            await context.SaveChangesAsync();
        }

        private static async Task SeedCountriesAndCitiesAsync(DatabaseContext context)
        {
            if (await context.Set<Country>().AnyAsync()) return;

            var country = new Country { Name = "Bosnia and Herzegovina" };
            context.Add(country);
            await context.SaveChangesAsync();

            var cities = new List<City>
            {
                new City { Name = "Sarajevo", CountryId = country.Id },
                new City { Name = "Banja Luka", CountryId = country.Id },
                new City { Name = "Tuzla", CountryId = country.Id }
            };

            context.AddRange(cities);
            await context.SaveChangesAsync();
        }

        private static async Task SeedCategoriesAndSubcategoriesAsync(DatabaseContext context)
        {
            if (await context.Set<Category>().AnyAsync()) return;

            // Prave kategorije
            var men = new Category { Name = "Men" };
            var women = new Category { Name = "Women" };
            var kids = new Category { Name = "Kids" };

            context.AddRange(men, women, kids);
            await context.SaveChangesAsync();

            // Subcategories
            var subcategories = new List<Subcategory>
            {
                // Men
                new Subcategory { Name = "Shirts", CategoryId = men.Id },
                new Subcategory { Name = "Sweaters", CategoryId = men.Id },
                new Subcategory { Name = "Coats", CategoryId = men.Id },
                new Subcategory { Name = "Pants", CategoryId = men.Id },

                // Women
                new Subcategory { Name = "Blouses", CategoryId = women.Id },
                new Subcategory { Name = "Dresses", CategoryId = women.Id },
                new Subcategory { Name = "Skirts", CategoryId = women.Id },
                new Subcategory { Name = "Jackets", CategoryId = women.Id },

                // Kids
                new Subcategory { Name = "T-Shirts", CategoryId = kids.Id },
                new Subcategory { Name = "Shorts", CategoryId = kids.Id },
                new Subcategory { Name = "Sweaters", CategoryId = kids.Id },
                new Subcategory { Name = "Coats", CategoryId = kids.Id }
            };

            context.AddRange(subcategories);
            await context.SaveChangesAsync();
        }
    }
}
