
//t.me / ProjectNPBot
//5874968303:AAE3qm0Jt7rI2a9gPfM12kHJoiASKCjgbxM

//var bot  = new Telg


using Newtonsoft.Json;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using HTML__Parser.Models;
using HTML__Parser.Servises;
using OfficeOpenXml;
using System.Threading;
using SwansonBot.Models;

var token = "5874968303:AAE3qm0Jt7rI2a9gPfM12kHJoiASKCjgbxM";

var bot = new TelegramBotClient(token);

Console.WriteLine("Start " + bot.GetMeAsync().Result.FirstName);

var cts = new CancellationTokenSource();
var core = new BotCore(
    new HTML__Parser.Models.ProductContext(),
    new ExelWriter() 
    );


bot.StartReceiving(
    // handle update
    core.HandleUpdate,
    // handle error
    async (bot, exeption, cancellationToken) =>
    {

        await Console.Out.WriteLineAsync(exeption.Message);
    },
    // option 
    new Telegram.Bot.Polling.ReceiverOptions { AllowedUpdates = { } },
    // cancelatiin token
    cts.Token

    );

Console.ReadLine();


//var bot = new TelegramBotClient(token);

//Console.WriteLine("Start " + bot.GetMeAsync().Result.FirstName);

//var cts = new CancellationTokenSource();

//bot.StartReceiving(
//    // handle update
//    async (update, cancellationToken) =>
//    {
//        await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(update));

//        if (update.Message != null)
//        {
//            string productCode = update.Message.Text.Trim();
//            using (var db = new ProductContext())
//            {
//                var product = db.Products.FirstOrDefault(p => p.ProductName == productCode);

//                if (product != null)
//                {
//                    string message = $"<b>{product.Title}</b>\n" +
//                                     $"Price: {product.Price}\n" +
//                                     $"Vendor: {product.Value}\n" +
//                                     $"Description: {product.Discription}\n" +
//                                     $"ImageUrl: {product.ImageUrl}\n";

//                    var replyMarkup = new InlineKeyboardMarkup(new[]
//                    {
//                        new[]
//                        {
//                            InlineKeyboardButton.WithCallbackData("Завантажити файл")
//                        }
//                    });

//                    await bot.SendTextMessageAsync(
//                        chatId: update.Message.Chat.Id,
//                        text: message,
//                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
//                        replyMarkup: replyMarkup
//                    );
//                }
//                else
//                {
//                    await bot.SendTextMessageAsync(
//                        chatId: update.Message.Chat.Id,
//                        text: "Product not found."
//                    );
//                }
//            }
//        }
//    },
//    // handle error
//    async (exception, cancellationToken) =>
//    {
//        await Console.Out.WriteLineAsync(exception.Message);
//    },
//    bot.OnCallbackQuery += async (sender, args) =>
//    {
//        if (args.CallbackQuery.Data == "Завантажити файл")
//        {
//            await using (var fileStream = new FileStream("products.xlsx", FileMode.Open))
//            {
//                await bot.SendDocumentAsync(
//                    chatId: args.CallbackQuery.Message.Chat.Id,
//                    document: new InputOnlineFile(fileStream, "products.xlsx"),
//                    caption: "Ваш файл XLS"
//                );
//            }
//        }
//    }

//// options
//new Telegram.Bot.Polling.ReceiverOptions { AllowedUpdates = { Telegram.Bot.Types.Enums.UpdateType.CallbackQuery } },

//    // cancellation token
//    cts.Token
//);


//Console.ReadLine();






//bot.StartReceiving(

//    // handle update

//    async (bot, update, cancellationToken) =>

//    {

//        await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(update));



//        if (update.Message != null)

//        {
//            //await bot.SendTextMessageAsync(update.Message.Chat, "Hello");

//            await bot.SendPhotoAsync(

//                chatId: update.Message.Chat.Id,

//                photo: InputFile.FromUri("https://media.swansonvitamins.com/images/items/master/SW257.jpg"),

//                caption:

//                "<b>Swanson Premium- Daily Essentials Multi with Iron</b>\n" +

//                "$15.99\n" +

//                "<a href=\"https://www.swansonvitamins.com/p/swanson-premium-daily-multi-vitamin-mineral-250-caps\">Open</a>\n ",

//                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
//                replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(text: "Open",
//                url: "https://www.swansonvitamins.com/p/swanson-premium-triple-magnesium-complex-400-mg-300-caps")));

//        }
//    },

// handle error

