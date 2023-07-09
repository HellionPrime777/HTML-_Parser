using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HTML__Parser.Models;
using HTML__Parser.Servises;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bots.Http;
using System.Net;
using System.Net.Mail;

namespace SwansonBot.Models;


public class BotCore
{
    private ProductContext context;
    private IProductWriter productWriter;

    public BotCore(ProductContext productsContext, IProductWriter writer)
    {
        context = productsContext;
        productWriter = writer;
    }

    private async Task SendProductsFileAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        var file = Guid.NewGuid().ToString("D") + ".xlsx";
        productWriter.SaveAs(file, await context.Products.ToListAsync());
        // send file
        await using (Stream stream = System.IO.File.OpenRead(file))
        {
            await bot.SendDocumentAsync(
            chatId: update.Message.Chat.Id,
            document: InputFile.FromStream(stream: stream, fileName: "products.xlsx"),
            caption: "List of products");
        }

        await Task.Run(() => System.IO.File.Delete(file), cancellationToken);

    }

    //private async Task SendProductsFileAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    //{
    //    var file = Guid.NewGuid().ToString("D") + ".xlsx";
    //    productWriter.SaveAs(file, await context.Products.ToListAsync());

    //    // Створити об'єкт листа
    //    var email = new MailMessage
    //    {
    //        From = new MailAddress("osambor1@gmail.com"), // Замініть на свою електронну адресу
    //        Subject = "Список продуктів",
    //        Body = "Знайдений файл продуктів у долученні.",
    //        IsBodyHtml = false
    //    };

    //    // Додати файл до листа
    //    email.Attachments.Add(new Attachment(file));

    //    // Встановити електронну адресу отримувача
    //    email.To.Add("osambor@ukr.net.com"); // Замініть на електронну адресу отримувача

    //    // Налаштувати клієнт SMTP для використання облікового запису Google
    //    var smtpClient = new SmtpClient("smtp.gmail.com")
    //    {
    //        Port = 587,
    //        UseDefaultCredentials = false,
    //        EnableSsl = true,
    //        Credentials = new NetworkCredential("osambor1@gmail.com", "jjnjixpolprkiktd") // Замініть на свої облікові дані Google
    //    };

    //    // Надіслати лист
    //    smtpClient.Send(email);

    //    // Видалити тимчасовий файл
    //    System.IO.File.Delete(file);
    //}


    private async Task SendProductsByEmailAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        var file = Guid.NewGuid().ToString("D") + ".xlsx";
        productWriter.SaveAs(file, await context.Products.ToListAsync());

        // Створити об'єкт листа
        var email = new MailMessage
        {
            From = new MailAddress("osambor1@gmail.com"), // Замініть на свою електронну адресу
            Subject = "Список продуктів",
            Body = "Знайдений файл продуктів у долученні.",
            IsBodyHtml = false
        };

        // Додати файл до листа
        email.Attachments.Add(new Attachment(file));

        // Встановити електронну адресу отримувача
        email.To.Add("osambor@ukr.net"); // Замініть на електронну адресу отримувача

        // Налаштувати клієнт SMTP для використання облікового запису Google
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            UseDefaultCredentials = false,
            EnableSsl = true,
            Credentials = new NetworkCredential("osambor1@gmail.com", "jjnjixpolprkiktd") // Замініть на свої облікові дані Google
        };

        // Надіслати лист
        smtpClient.Send(email);

        // Видалити тимчасовий файл
        System.IO.File.Delete(file);

        await bot.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Файл продуктів був надісланий на вашу електронну адресу."
        );
    }


    private async Task FindProductAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        var code = update.Message.Text.Trim();
        var product = context.Products.FirstOrDefault(x => x.Code == code);
        if (product != null)
        {
            await bot.SendPhotoAsync(
                    chatId: update.Message.Chat.Id,
                    photo: InputFile.FromUri(product.ImageUrl),
                    caption:
                    $"{(product.Available ? "🟢" : "🔴")} <b>{product.Title}</b>\n" +
                    $"{product.Description}\n" +
                    $"${product.Price.ToString("F")}\n",
                    parseMode: ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(
                            InlineKeyboardButton.WithUrl(
                                text: "Open on site",
                                url: product.FullUrl))
                    );

        }
        else
        {
            //        ReplyKeyboardMarkup replyKeyboardMarkup = new(
            //new[]
            //{
            //    new KeyboardButton[] { "Download file" },
            //    new KeyboardButton[] { "Отримати на пошту" } // Додайте кнопку "Отримати на пошту"
            //})
            //        {
            //            ResizeKeyboard = true
            //        };

            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[] { new KeyboardButton[] { "Download file" }, })
            {
                ResizeKeyboard = true
            };

            await bot.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Not found",
                replyMarkup: replyKeyboardMarkup
                );
        }
    }

    private async Task StartCommand(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[] { new KeyboardButton[] { "Download file" }, })
        {
            ResizeKeyboard = true
        };

        await bot.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Hello! This is Swanson parser bot.",
                replyMarkup: replyKeyboardMarkup
                );
    }

    public async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(update));

        if (update.Message != null)
        {
            await bot.SendChatActionAsync(
                       chatId: update.Message.Chat.Id,
                       chatAction: ChatAction.Typing
                       );

            var text = update.Message.Text;

            if (text == "Download file")
            {
                await SendProductsFileAsync(bot, update, cancellationToken);
            }
            else if (text == "/start")
            {
                await StartCommand(bot, update, cancellationToken);
            }
            else if (text == "SendEmailProduct")
            {
                await SendProductsByEmailAsync(bot, update, cancellationToken);
            }
            else
            {
                await FindProductAsync(bot, update, cancellationToken);
            }
            return;
        }
    }
}
