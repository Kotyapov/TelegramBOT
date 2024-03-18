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
using MySql.Data.MySqlClient;

namespace TelegramBOT.Commands.User
{
    public class BalanceCommand : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            var cmd = new MySqlCommand();
            string sqlQuery = $"SELECT moneyBank FROM test WHERE tgId = {update.Message.From.Id}";
            MySqlCommand command = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int moneyBank = Convert.ToInt32(reader["moneyBank"].ToString());
            reader.Close();

            await client.SendTextMessageAsync(update.Message.Chat.Id, $"Ваш баланс в банке {moneyBank}р");
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}