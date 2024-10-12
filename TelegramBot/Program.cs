using System;
using System.Collections;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Npgsql;

class Program   
{
    private static ITelegramBotClient botClient;
    private static string[] subjects = {"РПИ", "АКТИОС", "АИСД" };
    private static string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
    private static string token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
    static async Task Main(string[] args)
    {
        botClient = new TelegramBotClient(token);
        
        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Бот {me.Username} запущен!");

        // Инициализация базы данных
        InitializeDatabase();

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync
        );

        Console.WriteLine("Нажмите Enter для завершения работы...");
        Console.ReadLine();
    }

    private static void InitializeDatabase()
    {
        
            //Database.Delete(databaseFile);
            //SQLiteConnection.CreateFile(databaseFile);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                foreach (var subject in subjects)
                {
                    string createTableQuery = $@"CREATE TABLE IF NOT EXISTS Queue_{subject} (ID SERIAL PRIMARY KEY AUTOINCREMENT, Name TEXT, UserId BIGINT)";
                    using (var command = new NpgsqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }   
                }
            }
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
            var userName = update.Message.From.Username == null ? string.Concat(update.Message.From.FirstName, " ", update.Message.From.LastName) : update.Message.From.Username;
            long userId = update.Message.From.Id;
            var rand = new Random();

            if (messageText.ToLower().StartsWith("/add"))
            {
                string[] splitMessage = messageText.Split('_');
                if (splitMessage.Length == 2 && int.TryParse(splitMessage[1], out int subjectNumber) && subjectNumber >= 1 && subjectNumber <= 3)
                {
                    var added = AddUserToQueue(subjectNumber, userName, userId);
                    if (added)
                    {
                        await botClient.SendTextMessageAsync(chatId, $"Вы добавлены в очередь по предмету {subjects[subjectNumber-1]}");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId, $"Вы уже находитесь в очереди по предмету {subjects[subjectNumber-1]}");
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Неверный формат. Используйте /add номер_предмета (например, /add_1)");
                }
            }
            else if (messageText.ToLower().StartsWith("/done"))
            {
                string[] splitMessage = messageText.Split('_');
                if (splitMessage.Length == 2 && int.TryParse(splitMessage[1], out int subjectNumber) && subjectNumber >= 1 && subjectNumber <= 3)
                {
                    RemoveUserFromQueue(subjectNumber, userName, userId);
                    await botClient.SendTextMessageAsync(chatId, $"Вы удалены из очереди по предмету {subjects[subjectNumber-1]}");
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Неверный формат. Используйте /done номер_предмета (например, /done_1)");
                }
            }
            else if (messageText.ToLower().StartsWith("/queue"))
            {
                string[] splitMessage = messageText.Split('_');
                if (splitMessage.Length == 2 && int.TryParse(splitMessage[1], out int subjectNumber) && subjectNumber >= 1 && subjectNumber <= 3)
                {
                    var queue = GetQueue(subjectNumber);
                    await botClient.SendTextMessageAsync(chatId, queue);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Неверный формат. Используйте /queue номер_предмета (например, /queue_1)");
                }
            }
            else if(messageText.ToLower().Equals("/help"))
            {
                await botClient.SendTextMessageAsync(chatId,
                    "Доступные команды: \n/help - показать все команды.\n/add_{номер_предмета} - записаться в очередь по тому или иному предмету. Нумерация: РПИ - 1, АКТИОС - 2, АИСД - 3. \n /queue_{номер_предмета} - узнать очередь по тому или иному предмету. \n/done_{номер_предмета} - показать, что ты сдал лабораторную и выйти из очереди.");
            }
            else if (messageText.ToLower().Equals("/start"))
            {
                await botClient.SendTextMessageAsync(chatId,
                    "Привет, я бот, написанный студентом гр. 351001 для организации автоматической очереди этой самой группы. Напиши /help, чтобы узнать больше о моих командах. Также у тебя есть меню с заготовленными командами. Желаю приятного использования моих возможностей. Если найдешь баг или возникнут ошибки, либо я не отвечаю - пиши @Daniil_Rudenya. Удачи!");
            }

        }
    }

    private static bool AddUserToQueue(int subjectNumber, string userName, long id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string tableName = $"Queue_{subjects[subjectNumber-1]}";

            // Проверка на наличие пользователя в очереди
            string checkQuery = $"SELECT COUNT(*) FROM {tableName} WHERE UserId = @ID";
            using (var checkCommand = new NpgsqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@ID", id);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    // Пользователь уже в очереди
                    return false;
                }
            }

            // Если пользователя нет в очереди, добавляем его
            string insertQuery = $"INSERT INTO {tableName} (Name, UserId) VALUES (@Name, @ID)";
            using (var command = new NpgsqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", userName);
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
            }

            return true;
        }
    }

    private static void RemoveUserFromQueue(int subjectNumber, string userName, long id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string tableName = $"Queue_{subjects[subjectNumber-1]}";

            string deleteQuery = $"DELETE FROM {tableName} WHERE UserId = @ID";
            using (var command = new NpgsqlCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
            }
        }
    }

    private static string GetQueue(int subjectNumber)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string tableName = $"Queue_{subjects[subjectNumber-1]}";

            string selectQuery = $"SELECT ID, Name FROM {tableName}";
            using (var command = new NpgsqlCommand(selectQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var queueList = $"Текущая очередь по {subjects[subjectNumber-1]}:\n";
                    int position = 1;
                    while (reader.Read())
                    {
                        string name = reader.GetString(1);
                        queueList += $"{position}. {name}\n";
                        position++;
                    }

                    return position == 1 ? "Очередь пуста" : queueList;
                }
            }
        }
    }

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка: {exception.Message}");
        return Task.CompletedTask;
    }
}