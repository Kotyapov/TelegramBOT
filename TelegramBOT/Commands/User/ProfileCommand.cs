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
    public class ProfileCommand : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            DateTime now = DateTime.Now;
            conn.Open();
            var cmd = new MySqlCommand();
            string sqlQuery = $"SELECT tgId, name, role, money, moneyBank, lvl, farm, phone FROM test WHERE tgId = {update.Message.From.Id}";
            MySqlCommand command = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string tgId = reader["tgId"].ToString();
            string name = reader["name"].ToString();
            string role = reader["role"].ToString();
            int money = Convert.ToInt32(reader["money"].ToString());
            int moneyBank = Convert.ToInt32(reader["moneyBank"].ToString());
            string lvl = reader["lvl"].ToString();
            string farm = reader["farm"].ToString();
            string phone = reader["phone"].ToString();
            reader.Close();

            await client.SendTextMessageAsync(update.Message.Chat.Id, $"Дата и Время: {now.ToString("D")}\ntgId: {tgId}\nНик: {name}\nРоль: {role}\nУровень: {lvl}\nДеньги: {money}р\nДеньги в банке: {moneyBank}р\nМайнинг ферма: {farm}\nТелефон: {phone}");
            conn.Close();
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
