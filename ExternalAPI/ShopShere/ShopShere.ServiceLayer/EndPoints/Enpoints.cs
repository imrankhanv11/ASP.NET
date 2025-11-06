using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.ServiceLayer.EndPoints
{
    public static class Enpoints
    {
        public const string GetAllBooks = "api/Books/GellAllBooks";

        public const string GetOneBookById = "api/Books/GetByID";

        public const string GetOneBookByName = "api/Books/GetBookByName";

        public const string DeleteBook = "api/Books/DeleteBook";

        public const string AddBok = "api/Books/Addbook";
    }
}
