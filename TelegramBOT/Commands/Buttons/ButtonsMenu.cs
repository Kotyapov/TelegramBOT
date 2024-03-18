using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBOT.Commands.ButtonKeyboard
{
    public class ButtonsMenu : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            var cmd = new MySqlCommand();
            string sqlQuery = $"SELECT role FROM test WHERE tgId = {update.Message.From.Id}";
            MySqlCommand command = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string role = reader["role"].ToString();
            reader.Close();

            Roles r = new Roles();
            string AdminRole = r.AdminRole();

            if (role == AdminRole)
            {
                var replyKeyboard = new ReplyKeyboardMarkup(

                new List<KeyboardButton[]>()
                {
                            new KeyboardButton[]
                            {
                                new KeyboardButton("Привет!"),
                                new KeyboardButton("Профиль"),
                                new KeyboardButton("Банк"),
                                new KeyboardButton("Магазин"),
                                new KeyboardButton("Админ команды")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("Фермы")
                            }
                })
                {
                    ResizeKeyboard = true,
                };
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Это меню", replyMarkup: replyKeyboard);
            }
            else
            {

                var replyKeyboard = new ReplyKeyboardMarkup(

                new List<KeyboardButton[]>()
                {
                            new KeyboardButton[]
                            {
                                new KeyboardButton("Привет!"),
                                new KeyboardButton("Профиль"),
                                new KeyboardButton("Банк"),
                                new KeyboardButton("Магазин")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("Фермы")
                            }
                })
                {
                    ResizeKeyboard = true,
                };
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Это меню", replyMarkup: replyKeyboard);
            }
            conn.Close();
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
