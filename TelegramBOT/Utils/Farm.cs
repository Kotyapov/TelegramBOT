using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramBOT
{
    public class Farm
    {
        public void Farms() 
        {
            Console.WriteLine("Статус ферм: Запущено");
            Console.WriteLine("==================");
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            string sqlQuery = $"SELECT farm FROM test";
            MySqlCommand command = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string farm = reader["farm"].ToString();
            reader.Close();
            if(farm == "tests")
            {
                int num = 0;
                TimerCallback tm = new TimerCallback(Count);
                Timer timer = new Timer(tm, num, 0, 2000);
                Console.ReadLine();
            } 

            if (farm == "test")
            {
                int num = 0;
                TimerCallback tm = new TimerCallback(Count2);
                Timer timer = new Timer(tm, num, 0, 2000);
                Console.ReadLine();
            } 
        }

        public static void Count(object obj)
        {
            obj = 1;
            int x = (int)obj;
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            var cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"UPDATE test SET money = money + '{x}' WHERE farm = 'tests'";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Count2(object obj)
        {
            obj = 1;
            int x = (int)obj;
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            var cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"UPDATE test SET money = money + '{x}' WHERE farm = 'test'";
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
