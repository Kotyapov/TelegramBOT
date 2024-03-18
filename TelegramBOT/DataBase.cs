using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Telegram.Bot.Types;
using Telegram.Bot;
using Mysqlx.Crud;

namespace TelegramBOT
{
    internal class DataBase
    {

        public string connStr = "server=localhost;user=root;database=test;password=;";

        public void Connect()
        {
            
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            var stm = "SELECT VERSION()";
            MySqlCommand command = new MySqlCommand(stm, conn);
            Console.WriteLine($"Статус БД: Подключено");
            conn.Close();
        }

    }
}

