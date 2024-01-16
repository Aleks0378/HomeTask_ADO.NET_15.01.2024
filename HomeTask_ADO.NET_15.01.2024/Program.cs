//Описать базу данных, в которой хранятся данные о вершинах гор: название, высота, страна местонахождения. 

//1) Создать метод для добавления 1 горы.
//2) Создать метод для для добавления переменного количества гор.
//3) Создать метод для получения всех гор.
//4) Создать метод для получения горы по Id.
//5) Создать метод, в котором удалить из базы данных самую высокую гору.
//6) Создать метод, в котором удалить из базы самую высокую гору заданной страны.


using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;

namespace HomeTask_ADO.NET_15._01._2024
{
    internal class Program
    {
        static string connectionString = "";
        static void Main(string[] args)
        {
            //GetConnectionString("DefaultConnection");
            //CreateDatabase();

            GetConnectionString("DefaultConnection2");

            //CreateTable();
            //InsertIntoTable();

            //Mountain mountain = new Mountain { Name = "Table Mountain", Height = 1085, Country = "South Africa" };
            //InsertIntoTableMountain(mountain);

            //List<Mountain> mountains = new List<Mountain>
            //{
            //    new Mountain { Name = "Mount Kilimanjaro", Height = 5895, Country = "Tanzania" },
            //    new Mountain { Name = "Alpspitze", Height = 2628, Country = "Germany" }
            //};
            //InsertIntoTableMountain(mountains);

            //List<Mountain> mountains = new List<Mountain>();
            //mountains = (List<Mountain>)GetAllMountains();
            //Console.WriteLine("\nAll Mountains:");
            //PrintMountain(mountains);

            //int Id = 10;
            //Mountain mountain = new Mountain();
            //mountain = (Mountain)GetMountainById(Id);
            //if (mountain != null)
            //{
            //    Console.WriteLine($"\nMountain with Id= {Id}:");
            //    PrintMountain(mountain);
            //}
            //else Console.WriteLine("The Mountain Id was not found!");

            //DeleteFromTableTheHighestMountain();

            //DeleteFromTableTheHighestMountain("Nepal");

        }

        private static void GetConnectionString(string connectionPath)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            connectionString = configuration.GetConnectionString(connectionPath);
        }
        private static void CreateDatabase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("CREATE DATABASE Mountains", connection);
                sqlCommand.ExecuteNonQuery();
            }
        }
        private static void CreateTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = """
                    CREATE TABLE [MountainsTop] (
                    [id] Int Primary Key Identity,
                    [Name] Nvarchar(100) Not Null,
                    [Height] Int Not Null,
                    [Country] Nvarchar(100) Not Null);
                    """;
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                int count = sqlCommand.ExecuteNonQuery();
            }
        }
        private static void InsertIntoTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = """
                    INSERT INTO [MountainsTop] (Name, Height, Country) VALUES
                    ('Mount Everest', 8848, 'Nepal'),
                    ('K2', 8611, 'Pakistan'),
                    ('Kangchenjunga', 8586, 'Nepal'),
                    ('Lhotse', 8516, 'Nepal'),
                    ('Makalu', 8485, 'Nepal'),
                    ('Cho Oyu', 8188, 'Nepal'),
                    ('Dhaulagiri', 8167, 'Nepal'),
                    ('Manaslu', 8163, 'Nepal'),
                    ('Nanga Parbat', 8126, 'Pakistan'),
                    ('Annapurna', 8091, 'Nepal');
                    """;
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                int count = sqlCommand.ExecuteNonQuery();
            }
        }

        //1) Создать метод для добавления 1 горы:
        private static void InsertIntoTableMountain(Mountain Mountain)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"""
                    INSERT INTO [MountainsTop] (Name, Height, Country) VALUES
                    ('{Mountain.Name}',{Mountain.Height},'{Mountain.Country}');
                    """;
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                int count = sqlCommand.ExecuteNonQuery();
            }
        }

        //2) Создать метод для для добавления переменного количества гор:
        private static void InsertIntoTableMountain(List<Mountain>Mountains)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var Mountain in Mountains)
                {
                    string sql = $"""
                    INSERT INTO [MountainsTop] (Name, Height, Country) VALUES
                    ('{Mountain.Name}',{Mountain.Height},'{Mountain.Country}');
                    """;
                    SqlCommand sqlCommand = new SqlCommand(sql, connection);
                    int count = sqlCommand.ExecuteNonQuery();
                }
            }
        }

        //3) Создать метод для получения всех гор.
        private static IEnumerable<Mountain> GetAllMountains()
        {
            List<Mountain> mountains = new List<Mountain>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = """
                    SELECT Id, Name, Height, Country From [MountainsTop]
                    """;
                SqlCommand sqlCommand = new SqlCommand(sql, connection);

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            mountains.Add(new Mountain
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Height = reader.GetInt32(2),
                                Country = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return mountains;
        }

        //4) Создать метод для получения горы по Id.
        private static Mountain GetMountainById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"""
                    SELECT Id, Name, Height, Country From [MountainsTop]
                    WHERE Id={Id};
                    """;
                SqlCommand sqlCommand = new SqlCommand(sql, connection);

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Mountain
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Height = reader.GetInt32(2),
                            Country = reader.GetString(3),
                        };
                    }
                }
            }
            return null;
        }

        //5) Создать метод, в котором удалить из базы данных самую высокую гору.
        private static void DeleteFromTableTheHighestMountain()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = """
                    DELETE FROM [MountainsTop]
                    WHERE HEIGHT=(SELECT MAX(HEIGHT) FROM [MountainsTop]);
                    """;
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                sqlCommand.ExecuteNonQuery();
            }
        }

        //6) Создать метод, в котором удалить из базы самую высокую гору заданной страны.
        private static void DeleteFromTableTheHighestMountain(string country)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"""
                    DELETE FROM [MountainsTop]
                    WHERE HEIGHT=(SELECT MAX(HEIGHT) FROM [MountainsTop] WHERE Country='{country}');
                    """;
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                sqlCommand.ExecuteNonQuery();
            }
        }

        private static void PrintMountain(Mountain mountain)
        {
            Console.WriteLine(mountain.ToString());
        }
        private static void PrintMountain(List<Mountain> mountains)
        {
            foreach (Mountain mountain in mountains)
            {
                Console.WriteLine(mountain.ToString());
            }
        }
    }
}
