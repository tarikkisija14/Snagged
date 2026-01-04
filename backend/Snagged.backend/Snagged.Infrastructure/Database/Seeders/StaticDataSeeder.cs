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
                new Role { RoleName = "User", RoleDescription = "Standard user" },
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

            var men = new Category { Name = "Men" };
            var women = new Category { Name = "Women" };
            var kids = new Category { Name = "Kids" };
            var accessories = new Category { Name = "Accessories" }; // Added for completeness

            context.AddRange(men, women, kids, accessories);
            await context.SaveChangesAsync();

            var subcategories = new List<Subcategory>
            {
                new Subcategory { Name = "Shirts", CategoryId = men.Id },
                new Subcategory { Name = "Pants", CategoryId = men.Id },
                new Subcategory { Name = "Coats", CategoryId = men.Id },
                new Subcategory { Name = "Dresses", CategoryId = women.Id },
                new Subcategory { Name = "Skirts", CategoryId = women.Id },
                new Subcategory { Name = "Sneakers", CategoryId = accessories.Id }, 
                new Subcategory { Name = "Bags", CategoryId = accessories.Id }      
            };

            context.AddRange(subcategories);
            await context.SaveChangesAsync();
        }
    }
}