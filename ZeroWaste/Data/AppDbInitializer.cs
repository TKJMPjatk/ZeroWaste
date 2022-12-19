using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data.Static;
using ZeroWaste.Models;

namespace ZeroWaste.Data
{
    public class AppDbInitializer
    {
        public static List<String> userIds = new();
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
                        new IngredientType()
                        {
                            Name = "Ziarno"
                        },
                        new IngredientType()
                        {
                            Name = "Olej"
                        },
                        new IngredientType()
                        {
                            Name = "Cukier"
                        },
                        new IngredientType()
                        {
                            Name = "Drób"
                        },
                        new IngredientType()
                        {
                            Name = "Grzyb"
                        },
                        new IngredientType()
                        {
                            Name = "Plyn"
                        }
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
                            Name = "Chleb żytni",
                            Description = "Odmiana chleba wypiekanego z pszennej, żytniej lub mieszanej mąki razowej.",
                            IngredientTypeId = 9,
                            UnitOfMeasureId = 5
                        },
                        new Ingredient()
                        {
                            Name = "Ryż biały",
                            Description = "Ryż biały to ryż bielony, z którego usunięto łuskę, otręby i zarodki.",
                            IngredientTypeId = 13,
                            UnitOfMeasureId = 2
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
                        },
                        new Ingredient()
                        {
                            Name = "Jogurt naturalny",
                            Description = "Jogurt naturalny otrzymuje się z mleka pasteryzowanego w procesie fermentacji mlekowej.",
                            IngredientTypeId = 1,
                            UnitOfMeasureId = 2
                        },
                        new Ingredient()
                        {
                            Name = "Cukier biały",
                            Description = "Rodzaj cukru buraczanego, wyróżniany na podstawie cech i sposobu produkcji.",
                            IngredientTypeId = 2,
                            UnitOfMeasureId = 2
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
                        new Category()
                        {
                            Name = "Obiady"
                        },
                        new Category()
                        {
                            Name = "Wszystkie"
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Recipes.Any())
                {
                    context.Recipes.AddRange(new List<Recipe>()
                    {
                        new Recipe()
                        {
                            Title = "Spaghetti bolognese",
                            Description = "1.Na głębokiej patelni rozgrzej około 2 łyżki oliwy z oliwek.\n2.Na rozgrzaną patelnię wrzuć czosnek, a po chwili dodaj mięso, rozdrabniaj je np. widelcem, tak aby nie powstały grube mięsne grudki.\n3.Do mięsa dodaj zioła oraz koncentrat. Całość podgrzewaj przez chwilę, dodaj passatę (przecier pomidorowy), gotuj na małym ogniu około 30 minut.\nMakaron ugotuj al dente, podawaj go z sosem, serem, i bazylią.",
                            EstimatedTime = 45,
                            DifficultyLevel = 2,
                            CreatedAt = DateTime.Now,
                            CategoryId = 1,
                            AuthorId = userIds[0],
                            StatusId = 1,
                        },
                        new Recipe()
                        {
                            Title = "Spaghetti bolognese",
                            Description = "*Niecenzuralne słowa*",
                            EstimatedTime = 45,
                            DifficultyLevel = 1,
                            CreatedAt = DateTime.Now,
                            CategoryId = 1,
                            AuthorId = userIds[0],
                            StatusId = 2,
                        },
                        new Recipe()
                        {
                            Title = "Makaron z truskawkami i serem",
                            Description = "Makaron gotujemy al dente. Odcedzamy \r\nna sicie, przelewamy zimną wodą. Twaróg \r\nrozgniatamy w misce widelcem. Dodajemy \r\njogurt naturalny i 30 gram cukru (ewentualnie \r\nzamiennik cukru, np. erytrol). Mieszamy. \r\nPołowę truskawek kroimy w kostkę, resztę \r\nz kolei rozgniatamy widelcem i mieszamy \r\nz 15 gram cukru. Zimny makaron \r\npolewamy sosem twarogowym, musem \r\ntruskawkowym z cukrem. Przekładamy do \r\nbento. Dekorujemy pozostałymi truskaw- \r\nkami i listkami mięty. Makaron jemy na \r\nzimno. Dobrze sprawdza się jako posiłek \r\nw upalne dni.",
                            EstimatedTime = 20,
                            DifficultyLevel = 2,
                            CreatedAt = DateTime.Now,
                            CategoryId = 1,
                            AuthorId = userIds[0],
                            StatusId = 1,
                        },
                        new Recipe()
                        {
                            Title = "Smoothie tropikalne",
                            Description = "Połączenie banana i ananasa daje tropikalne połączenie a marchew nadaje polskiej nuty. Obierz marchewki, banany, przygotuj ananasa, dodaj wody na oko. Zmixuj. Gotowe.",
                            EstimatedTime = 5,
                            DifficultyLevel = 2,
                            CreatedAt = DateTime.Now,
                            CategoryId = 5,
                            AuthorId = userIds[1],
                            StatusId = 1,
                        },
                        new Recipe()
                        {
                            Title = "Chleb w jajku",
                            Description = "1.Lekko ubić jajka z vegetą, wymoczyć pokrojony w kromki chleb w jajku, włożyć na rozgrzaną patelnię, podsmażyć, podawać z pomidorami.",
                            EstimatedTime = 15,
                            DifficultyLevel = 3,
                            CreatedAt = DateTime.Now,
                            CategoryId = 3,
                            AuthorId = userIds[1],
                            StatusId = 1,
                        },
                    });
                    context.SaveChanges();
                }

                if (!context.RecipeIngredients.Any())
                {
                    context.RecipeIngredients.AddRange(new List<RecipeIngredient>()
                    {
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 19,
                            Quantity = 300
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 27,
                            Quantity = 300
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 26,
                            Quantity = 300
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 32,
                            Quantity = 30
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 20,
                            Quantity = 15
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 33,
                            Quantity = 1
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 34,
                            Quantity = 2.5
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 35,
                            Quantity = 2.5
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 12,
                            Quantity = 3
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 13,
                            Quantity = 3
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 36,
                            Quantity = 4
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 19,
                            Quantity = 300
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 27,
                            Quantity = 300
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 26,
                            Quantity = 300
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 32,
                            Quantity = 30
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 20,
                            Quantity = 15
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 33,
                            Quantity = 1
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 34,
                            Quantity = 2.5
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 35,
                            Quantity = 2.5
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 12,
                            Quantity = 3
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 13,
                            Quantity = 3
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 2,
                            IngredientId = 36,
                            Quantity = 4
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 3,
                            IngredientId = 29,
                            Quantity = 200
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 3,
                            IngredientId = 30,
                            Quantity = 200
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 3,
                            IngredientId = 37,
                            Quantity = 140
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 3,
                            IngredientId = 38,
                            Quantity = 45
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 3,
                            IngredientId = 7,
                            Quantity = 250
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 3,
                            IngredientId = 31,
                            Quantity = 6
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 4,
                            IngredientId = 2,
                            Quantity = 2
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 4,
                            IngredientId = 9,
                            Quantity = 3
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 4,
                            IngredientId =10,
                            Quantity = 1
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 5,
                            IngredientId =22,
                            Quantity = 10
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 5,
                            IngredientId =21,
                            Quantity = 4
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 5,
                            IngredientId =23,
                            Quantity = 1
                        },
                    });
                    context.SaveChanges();
                }

