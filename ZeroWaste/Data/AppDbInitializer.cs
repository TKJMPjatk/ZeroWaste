using Microsoft.AspNetCore.Identity;
using ZeroWaste.Data.Static;
using ZeroWaste.Models;

namespace ZeroWaste.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                if (!context.IngredientTypes.Any())
                {
                    context.IngredientTypes.AddRange(new List<IngredientType>()
                    {
                        new IngredientType()
                        {
                            Name = "Przyprawa"
                        },
                        new IngredientType()
                        {
                            Name = "Owoc"
                        },
                        new IngredientType()
                        {
                            Name = "Warzywo"
                        },
                        new IngredientType()
                        {
                            Name = "Mięso"
                        },
                        new IngredientType()
                        {
                            Name = "Ryba"
                        },
                        new IngredientType()
                        {
                            Name = "Ser"
                        },
                        new IngredientType()
                        {
                            Name = "Owoc morza"
                        },
                        new IngredientType()
                        {
                            Name = "Orzech"
                        },
                        new IngredientType()
                        {
                            Name = "Pieczywo"
                        },
                        new IngredientType()
                        {
                            Name = "Jajo"
                        },
                        new IngredientType()
                        {
                            Name = "Nabiał"
                        },
                        new IngredientType()
                        {
                            Name = "Makaron"
                        },
                        new IngredientType()
                        {
                            Name = "Kasza"
                        },
                        new IngredientType()
                        {
                            Name = "Sos"
                        },
                        new IngredientType()
                        {
                            Name = "Napój gazowany"
                        },
                        new IngredientType()
                        {
                            Name = "Napój niegazowany"
                        },
                        new IngredientType()
                        {
                            Name = "Napój alkoholowy"
                        },
                        new IngredientType()
                        {
                            Name = "Słona przekąska"
                        },
                        new IngredientType()
                        {
                            Name = "Napój mleczny"
                        },
                        new IngredientType()
                        {
                            Name = "Słodycz"
                        },
                        new IngredientType()
                        {
                            Name = "Kwiat"
                        },
                        new IngredientType()
                        {
                            Name = "Ciasto"
                        },
                        new IngredientType()
                        {
                            Name = "Mąka"
                        },
                        new IngredientType()
                        {
                            Name = "Ryż"
                        },
                    });
                    context.SaveChanges();
                }

                if (!context.UnitOfMeasures.Any())
                {
                    context.UnitOfMeasures.AddRange(new List<UnitOfMeasure>()
                    {
                        new UnitOfMeasure()
                        {
                            Name = "Kilogram",
                            Shortcut = "Kg"
                        },
                        new UnitOfMeasure()
                        {
                            Name = "Gram",
                            Shortcut = "g"
                        },
                        new UnitOfMeasure()
                        {
                            Name = "Litr",
                            Shortcut = "l"
                        },
                        new UnitOfMeasure()
                        {
                            Name = "Mililitr",
                            Shortcut = "ml"
                        },
                        new UnitOfMeasure()
                        {
                            Name = "Sztuka",
                            Shortcut = "szt."
                        },
                    });
                    context.SaveChanges();
                }

                if (!context.Ingredients.Any())
                {
                    context.Ingredients.AddRange(new List<Ingredient>()
                    {
                        new Ingredient()
                        {
                            Name = "Papryka",
                            Description = "Rodzaj roślin należących do rodziny psiankowatych.",
                            IngredientTypeId = 3,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Marchewka",
                            Description = "Podgatunek marchwi zwyczajnej z licznymi odmianami jadalnymi i pastewnymi.",
                            IngredientTypeId = 3,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Pietruszka",
                            Description = "Gatunek rośliny dwuletniej z rodziny selerowatych.",
                            IngredientTypeId = 3,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Pomidor",
                            Description = "Jeden z gatunków – pomidor zwyczajny – jest rozpowszechnioną rośliną uprawną.",
                            IngredientTypeId = 3,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Ogórek",
                            Description = "Rodzaj roślin jednorocznych z rodziny dyniowatych.",
                            IngredientTypeId = 3,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Dynia",
                            Description = "Rodzaj roślin z rodziny dyniowatych.",
                            IngredientTypeId = 3,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Truskawka",
                            Description = "Truskawka to bez wątpienia jeden z najpopularniejszych i najsmaczniejszych owoców uprawianych w Polsce.",
                            IngredientTypeId = 2,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Jabłko",
                            Description = "Jadalny, kulisty owoc drzew z rodzaju jabłoń Malus.",
                            IngredientTypeId = 2,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Banan",
                            Description = "Słowo banan pochodzi bądź od arabskiego słowa banan, oznaczającego palec lub z afrykańskiego języka wolof, w którym rośliny te określa się mianem banaana.",
                            IngredientTypeId = 2,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Ananas",
                            Description = "Nie nadaje się do pizzy.",
                            IngredientTypeId = 2,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Śliwka",
                            Description = "Śliwka jest owocem niektórych gatunków w Prunus subg.",
                            IngredientTypeId = 2,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Sól kuchenna",
                            Description = "Artykuł spożywczy, będący prawie czystym chlorkiem sodu, stosowany jako wzmacniacz smaku i naturalny konserwant.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Pieprz czarny",
                            Description = "Pieprz czarny uprawiany jest jako roślina użytkowa w tropikalnych rejonach o klimacie gorącym i wilgotnym.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Papryka słodka",
                            Description = "Papryka słodka mielona to bardzo popularna przyprawa kuchni włoskiej.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Curry",
                            Description = "Curry to mieszanka przypraw pochodząca z subkontynentu indyjskiego.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Mąka pszenna typ 450",
                            Description = "Rodzaj mąki otrzymywany z pszenicy.",
                            IngredientTypeId = 23,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Mąka pszenna typ 650",
                            Description = "Rodzaj mąki otrzymywany z pszenicy.",
                            IngredientTypeId = 23,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Mąka pszenna typ 850",
                            Description = "Rodzaj mąki otrzymywany z pszenicy.",
                            IngredientTypeId = 23,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Przecier pomidorowy",
                            Description = "Jest bardziej wodnisty niż koncentrat. Ma mocno pomidorowy smak i jest zazwyczaj doprawiony typowymi ziołami.",
                            IngredientTypeId = 14,
                            UnitOfMeasureId = 4
                        },
                        new Ingredient()
                        {
                            Name = "Koncentrat pomidorowy",
                            Description = "Produkt spożywczy, będący częściowo odwodnionym przecierem pomidorowym,.",
                            IngredientTypeId = 14,
                            UnitOfMeasureId = 4
                        },
                        new Ingredient()
                        {
                            Name = "Jajo kurze",
                            Description = "Produkt spożywczy będący podstawą wielu potraw, który jest bogatym źródłem substancji odżywczych.",
                            IngredientTypeId = 10,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Vegeta",
                            Description = "Mieszanka przypraw i warzyw.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Chleb pszenny",
                            Description = "Wypiekany z mąki pszennej.",
                            IngredientTypeId = 9,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Chleb pszenny",
                            Description = "Odmiana chleba wypiekanego z pszennej, żytniej lub mieszanej mąki razowej.",
                            IngredientTypeId = 9,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Ryż biały",
                            Description = "Ryż biały to ryż bielony, z którego usunięto łuskę, otręby i zarodki.",
                            IngredientTypeId = 13,
                            UnitOfMeasureId = 25
                        },
                        new Ingredient()
                        {
                            Name = "Makaron spaghetti",
                            Description = "Rodzaj długiego, cienkiego makaronu, podstawa dań tradycyjnej kuchni włoskiej.",
                            IngredientTypeId = 12,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Mięso mielone",
                            Description = " Mięso bez kości, które zostało rozdrobnione na kawałki i zawiera mniej niż 1% soli.",
                            IngredientTypeId = 4,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Ser żółty gouda",
                            Description = "Gatunek sera półtwardego, podpuszczkowego, dojrzewającego, produkowanego z mleka krowiego.",
                            IngredientTypeId = 11,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Makaron kokardki",
                            Description = "Rodzaj drobnego makaronu.",
                            IngredientTypeId = 12,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Twaróg",
                            Description = "Ser biały – produkt wytwarzany z mleka, zaliczany do serów świeżych, o białej barwie i grudkowatej bądź kremowej konsystencji, zależnej od zawartości tłuszczu w mleku.",
                            IngredientTypeId = 11,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Mięta - liść",
                            Description = "Rodzaj roślin z rodziny jasnotowatych.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Parmezan",
                            Description = "Ser twardy typu podpuszczkowego, wytwarzany z krowiego mleka, leżakujący w solance.",
                            IngredientTypeId = 11,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Czosnek - ząbek",
                            Description = "Gatunek byliny należący do rodziny amarylkowatych.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Tymianek suszony",
                            Description = "Gatunek rośliny należący do rodziny jasnotowatych.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Oregano suszony",
                            Description = "Oregano to popularna nazwa lebiodki pospolitej.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Bazylia - liść",
                            Description = "Gatunek rośliny jednorocznej z rodziny jasnotowatych.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 5
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Statuses.Any())
                {
                    context.Statuses.AddRange(new List<Status>()
                    {
                        new Status()
                        {
                            Name = "Zatwierdzony"
                        },
                        new Status()
                        {
                            Name = "Niepotwierdzony"
                        },
                        new Status()
                        {
                            Name = "Odrzucony"
                        },
                    });
                    context.SaveChanges();
                }

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(new List<Category>()
                    {
                        new Category()
                        {
                            Name = "Makarony"
                        },
                        new Category()
                        {
                            Name = "Pizza"
                        },
                        new Category()
                        {
                            Name = "Śniadania"
                        },
                        new Category()
                        {
                            Name = "Zupy"
                        },
                        new Category()
                        {
                            Name = "Smoothies"
                        },
                        new Category()
                        {
                            Name = "Burgery"
                        },
                        new Category()
                        {
                            Name = "Przekąski"
                        },
                        new Category()
                        {
                            Name = "Dania z ryżem"
                        },
                        new Category()
                        {
                            Name = "Pierogi"
                        },
                        new Category()
                        {
                            Name = "Coś słodkiego"
                        },
                        new Category()
                        {
                            Name = "Kolacje"
                        },
                    });
                    context.SaveChanges();
                }

                //if(!context.Recipes)
            }
        }

        public static async Task SeedRolesAndUsersAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string admins20034Email = "s20034@pjwstk.edu.pl";
                var adminS20034User = await userManager.FindByEmailAsync(admins20034Email);
                if (adminS20034User is null)
                {
                    var newS20034AdminUser = new ApplicationUser()
                    {
                        UserName = "Jakub Michalak",
                        Email = admins20034Email,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newS20034AdminUser, "RUCH200nowe");
                    await userManager.AddToRoleAsync(newS20034AdminUser, UserRoles.Admin);
                }

                string admins19424Email = "s19424@pjwstk.edu.pl";
                var admins19424User = await userManager.FindByEmailAsync(admins19424Email);
                if (admins19424User is null)
                {
                    var newS19424AdminUser = new ApplicationUser()
                    {
                        UserName = "Tomasz Krasieńko",
                        Email = admins19424Email,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newS19424AdminUser, "RUCH200nowe");
                    await userManager.AddToRoleAsync(newS19424AdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@gmail.com";
                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser is null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        UserName = "Jan Kowalski",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "RUCH200nowe");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }

                string anotherAppUserEmail = "user2@gmail.com";
                var anotherAppUser = await userManager.FindByEmailAsync(anotherAppUserEmail);
                if (anotherAppUser is null)
                {
                    var newAnotherAppUser = new ApplicationUser()
                    {
                        UserName = "Adam Nowak",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAnotherAppUser, "RUCH200nowe");
                    await userManager.AddToRoleAsync(newAnotherAppUser, UserRoles.User);
                }
            }
        }
    }
}
