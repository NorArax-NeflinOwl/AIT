using AIT.DataBases;
using AIT.DataBases.DBModel;
using AITLib.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIT.Helpers
{
    public class AitAdminManager
    {
        public static string Title(AitPerson admin)
        {
            return admin.Login.ToUpperInvariant();
        }

        public static async Task<AitPerson> CreateAdminIfNotExist()
        {
            string login = "admin";
            AitPerson admin;
            var person = AitDBContextInstance.Instance.AitPersons.Where(p => p.Login == login).ToDictionary(n => n.ID).FirstOrDefault(q => q.Value.Login.Equals(login));
            if (null == person.Value)
            {
                admin = await CreateAdminAccount(login);
                AitRememberHelper.CreateMemoryLogin(admin);
            }
            else
            {
                admin = person.Value;
            }

            return admin;
        }

        private static async Task<AitPerson> CreateAdminAccount(string login)
        {
            var person = new AitPerson
            {
                Login = login,
                Password = AitGenerators.GenerateMD5Hash(login),
            };

            var personDetail = new AitPersonsDetail
            {
                PersonID = person.ID,
                BDate = DateTime.Now,
                Person = person
            };
            var quickNotes = new List<AitQuickNote>();

            person.PersonalDetails = personDetail;
            person.QuickNotes = quickNotes;
            AitDBContextInstance.Instance.AitPersons.Add(person);
            await AitDBContextInstance.Instance.SaveChangesAsync();

            return person;
        }
    }
}
