using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBOT.Commands.User.Bank
{
    public class ButtonsBank : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var bankKeyboard = new ReplyKeyboardMarkup(

                    new List<KeyboardButton[]>()
                    {
                        new KeyboardButton[]
                    {
                        new KeyboardButton("Баланс"),
                        new KeyboardButton("Пополнить"),
                        new KeyboardButton("Вывести")
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Меню")
                    }
                    })
            {
                ResizeKeyboard = true,
            };
            await client.SendTextMessageAsync(update.Message.Chat.Id, "Bank", replyMarkup: bankKeyboard);
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
