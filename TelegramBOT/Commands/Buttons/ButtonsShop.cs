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

namespace TelegramBOT.Commands.Buttons
{
    public class ButtonsShop : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var replyKeyboard = new ReplyKeyboardMarkup(

            new List<KeyboardButton[]>()
            {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Телефон 1"),
                        new KeyboardButton("Телефон 2"),
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Меню")
                    }
            })
            {
                ResizeKeyboard = true,
            };

            await client.SendTextMessageAsync(update.Message.Chat.Id, "Выберите телефон:\n1.Iphone 15 PRO Цена: 15000\n2.Samsung s10+", replyMarkup: replyKeyboard);
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
