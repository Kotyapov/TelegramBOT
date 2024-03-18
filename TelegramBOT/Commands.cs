using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Mysqlx.Crud;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Text.RegularExpressions;
using Google.Protobuf.WellKnownTypes;

namespace TelegramBOT
{
    public class Commands1
    {
        public async Task Update(ITelegramBotClient client, Telegram.Bot.Types.Update update, CancellationToken token)
        {
            var mess = update.Message;
            DataBase db = new DataBase();
            MySqlConnection conn = new MySqlConnection(db.connStr);
            conn.Open();
            var cmd = new MySqlCommand();

            //Roles
            string UserRole = "User";
            string AdminRole = "Admin";

            //businessName


            //SleshCommand
            if (mess.Text == "/start")
            {
                string usernameToCheck = "some_username";
                string query = $"SELECT COUNT(*) FROM test WHERE tgId = {mess.From.Id}";
                MySqlCommand check = new MySqlCommand(query, conn);
                check.Parameters.AddWithValue($"{mess.From.Id}", usernameToCheck);
                int userCount = Convert.ToInt32(check.ExecuteScalar());

                if (userCount > 0)
                {
                    await client.SendTextMessageAsync(mess.Chat.Id, "Такой профиль уже есть");
                }
                else
                {
                    await client.SendTextMessageAsync(mess.Chat.Id, "Профиль зарегистрирован введите 'Меню' для открытия меню");
                    cmd.Connection = conn;
                    cmd.CommandText = $"INSERT INTO test(id, tgId, name, role, money, moneyBank, lvl, phone) VALUES('2', '{mess.From.Id}','{mess.Chat.FirstName}', 'User', '100', '0', '1', 'Нету')";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"Профиль с ником {mess.Chat.FirstName} создан");
                }


            }

            //Select

            string sqlQuery = $"SELECT tgId, name, role, money, moneyBank, lvl, business, phone FROM test WHERE tgId = {mess.From.Id}";
            MySqlCommand command = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string tgId = reader["tgId"].ToString();
            string name = reader["name"].ToString();
            string role = reader["role"].ToString();
            int money = Convert.ToInt32(reader["money"].ToString());
            int moneyBank = Convert.ToInt32(reader["moneyBank"].ToString());
            string lvl = reader["lvl"].ToString();
            string business = reader["business"].ToString();
            string phone = reader["phone"].ToString();
            reader.Close();


            //TextCommand
            if (mess.Text != null)
            {
                if (mess.Text.ToLower().Contains("привет"))
                {
                    await client.SendTextMessageAsync(mess.Chat.Id, $"Привет, {mess.Chat.FirstName}!"); ;
                    return;
                }
            }


            if (mess.Text != null)
            {
                if (mess.Text.ToLower().Contains("профиль"))
                {

                    await client.SendTextMessageAsync(mess.Chat.Id, $"tgId: {tgId}\nНик: {name}\nРоль: {role}\nДеньги: {money}р\nДеньги в банке: {moneyBank}р\nУровень: {lvl}\nБизнес: {business}\nТелефон: {phone}");
                }
            }

