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
    public class BuyPhone : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            var cmd = new MySqlCommand();
            string sqlQuery = $"SELECT tgId, money, phone FROM test WHERE tgId = {update.Message.From.Id}";
            MySqlCommand command = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string tgId = reader["tgId"].ToString();
            int money = Convert.ToInt32(reader["money"].ToString());
            string phone = reader["phone"].ToString();
            reader.Close();
            string[] words = update.Message.Text.Split(' ');
            if (words.Length > 1)
            {
                int value = Convert.ToInt32(words[1]);
                if (phone == "Нету") {
                    switch (value)
                    {
                        case 1:
                            if (money >= 15000)
                            {
                                await client.SendTextMessageAsync(update.Message.Chat.Id, "Вы купили Iphone 15 pro");
                                cmd.Connection = conn;
                                cmd.CommandText = $"UPDATE test SET phone = 'Iphone 15 pro', money = money - 15000 WHERE tgId = '{update.Message.From.Id}'";
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                await client.SendTextMessageAsync(update.Message.Chat.Id, "Недостаточно средств");
                            }
                            break;
                        case 2:
                            if (money >= 10000)
                            {
                                await client.SendTextMessageAsync(update.Message.Chat.Id, "Вы купили Samsung s10+");
                                cmd.Connection = conn;
                                cmd.CommandText = $"UPDATE test SET phone = 'Samsung s10+', money = money - 10000 WHERE tgId = '{update.Message.From.Id}'";
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                await client.SendTextMessageAsync(update.Message.Chat.Id, "Недостаточно средств");
                            }
                            break;
                        default:
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Такого телефона нету");
                            break;
                    }
                } else
                {
                    await client.SendTextMessageAsync(update.Message.Chat.Id, "У вас уже есть телефон");
                }
            } else
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Вы не выбрали телефон");
            }

            
            conn.Close();
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
