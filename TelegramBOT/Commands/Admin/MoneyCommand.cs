using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading;
using Telegram.Bot.Types.ReplyMarkups;
using System.Data;
using MySql.Data.MySqlClient;

namespace TelegramBOT.Commands.Admin
{
    public class MoneyCommand : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            var cmd = new MySqlCommand();
            string sqlQuery = $"SELECT name, role, money FROM test WHERE tgId = {update.Message.From.Id}";
            MySqlCommand command = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string name = reader["name"].ToString();
            string role = reader["role"].ToString();
            int money = Convert.ToInt32(reader["money"].ToString());
            reader.Close();
            Roles r = new Roles();
            string AdminRole = r.AdminRole();

            string[] words = update.Message.Text.Split(' ');
            if (words.Length > 2)
            {
                if (words.Length > 1)
                {
                    string user = words[1];
                    int value;
                    if (int.TryParse(words[2], out value))
                    {
                        if (role == AdminRole)
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = $"UPDATE test SET money = money + '{value}' WHERE name = '{user}'";
                            cmd.ExecuteNonQuery();
                            await client.SendTextMessageAsync(update.Message.Chat.Id, $"Пользователю с ником {user} дали {value}р");
                        }
                        else
                        {
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Недостаточно прав");
                        }
                    }
                    else
                    {
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Ввведите сумму, а не букву или слово");
                    }
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите например: /givemoney Котяпов 100");
                }
            }
            else
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Вы не указали сумму");
            }
            conn.Close();
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
