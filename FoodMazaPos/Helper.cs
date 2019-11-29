using System.Configuration;

namespace Foodies
{
    class Helper
    {
        public static string con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}
