using Microsoft.EntityFrameworkCore;
using ZaruPass.Core.DataBase.Migrations;

namespace ZaruPass.Core.DataBase
{
    public class PasswordRepository
    {
        private MyContext _context;
        public PasswordRepository()
        {
            _context = new MyContext();
            _context.Database.EnsureCreated();
        }

        public IEnumerable<Password> GetPasswords()
        {
            return _context.Passwords.ToList();
        }

        public void ClearPasswords()
        {
            _context.Passwords.ExecuteDelete();
        }

        public void DeletePassword(Password password)
        {
            _context.Passwords.Remove(password);
            _context.SaveChanges();
        }

        public void DeletePassword(string site)
        {
            var password = _context.Passwords.Where(p => p.Site == site).FirstOrDefault();
            if(password is not null)
            {
                _context.Passwords.Remove(password);
                _context.SaveChanges();
            }
        }

        public Password? GetBySite(string site)
        {
            return _context.Passwords.Where(entity => entity.Site == site).FirstOrDefault();
        }

        public byte[]? GetPasswordBySite(string site)
        {
            var data = _context.Passwords.Where(entity => entity.Site == site).FirstOrDefault();
            return data?.Pass;
        }

        public void AddPassword(string site, byte[] password)
        {
            var data = new Password() { Pass = password, Site = site };
            _context.Passwords.Add(data);
            _context.SaveChanges();
        }

        public IEnumerable<string> GetSites()
        {
            List<string> list = new List<string>();
            foreach (var item in _context.Passwords)
            {
                list.Add(item.Site);
            }
            return list;
        }

        public void UpdatePassword(string site, byte[] newPassword)
        {
            var password = _context.Passwords.Where(p => p.Site == site).FirstOrDefault();
            password?.Pass = newPassword;
            _context.SaveChanges();
        }
    }
}
