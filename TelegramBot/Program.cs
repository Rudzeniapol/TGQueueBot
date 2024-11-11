using System;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

class Program
{
    private static ITelegramBotClient botClient;
    private static string[] subjects = { "РПИ", "АКТИОС" };
    private static string databaseFile = "queue.db";
    private static string token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");

    //private static string[] names = {"lutatel_risa","ivan_hdjddjs","Александр Шевченко","gaegxh","xxxSkibidiSigmaxxx","Satori12","kostyabelbet","Daniil_Rudenya","TPNE05","temaqt","faysonin","renuxela","ilya2005pan4","ZimaBlue9","leravvvvvvvv","fluffy11lol","who_o","werty2648","v4ssal","alekRadt","verretta","SergeyOrsik","DJ_4_7","push_offset_name","egor_geekin","koan6gi","S1024","valery_kuzh"};
    //private static long[] IDs = {2078365425, 563239661, 5540649849, 988522114, 5167248295, 1134073796, 1256601598, 1251070423, 835559951, 885887534, 827919637, 1041411876, 1009510323, 1955527569, 1192276306, 640411109, 1006764486, 1396730464, 865601041, 824298179, 1296088366, 1232957794, 525033481, 845340305, 894871193, 1464860402, 884211123, 947840361};
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
        
        await Task.Delay(-1);
    }

    private static void InitializeDatabase()
    {
        if (!File.Exists(databaseFile))
        {
            SQLiteConnection.CreateFile(databaseFile);
        }

        using (var connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;"))
        {
            connection.Open();

            foreach (var subject in subjects)
            {
                string createTableQuery = $@"CREATE TABLE IF NOT EXISTS Queue_{subject} (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    UserId INTEGER UNIQUE)";
                
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                
            }
            /*for (int i = 1; i <= 28; i++)
            {
                string transferDataBase = $@"INSERT INTO Queue_РПИ (Name, UserId) VALUES (@Name, @UserId)";
                using (var command = new SQLiteCommand(transferDataBase, connection))
                {
                    command.Parameters.AddWithValue("@Name", names[i-1]);
                    command.Parameters.AddWithValue("@UserId", IDs[i-1]);
                    command.ExecuteNonQuery();
                }
            }}*/
        }
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text.Trim();
            var userName = update.Message.From.Username ?? $"{update.Message.From.FirstName} {update.Message.From.LastName}";
            long userId = update.Message.From.Id;

            if (messageText.StartsWith("/add"))
            {
                string[] splitMessage = messageText.Split('_');
                if (splitMessage.Length == 2 && int.TryParse(splitMessage[1], out int subjectNumber) && subjectNumber >= 1 && subjectNumber <= subjects.Length)
                {
                    var added = AddUserToQueue(subjectNumber, userName, userId);
                    await botClient.SendTextMessageAsync(chatId, added
                        ? $"Вы добавлены в очередь по предмету {subjects[subjectNumber - 1]}"
                        : $"Вы уже находитесь в очереди по предмету {subjects[subjectNumber - 1]}");
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Неверный формат. Используйте /add номер_предмета (например, /add_1)");
                }
            }
            else if (messageText.StartsWith("/done"))
            {
                string[] splitMessage = messageText.Split('_');
                if (splitMessage.Length == 2 && int.TryParse(splitMessage[1], out int subjectNumber) && subjectNumber >= 1 && subjectNumber <= subjects.Length)
                {
                    RemoveUserFromQueue(subjectNumber, userName, userId);
                    await botClient.SendTextMessageAsync(chatId, $"Вы удалены из очереди по предмету {subjects[subjectNumber - 1]}");
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Неверный формат. Используйте /done номер_предмета (например, /done_1)");
                }
            }
            else if (messageText.StartsWith("/queue"))
            {
                string[] splitMessage = messageText.Split('_');
                if (splitMessage.Length == 2 && int.TryParse(splitMessage[1], out int subjectNumber) && subjectNumber >= 1 && subjectNumber <= subjects.Length)
                {
                    var queue = GetQueue(subjectNumber);
                    await botClient.SendTextMessageAsync(chatId, queue);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Неверный формат. Используйте /queue номер_предмета (например, /queue_1)");
                }
            }
            else if (messageText.Equals("/help"))
            {
                await botClient.SendTextMessageAsync(chatId,
                    "Доступные команды: \n/help - показать все команды.\n/add_{номер_предмета} - записаться в очередь по тому или иному предмету. Нумерация: РПИ - 1, АКТИОС - 2. \n/queue_{номер_предмета} - узнать очередь по тому или иному предмету. \n/done_{номер_предмета} - показать, что ты сдал лабораторную и выйти из очереди.");
            }
            else if (messageText.Equals("/start"))
            {
                await botClient.SendTextMessageAsync(chatId, "Привет, я бот, написанный студентом для организации автоматической очереди. Напиши /help, чтобы узнать больше о моих командах.");
            }
        }
    }

    private static bool AddUserToQueue(int subjectNumber, string userName, long userId)
    {
        using (var connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;"))
        {
            connection.Open();
            string tableName = $"Queue_{subjects[subjectNumber - 1]}";

            string checkQuery = $"SELECT COUNT(*) FROM {tableName} WHERE UserId = @UserId";
            using (var checkCommand = new SQLiteCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@UserId", userId);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    return false;
                }
            }

            string insertQuery = $"INSERT INTO {tableName} (Name, UserId) VALUES (@Name, @UserId)";
            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", userName);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }

            return true;
        }
    }

    private static void RemoveUserFromQueue(int subjectNumber, string userName, long userId)
    {
        using (var connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;"))
        {
            connection.Open();
            string tableName = $"Queue_{subjects[subjectNumber - 1]}";

            string deleteQuery = $"DELETE FROM {tableName} WHERE UserId = @UserId";
            using (var command = new SQLiteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }
    }

    private static string GetQueue(int subjectNumber)
    {
        using (var connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;"))
        {
            connection.Open();
            string tableName = $"Queue_{subjects[subjectNumber - 1]}";

            string selectQuery = $"SELECT Name FROM {tableName} ORDER BY ID";
            using (var command = new SQLiteCommand(selectQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var queueList = $"Текущая очередь по {subjects[subjectNumber - 1]}:\n";
                    int position = 1;
                    while (reader.Read())
                    {
                        queueList += $"{position}. {reader.GetString(0)}\n";
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
