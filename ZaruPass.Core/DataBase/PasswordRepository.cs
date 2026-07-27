using System.Collections;
using System.Security.Policy;
using ZaruPass.Core.DataBase.Migrations;

namespace ZaruPass.Core.DataBase
{
    public class PasswordRepository
    {
        public PasswordRepository()
        {
            using (var context = new MyContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public byte[]? GetPasswordBySite(string site)
        {
            using (var context = new MyContext())
            {
                var data = context.Passwords.Where(entity => entity.Site == site).FirstOrDefault();
                return data?.Pass;
            }
        }

        public void AddPassword(string site, byte[] password)
        {
            using (var context = new MyContext())
            {
                var data = new Password() { Pass = password, Site = site };
                context.Passwords.Add(data);
                context.SaveChanges();
            }
        }

        public IEnumerable<string> GetSites()
        {
            using (var context = new MyContext())
            {
                List<string> list = new List<string>();
                foreach(var item in context.Passwords)
                {
                    list.Add(item.Site);
                }
                return list;
            }
        }
    }
}
