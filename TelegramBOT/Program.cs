using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Data.SqlClient;
using Mysqlx.Crud;
using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Metrics;
using TelegramBOT.Commands.User;
using TelegramBOT.Commands.ButtonKeyboard;
using TelegramBOT.Commands.User.Bank;
using TelegramBOT.Commands.Buttons;
using TelegramBOT.Utils;

namespace TelegramBOT
{
    internal class Program 
    {
        static void Main(string[]args)
        {
            //commands
            StartCommand startCommand = new StartCommand();
            ProfileCommand profileCommand = new ProfileCommand();
            ButtonsMenu buttonsMenu = new ButtonsMenu();
            ButtonsBank buttonsBank = new ButtonsBank();
            BalanceCommand balanceCommand = new BalanceCommand();
            ReplenishCommand replenishCommand = new ReplenishCommand();
            BringoutCommand bringoutCommand = new BringoutCommand();
            ButtonsShop buttonsShop = new ButtonsShop();
            BuyPhone buyPhone = new BuyPhone();
            MyFarm myFarm = new MyFarm();
            ButtonsFarm buttonsFarm = new ButtonsFarm();
            BuyFarmCommand buyFarmCommand = new BuyFarmCommand();

            MainHandler mainHandler = new MainHandler(
                startCommand,
                profileCommand,
                buttonsMenu,
                buttonsBank,
                balanceCommand,
                replenishCommand,
                bringoutCommand,
                buttonsShop,
                buyPhone,
                buttonsFarm,
                myFarm,
                buyFarmCommand);
            //Main
            DataBase db = new DataBase();
            ErrorDebug err = new ErrorDebug();
            Farm farm = new Farm();
            VIP vip = new VIP();
            var client = new TelegramBotClient("TOKEN");
            client.StartReceiving(mainHandler);
            var me = client.GetMeAsync().Result;
            //Info
            Console.WriteLine("==================");
            Thread log = Thread.CurrentThread;
            log.Name = $"Телеграм бот {me.Username}";
            Console.WriteLine($"Имя потока: {log.Name}");
            Console.WriteLine($"Запущен ли поток: {log.IsAlive}");
            Console.WriteLine($"Id потока: {log.ManagedThreadId}");
            Console.WriteLine($"Приоритет потока: {log.Priority}");
            Console.WriteLine($"Статус потока: {log.ThreadState}");
            db.Connect();
            Console.WriteLine("==================");
            farm.Farms();
            Console.ReadLine();
            //MainStart
        }

    }
}


