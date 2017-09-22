using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Checklists.AggregateRoots;
using CsvHelper;
using LeadPipe.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Checkmunk.Data.SeedData
{
    public class TestDataSeeder
    {
        public static void SeedDatabase(CheckmunkContext context)
        {
            SeedChecklistUsers(context);
            SeedChecklists(context);
            SeedUsers(context);
        }

        private static IEnumerable<Domain.Checklists.Entities.User> GetTestChecklistUsers()
        {
            var assembly = typeof(CheckmunkContext).GetTypeInfo().Assembly;

            const string resourceName = "Checkmunk.Data.SeedData.Users.csv";

            var users = new List<Domain.Checklists.Entities.User>();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var csvReader = new CsvReader(reader);

                    csvReader.Configuration.HasHeaderRecord = true;
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.IgnorePrivateAccessor = true;

                    users.AddRange(csvReader.GetRecords<Domain.Checklists.Entities.User>().ToList());
                }
            }

            return users;
        }

        private static IEnumerable<Checklist> GetTestChecklists(CheckmunkContext context)
        {
            var assembly = typeof(CheckmunkContext).GetTypeInfo().Assembly;

            const string resourceName = "Checkmunk.Data.SeedData.Checklists.csv";

            var checklists = new List<Checklist>();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var csvReader = new CsvReader(reader);

                    csvReader.Configuration.HasHeaderRecord = true;
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.IgnorePrivateAccessor = true;

                    var csvChecklists = csvReader.GetRecords<CsvChecklist>().ToArray();

                    foreach (var csvChecklist in csvChecklists)
                    {
                        var user = context.ChecklistUsers.FirstOrDefault(u => u.EmailAddress.Equals(csvChecklist.CreatedBy));

                        if (user == null)
                        {
                            throw new Exception($"Could not find user with email address:{csvChecklist.CreatedBy} in the seed data.");
                        }

                        var checklist = ChecklistBuilder.Build()
                                                        .WithTitle(csvChecklist.Title)
                                                        .ByUser(user)
                                                        .OnDate(csvChecklist.CreatedAt)
                                                        .Finish();

                        for (var i = 1; i <= RandomValueProvider.RandomInteger(2, 15); i++)
                        {
                            checklist.AddCheckbox(RandomValueProvider.LoremIpsum(RandomValueProvider.RandomInteger(2, 10)));
                        }

                        checklists.Add(checklist);
                    }
                }
            }

            return checklists;
        }

        private static IEnumerable<Domain.Users.AggregateRoots.User> GetTestUsers()
        {
            var assembly = typeof(CheckmunkContext).GetTypeInfo().Assembly;

            const string resourceName = "Checkmunk.Data.SeedData.Users.csv";

            var users = new List<Domain.Users.AggregateRoots.User>();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var csvReader = new CsvReader(reader);

                    csvReader.Configuration.HasHeaderRecord = true;
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.IgnorePrivateAccessor = true;

                    users.AddRange(csvReader.GetRecords<Domain.Users.AggregateRoots.User>().ToList());
                }
            }

            return users;
        }

        private static IEnumerable<Checklist> SeedChecklists(CheckmunkContext context)
        {
            var checklists = GetTestChecklists(context);
            context.Checklists.AddRange(checklists);
            context.SaveChanges();

            return context.Checklists;
        }

        private static IEnumerable<Domain.Checklists.Entities.User> SeedChecklistUsers(CheckmunkContext context)
        {
            var users = GetTestChecklistUsers();
            context.ChecklistUsers.AddRange(users);
            context.SaveChanges();

            return context.ChecklistUsers;
        }

        private static IEnumerable<Domain.Users.AggregateRoots.User> SeedUsers(CheckmunkContext context)
        {
            var users = GetTestUsers();
            context.Users.AddRange(users);
            context.SaveChanges();

            return context.Users;
        }
    }

    internal class CsvChecklist
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}