                if (!context.RecipeReviews.Any())
                {
                    context.RecipeReviews.AddRange(new List<RecipeReview>()
                    {
                        new RecipeReview()
                        {
                            Stars = 4,
                            Description = "Dobre.",
                            AuthorId = userIds[0],
                            CreatedAt = DateTime.Now,
                            RecipeId = 1,
                        },
                        new RecipeReview()
                        {
                            Stars = 5,
                            Description = "Rewelacja! Dzieci uwielbiają!",
                            AuthorId = userIds[0],
                            CreatedAt = DateTime.Now,
                            RecipeId = 3,
                        },
                        new RecipeReview()
                        {
                            Stars = 1,
                            Description = "Na oko to chłop w szpitalu umarl!",
                            AuthorId = userIds[0],
                            CreatedAt = DateTime.Now,
                            RecipeId = 4,
                        },
                        new RecipeReview()
                        {
                            Stars = 5,
                            Description = "Mógłbym jeść to codziennie :)",
                            AuthorId = userIds[1],
                            CreatedAt = DateTime.Now,
                            RecipeId = 5,
                        },
                    });
                    context.SaveChanges();
                }

                if (!context.ShoppingLists.Any())
                {
                    context.ShoppingLists.AddRange(new List<ShoppingList>()
                    {
                        new ShoppingList()
                        {
                            UserId = userIds[1],
                            Title = "Chleb w jajku - lista zakupów",
                            Note = "pamiętać o pomidorach",
                            CreatedAt = DateTime.Now
                        },
                        new ShoppingList()
                        {
                            UserId = userIds[0],
                            Title = "Makaron z truskawkami - lista zakupów",
                            Note = "",
                            CreatedAt = DateTime.Now
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.ShoppingListIngredients.Any())
                {
                    context.ShoppingListIngredients.AddRange(new List<ShoppingListIngredient>()
                    {
                        new ShoppingListIngredient()
                        {
                            ShoppingListId = 1,
                            IngredientId = 22,
                            Quantity = 10
                        },
                        new ShoppingListIngredient()
                        {
                            ShoppingListId = 1,
                            IngredientId =21,
                            Quantity = 4
                        },
                        new ShoppingListIngredient()
                        {
                            ShoppingListId = 1,
                            IngredientId =23,
                            Quantity = 1
                        },
                        new ShoppingListIngredient()
                        {
                            ShoppingListId = 2,
                            IngredientId = 29,
                            Quantity = 200
                        },
                        new ShoppingListIngredient()
                        {
                            ShoppingListId = 2,
                            IngredientId = 30,
                            Quantity = 200
                        },
                        new ShoppingListIngredient()
                        {
                            ShoppingListId = 2,
                            IngredientId = 37,
                            Quantity = 140
                        },
                        new ShoppingListIngredient()
                        {
                            ShoppingListId = 2,
                            IngredientId = 38,
                            Quantity = 45
                        },
                        new ShoppingListIngredient()
                        {
                            ShoppingListId = 2,
                            IngredientId = 7,
                            Quantity = 250
                        },
                        new ShoppingListIngredient()
                        {
                            ShoppingListId = 2,
                            IngredientId = 31,
                            Quantity = 6
                        },
                    });
                    context.SaveChanges();
                }
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
                        UserName = "jakub-michalak",
                        FullName = "Jakub Michalak",
                        Email = admins20034Email,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.Now,
                        Banned = false
                    };
                    await userManager.CreateAsync(newS20034AdminUser, "RUCH200nowe!");
                    await userManager.AddToRoleAsync(newS20034AdminUser, UserRoles.Admin);
                }
                else
                {
                    userIds.Add(adminS20034User.Id);
                }

                string admins19424Email = "s19424@pjwstk.edu.pl";
                var admins19424User = await userManager.FindByEmailAsync(admins19424Email);
                if (admins19424User is null)
                {
                    var newS19424AdminUser = new ApplicationUser()
                    {
                        UserName = "tomasz-krasienko",
                        FullName = "Tomasz Krasieńko",
                        Email = admins19424Email,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.Now,
                        Banned = false
                    };
                    await userManager.CreateAsync(newS19424AdminUser, "RUCH200nowe!");
                    await userManager.AddToRoleAsync(newS19424AdminUser, UserRoles.Admin);
                }
                else
                {
                    userIds.Add(admins19424User.Id);
                }

                string appUserEmail = "user@gmail.com";
                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser is null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        UserName = "jan-kowalski",
                        FullName = "Jan Kowalski",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.Now,
                        Banned = false
                    };
                    await userManager.CreateAsync(newAppUser, "RUCH200nowe!");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                    userIds.Add(newAppUser.Id);
                }
                else
                {
                    userIds.Add(appUser.Id);
                }

                string anotherAppUserEmail = "user2@gmail.com";
                var anotherAppUser = await userManager.FindByEmailAsync(anotherAppUserEmail);
                if (anotherAppUser is null)
                {
                    var newAnotherAppUser = new ApplicationUser()
                    {
                        UserName = "adam-nowak",
                        FullName = "Adam Nowak",
                        Email = anotherAppUserEmail,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.Now,
                        Banned = false
                    };
                    await userManager.CreateAsync(newAnotherAppUser, "RUCH200nowe!");
                    await userManager.AddToRoleAsync(newAnotherAppUser, UserRoles.User);
                    userIds.Add(newAnotherAppUser.Id);
                }
                else
                {
                    userIds.Add(anotherAppUser.Id);
                }
            }
        }
    }
}
