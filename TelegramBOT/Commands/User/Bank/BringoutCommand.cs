using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace TelegramBOT.Commands.User
{
    public class BringoutCommand : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            var cmd = new MySqlCommand();
            string sqlQuery = $"SELECT tgId, money, moneyBank FROM test WHERE tgId = {update.Message.From.Id}";
            MySqlCommand command = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string tgId = reader["tgId"].ToString();
            int money = Convert.ToInt32(reader["money"].ToString());
            int moneyBank = Convert.ToInt32(reader["moneyBank"].ToString());
            reader.Close();

            string[] words = update.Message.Text.Split(' ');
            if (words.Length > 1)
            {
                int value;
                if (int.TryParse(words[1], out value))
                {
                    if (moneyBank >= value)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = $"UPDATE test SET moneyBank = moneyBank - '{value}', money = money + '{value}' WHERE tgId = {update.Message.From.Id}";
                        cmd.ExecuteNonQuery();
                        await client.SendTextMessageAsync(update.Message.Chat.Id, $"Вы вывели со своего банковсого счета {value}р");
                    }
                    else
                    {
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Недостаточно средств на вашем счету");
                    }

                }
                else
                {
                    await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите число, а не слово или букву");
                }
            }
            else
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Введите например: Вывести 100");
            }
            conn.Close();
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
