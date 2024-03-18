using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBOT.Commands.Buttons
{
    public class ButtonsFarm :IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var farmKeyboard = new ReplyKeyboardMarkup(

                    new List<KeyboardButton[]>()
                    {
                        new KeyboardButton[]
                    {
                        new KeyboardButton("Моя ферма"),
                        new KeyboardButton("Купить ферму"),
                        new KeyboardButton("Продать ферму")
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Меню")
                    }
                    })
            {
                ResizeKeyboard = true,
            };
            await client.SendTextMessageAsync(update.Message.Chat.Id, "Список ферм\nДля покупки бизнеса введите купить НАЗВАНИЕ БИЗНЕСА\ntest - 1р\ntest2 - 2р", replyMarkup: farmKeyboard);
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
