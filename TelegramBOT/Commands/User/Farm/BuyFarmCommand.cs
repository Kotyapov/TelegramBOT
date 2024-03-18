using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
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
    public class BuyFarmCommand : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            var cmd = new MySqlCommand();

            string[] allowedFarmNames = { "test", "tests" };

            string[] words = update.Message.Text.Split(' ');

            if (words.Length < 1)
            {
                Console.WriteLine("error");
                conn.Close();
                return;
            }
            string commandFarmName = words[1];
            if (!allowedFarmNames.Contains(commandFarmName.ToLower()))
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Такого бизнеса нет");
                conn.Close();
                return;
            }

            string sqlQuery = $"SELECT test.money, test.farm, farms.farmName, farms.farmPrice FROM test JOIN farms WHERE test.tgId = '{update.Message.From.Id}' AND farms.farmName = '{commandFarmName}'";
            MySqlCommand command = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int dbMoney = Convert.ToInt32(reader["money"].ToString());
            int dbPriceFarm = Convert.ToInt32(reader["farmPrice"].ToString());
            string dbFarmName = reader["farmName"].ToString();
            string dbFarmNameUser = reader["farm"].ToString();

            reader.Close();

            if (commandFarmName == dbFarmNameUser)
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, $"У вас уже имеется бизнес {commandFarmName}");
                conn.Close();
                return;
            }
            if (dbFarmNameUser != "Нету")
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "У вас уже имеется бизнес");
                conn.Close();
                return;
            }
            if (dbMoney < dbPriceFarm)
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Недостаточно средств");
                conn.Close();
                return;
            }
            cmd.Connection = conn;
            cmd.CommandText = $"UPDATE test JOIN farms SET test.farm = farms.farmName, test.money = test.money - farms.farmPrice WHERE test.tgId = '{update.Message.From.Id}' AND farms.farmName = '{commandFarmName}'";
            cmd.ExecuteNonQuery();
            await client.SendTextMessageAsync(update.Message.Chat.Id, $"Вы купили бизнес {commandFarmName}");
            conn.Close();
            return;

        }


        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
