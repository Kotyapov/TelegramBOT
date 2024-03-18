using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegramBOT.Commands.Admin;
using TelegramBOT.Commands.ButtonKeyboard;
using TelegramBOT.Commands.Buttons;
using TelegramBOT.Commands.User;
using TelegramBOT.Commands.User.Bank;

namespace TelegramBOT
{
    public class MainHandler : IUpdateHandler
    {
        private readonly StartCommand _startCommand;
        private readonly ProfileCommand _profileCommand;
        private readonly ButtonsMenu _buttonsMenu;
        private readonly ButtonsBank _buttonsBank;
        private readonly BalanceCommand _balanceCommand;
        private readonly ReplenishCommand _replenishCommand;
        private readonly BringoutCommand _bringoutCommand;
        private readonly ButtonsShop _buttonsShop;
        private readonly BuyPhone _buyPhone;
        private readonly ButtonsFarm _buttonsFarm;
        private readonly MyFarm _myFarm;
        private readonly BuyFarmCommand _buyFarmCommand;
        private readonly MoneyCommand _moneyCommand;

        public MainHandler(
            StartCommand startCommand,
            ProfileCommand profileCommand,
            ButtonsMenu buttonsMenu,
            ButtonsBank buttonsBank,
            BalanceCommand balanceCommand,
            ReplenishCommand replenishCommand,
            BringoutCommand bringoutCommand,
            ButtonsShop buttonsShop,
            BuyPhone buyPhone,
            ButtonsFarm buttonsFarm,
            MyFarm myFarm,
            BuyFarmCommand buyFarmCommand
            )
        {
            _startCommand = startCommand;
            _profileCommand = profileCommand;
            _buttonsMenu = buttonsMenu;
            _buttonsBank = buttonsBank;
            _balanceCommand = balanceCommand;
            _replenishCommand = replenishCommand;
            _bringoutCommand = bringoutCommand;
            _buttonsShop = buttonsShop;
            _buyPhone = buyPhone;
            _buttonsFarm = buttonsFarm;
            _myFarm = myFarm;
            _buyFarmCommand = buyFarmCommand;
        }
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            /*Dictionary<string, Func<Update, Task>> commandHandlers = new Dictionary<string, Func<Update, Task>>
{
    { "/start", (update) => _startCommand.HandleUpdateAsync(client, update, token) },
    { "профиль", (update) => _profileCommand.HandleUpdateAsync(client, update, token) },
};

if(commandHandlers.ContainsKey(update.Message.Text.ToLower()))
{
    await commandHandlers[update.Message.Text.ToLower()](update);
}*/
            if (update.Message.Text.StartsWith("/start"))
            {
                await _startCommand.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("профиль"))
            {
                await _profileCommand.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("меню"))
            {
                await _buttonsMenu.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("банк"))
            {
                await _buttonsBank.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("баланс"))
            {
                await _balanceCommand.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("пополнить"))
            {
                await _replenishCommand.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("вывести"))
            {
                await _bringoutCommand.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("магазин"))
            {
                await _buttonsShop.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("телефон"))
            {
                await _buyPhone.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("фермы"))
            {
                await _buttonsFarm.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("моя ферма"))
            {
                await _myFarm.HandleUpdateAsync(client, update, token);
            }
            else if (update.Message.Text.ToLower().Contains("купить"))
            {
                await _buyFarmCommand.HandleUpdateAsync(client, update, token);
            }

        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
