using System.Threading.Tasks;
using MySqlConnector;

namespace ApplicationUP.Core
{
    public class Connector
    {
        // Строка подключения
        private static string Link =
        "Server=37.18.74.116;" +
        "Port=3309;" +
        "UserID=p101_l_darya;" +
        "Password=Qwerty123;" +
        "Database=p101_db_l_darya;" +
        "CharacterSet=utf8mb4;" +
        "ConvertZeroDatetime=True;" +
        "AllowZeroDatetime=True;";


        // Объект подключения
        private readonly MySqlConnection Conn = new MySqlConnection(Link);

        // Метод асинхронного подключения к БД с объекта подключения 
        public async Task GetOpen() => await Conn.OpenAsync();

        // Метод асинхронного отключения от БД с объекта подключения
        public async Task GetClose() => await Conn.CloseAsync();

        // Метод возвращения объекта подключения
        public MySqlConnection GetConn() => Conn;
    }
}
