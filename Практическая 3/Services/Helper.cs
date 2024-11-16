using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Практическая_3.Models;

namespace Практическая_3.Services
{
    public class Helper
    {
        public static MusicRecordEntities _context;

        public static MusicRecordEntities GetContext()
        {
            if (_context == null)
                _context = new MusicRecordEntities();
            return _context;
        }

        public void CreateUser(UserAccounts user)
        {
            _context.UserAccounts.Add(user);
            _context.SaveChanges();
        }
        public void UpdateUser(UserAccounts user)
        {
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public void RemoveUser(int idUser)
        {
            var users = _context.UserAccounts.Find(idUser);
            _context.UserAccounts.Remove(users);
            _context.SaveChanges();
        }
    }
}
