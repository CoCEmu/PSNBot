﻿using PsnLib.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PsnLib.Entities;
using System.Net;
using PSNBot.Services;
using PSNBot.Process;

namespace PSNBot
{
    // test - -120625429
    // main - -1001017895589
    // TODO пасхалки
    // TODO поправить текст правил
    // TODO поправить текст помощи
    // TODO выделять жирным то что искал пользователь
    //  TODO ХОЧУ С КЕМ ТО ПОИГРАТЬ
    // 	/await ИМЯ ИГРЫ
    // TODO команда rating
    // TODO стикеры для ачивок
    // TODO Ещё полезным будет знать статы человека, который выше тебя по рейтингу на 1

    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            var client = new PSNService();
            var task = client.Login("retran@tolkien.ru", "");
            task.Wait();

            var telegramClient = new Telegram.TelegramClient("");
            var database = new DatabaseService("../psnbot.sqlite");

            var accounts = new AccountService(database);
            var timestampService = new TimeStampService(database);

            var registrationProcess = new RegistrationProcess(telegramClient, client, accounts);

            using (var poller = new MessagePoller(database, telegramClient, client, accounts, registrationProcess, -1001017895589))
            using (var imagePoller = new ImagePoller(telegramClient, client, accounts, timestampService, -1001017895589))
            using (var trophyPoller = new TrophyPoller(telegramClient, client, accounts, timestampService, -1001017895589))
            using (var friendPoller = new FriendPoller(telegramClient, client, accounts, registrationProcess))
            {
                poller.Start();
                imagePoller.Start();
                trophyPoller.Start();
                friendPoller.Start();

                while (true)
                {
                    ;
                }
            }
        }
    }
}