            if (mess.Text != null)
            {

                if (mess.Text.ToLower().Contains("меню"))
                {
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
                                new KeyboardButton("Бизнес")
                            }
                        })
                        {
                            ResizeKeyboard = true,
                        };
                        await client.SendTextMessageAsync(mess.Chat.Id, "Это меню", replyMarkup: replyKeyboard);
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
                                new KeyboardButton("Бизнес")
                            }
                        })
                        {
                            ResizeKeyboard = true,
                        };
                        await client.SendTextMessageAsync(mess.Chat.Id, "Это меню", replyMarkup: replyKeyboard);
                    }
                }
            }
            //Bank
            if (mess.Text != null)
            {
                if (mess.Text.ToLower().Contains("банк"))
                {
                    var bankKeyboard = new ReplyKeyboardMarkup(

                    new List<KeyboardButton[]>()
                    {
                        new KeyboardButton[]
                    {
                        new KeyboardButton("Баланс"),
                        new KeyboardButton("Пополнить"),
                        new KeyboardButton("Вывести")
                    }
                    })
                    {
                        ResizeKeyboard = true,
                    };
                    await client.SendTextMessageAsync(mess.Chat.Id, "Bank", replyMarkup: bankKeyboard);
                }
            }

            if (mess.Text != null)
            {
                if (mess.Text.ToLower().Contains("баланс"))
                {
                    await client.SendTextMessageAsync(mess.Chat.Id, $"Ваш баланс в банке {moneyBank}р");
                }
            }

            if (mess.Text != null)
            {
                if (mess.Text.ToLower().Contains("пополнить"))
                {
                    string[] words = mess.Text.Split(' ');
                    if (words.Length > 1)
                    {
                        int value;
                        if (int.TryParse(words[1], out value))
                        {
                            if (money >= value)
                            {
                                cmd.Connection = conn;
                                cmd.CommandText = $"UPDATE test SET money = money - '{value}', moneyBank = moneyBank + '{value}' WHERE tgId = '{mess.From.Id}'";
                                cmd.ExecuteNonQuery();
                                await client.SendTextMessageAsync(mess.Chat.Id, $"Вы пополнили свой счет в банке на {value}р");
                            }
                            else
                            {
                                await client.SendTextMessageAsync(mess.Chat.Id, $"Недостаточно средств на балансе Ваш баланс: {money}");
                            }
                        }
                        else
                        {
                            await client.SendTextMessageAsync(mess.Chat.Id, "Введите сумму а не букву или слово");
                        }
                    }
                    else
                    {
                        await client.SendTextMessageAsync(mess.Chat.Id, "Введите Например: Пополнить 100");
                    }
                }
            }

            if (mess.Text != null)
            {
                if (mess.Text.ToLower().Contains("вывести"))
                {
                    string[] words = mess.Text.Split(' ');
                    if (words.Length > 1)
                    {
                        int value;
                        if (int.TryParse(words[1], out value))
                        {
                            if (moneyBank >= value)
                            {
                                cmd.Connection = conn;
                                cmd.CommandText = $"UPDATE test SET moneyBank = moneyBank - '{value}', money = money + '{value}' WHERE tgId = {mess.From.Id}";
                                cmd.ExecuteNonQuery();
                                await client.SendTextMessageAsync(mess.Chat.Id, $"Вы вывели со своего банковсого счета {value}р");
                            }
                            else
                            {
                                await client.SendTextMessageAsync(mess.Chat.Id, "Недостаточно средств на вашем счету");
                            }

                        }
                        else
                        {
                            await client.SendTextMessageAsync(mess.Chat.Id, "Введите число, а не слово или букву");
                        }
                    }
                    else
                    {
                        await client.SendTextMessageAsync(mess.Chat.Id, "Введите например: Вывести 100");
                    }
                }
            }

            /*if (mess.Text != null)
            {
                if (mess.Text.ToLower().Contains("bansss"))
                {
                    string[] words = mess.Text.Split(' ');
                    if (words.Length > 1)
                    {
                        int value;
                        if (int.TryParse(words[1], out value))
                        {
                            Console.WriteLine(value);
                        }
                        else
                        {
                            Console.WriteLine("no");
                        }
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }
                }
                
            }*/

            //Shop
            if (mess.Text != null)
            {
                if (mess.Text.ToLower().Contains("магазин"))
                {
                    var replyKeyboard = new ReplyKeyboardMarkup(

                    new List<KeyboardButton[]>()
                    {
                        new KeyboardButton[]
                    {
                        new KeyboardButton("Iphone 15 PRO PRICE: 15.000"),
                        new KeyboardButton("Меню"),
                    }
                    })
                    {
                        ResizeKeyboard = true,
                    };

                    await client.SendTextMessageAsync(mess.Chat.Id, "Магазин", replyMarkup: replyKeyboard);
                }
            }
            if (mess.Text != null)
            {
                if (mess.Text.ToLower().Contains("iphone 15 pro price: 15.000"))
                {
                    if (money >= 15000)
                    {
                        await client.SendTextMessageAsync(mess.Chat.Id, "Вы купили Iphone 15 pro");
                        cmd.Connection = conn;
                        cmd.CommandText = $"UPDATE test SET phone = 'Iphone 15 pro', money = money - 15000 WHERE tgId = '{mess.From.Id}'";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        await client.SendTextMessageAsync(mess.Chat.Id, "Недостаточно средств");
                    }
                }
            }

            //Business
                if (mess.Text.ToLower().Contains("бизнес"))
                {
                    var businessKeyboard = new ReplyKeyboardMarkup(

                    new List<KeyboardButton[]>()
                    {
                        new KeyboardButton[]
                    {
                        new KeyboardButton("Мой бизнес"),
                        new KeyboardButton("Купить бизнес"),
                        new KeyboardButton("Продать бизнес")
                    }
                    })
                    {
                        ResizeKeyboard = true,
                    };
                    await client.SendTextMessageAsync(mess.Chat.Id, "Список бизнесов\nДля покупки бизнеса введите купить НАЗВАНИЕ БИЗНЕСА\ntest - 1р\ntest2 - 2р", replyMarkup: businessKeyboard);

                }
           


                if(mess.Text.ToLower().Contains("купить"))
                {
                string[] words = mess.Text.Split(' ');
                if (words.Length > 1)
                {
                    string value = words[1];
                    if (value.ToLower() == "test")
                    {
                        if (value == "test")
                        {
                            if (business == "Нету")
                            {
                                if (business != "test")
                                {
                                    if (money >= 1)
                                    {
                                        cmd.Connection = conn;
                                        cmd.CommandText = $"UPDATE test SET business = '{value}', money = money - 1 WHERE tgId = '{mess.From.Id}'";
                                        cmd.ExecuteNonQuery();
                                        await client.SendTextMessageAsync(mess.Chat.Id, "Вы купили бизнес test");
                                    }
                                    else
                                    {
                                        await client.SendTextMessageAsync(mess.Chat.Id, "Недостаточно средств");
                                    }
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(mess.Chat.Id, "У вас уже имеется бизнес test");
                                }
                            }
                            else
                            {
                                await client.SendTextMessageAsync(mess.Chat.Id, "У вас уже имеется бизнес");
                            }
                        }
                    }
                    else
                    {
                        await client.SendTextMessageAsync(mess.Chat.Id, "Такого бизнеса нет");
                    }

                    if (value.ToLower() == "test1")
                    {
                        if (value == "test1")
                        {
                            if (business == "Нету")
                            {
                                if (business != "test1")
                                {
                                    if (money >= 1)
                                    {
                                        cmd.Connection = conn;
                                        cmd.CommandText = $"UPDATE test SET business = '{value}', money = money - 1 WHERE tgId = '{mess.From.Id}'";
                                        cmd.ExecuteNonQuery();
                                        await client.SendTextMessageAsync(mess.Chat.Id, "Вы купили бизнес test1");
                                    }
                                    else
                                    {
                                        await client.SendTextMessageAsync(mess.Chat.Id, "Недостаточно средств");
                                    }
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(mess.Chat.Id, "У вас уже имеется бизнес test1");
                                }
                            }
                            else
                            {
                                await client.SendTextMessageAsync(mess.Chat.Id, "У вас уже имеется бизнес");
                            }
                        }
                        else
                        {
                            await client.SendTextMessageAsync(mess.Chat.Id, "Такого бизнеса нету");
                        }
                    }
                } else
                {
                    await client.SendTextMessageAsync(mess.Chat.Id, "Вы не ввели название бизнеса");
                }
                }



            //AdminCommands

            if (mess.Text != null)
            {
                if (role == AdminRole)
                {
                    if (mess.Text.ToLower().Contains("админ команды"))
                    {
                        await client.SendTextMessageAsync(mess.Chat.Id, "Админ команды\n/givemoney Ник Кол-во");
                    }
                }
            }

            if (mess.Text.StartsWith("/givemoney"))
            {
                string[] words = mess.Text.Split(' ');
                if (words.Length > 2)
                {
                    if (words.Length > 1)
                    {
                        string user = words[1];
                        int value;
                        if (int.TryParse(words[2], out value))
                        {
                            if (role == AdminRole)
                            {
                                cmd.Connection = conn;
                                cmd.CommandText = $"UPDATE test SET money = money + '{value}' WHERE name = '{user}'";
                                cmd.ExecuteNonQuery();
                                await client.SendTextMessageAsync(mess.Chat.Id, $"Пользователю с ником {user} дали {value}р");
                            }
                            else
                            {
                                await client.SendTextMessageAsync(mess.Chat.Id, "Недостаточно прав");
                            }
                        }
                        else
                        {
                            await client.SendTextMessageAsync(mess.Chat.Id, "Ввведите сумму, а не букву или слово");
                        }
                    }
                    else
                    {
                        await client.SendTextMessageAsync(mess.Chat.Id, "Введите например: /givemoney Котяпов 100");
                    }
                }
                else
                {
                    await client.SendTextMessageAsync(mess.Chat.Id, "Вы не указали сумму");
                }
            }
            if (mess.Text.StartsWith("/editrole"))
            {
                string[] words = mess.Text.Split(' ');
                if (words.Length > 2)
                {
                    if (words.Length > 1)
                    {
                        string user = words[1];
                        string value = words[2];
                        
                            if (role == AdminRole)
                            {
                                cmd.Connection = conn;
                                cmd.CommandText = $"UPDATE test SET role = '{value}' WHERE name = '{user}'";
                                cmd.ExecuteNonQuery();
                                await client.SendTextMessageAsync(mess.Chat.Id, $"Пользователю с ником {user} была выдана роль {value}");
                            }
                            else
                            {
                                await client.SendTextMessageAsync(mess.Chat.Id, "Недостаточно прав");
                            }
                    }
                    else
                    {
                        await client.SendTextMessageAsync(mess.Chat.Id, "Введите например: /editrole Котяпов Admin\nСписок ролей\nAdmin - Администратор\nUser - Пользователь");
                    }
                }
                else
                {
                    await client.SendTextMessageAsync(mess.Chat.Id, "Вы не указали роль");
                }
            }




            conn.Close();
        }
    }
}
