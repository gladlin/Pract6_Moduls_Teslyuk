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
        /// <summary>
        /// Дает возможность для взаимодействия с бд
        /// </summary>
        /// <returns>Контекст базы данных.</returns>
        
        public static MusicRecordEntities _context;

        public static MusicRecordEntities GetContext()
        {
            if (_context == null)
                _context = new MusicRecordEntities();
            return _context;
        }
    }
}
