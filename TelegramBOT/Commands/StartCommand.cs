using MySql.Data.MySqlClient;
using Mysqlx.Prepare;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace TelegramBOT
{
    public class StartCommand : IUpdateHandler
    {

        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            var cmd = new MySqlCommand();
            string usernameToCheck = "some_username";
            string query = $"SELECT COUNT(*) FROM test WHERE tgId = {update.Message.From.Id}";
            MySqlCommand check = new MySqlCommand(query, conn);
            check.Parameters.AddWithValue($"{update.Message.From.Id}", usernameToCheck);
            int userCount = Convert.ToInt32(check.ExecuteScalar());

            if (userCount > 0)
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, $"Такой профиль уже есть");
            }
            else
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Профиль зарегистрирован введите 'Меню' для открытия меню");
                cmd.Connection = conn;
                cmd.CommandText = $"INSERT INTO test(id, tgId, name, role, money, moneyBank, lvl, farm, phone) VALUES('2', '{update.Message.From.Id}','{update.Message.Chat.FirstName}', 'User', '100', '0', '1', 'Нету', 'Нету')";
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Профиль с ником {update.Message.Chat.FirstName} создан");
            }
            conn.Close();
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
