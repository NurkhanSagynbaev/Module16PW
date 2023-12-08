using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Module16PW
{

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Добро пожаловать в приложение для логирования изменений в файлах!");

            // Получаем от пользователя путь к директории и лог-файлу
            Console.Write("Введите путь к отслеживаемой директории: ");
            string directoryPath = Console.ReadLine();

            Console.Write("Введите путь к лог-файлу: ");
            string logFilePath = Console.ReadLine();

            // Создаем FileSystemWatcher
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                // Настройки FileSystemWatcher
                watcher.Path = directoryPath;
                watcher.IncludeSubdirectories = true; // Рекурсивное отслеживание
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

                // Подписываемся на события изменений
                watcher.Created += OnCreated;
                watcher.Deleted += OnDeleted;
                watcher.Renamed += OnRenamed;

                // Запускаем отслеживание
                watcher.EnableRaisingEvents = true;

                Console.WriteLine($"Отслеживание запущено для директории: {directoryPath}");

                while (true)
                {
                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1. Фильтрация изменений");
                    Console.WriteLine("2. Рекурсивное отслеживание");
                    Console.WriteLine("3. Завершить работу");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            FilterChanges(watcher);
                            break;
                        case "2":
                            ToggleRecursive(watcher);
                            break;
                        case "3":
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Повторите попытку.");
                            break;
                    }
                }
            }
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            LogToFile($"Создан: {e.FullPath}");
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            LogToFile($"Удален: {e.FullPath}");
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            LogToFile($"Переименован: {e.OldFullPath} в {e.FullPath}");
        }

        private static void LogToFile(string logMessage)
        {
            try
            {
                string logFilePath = "log.txt"; // Путь к лог-файлу
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {logMessage}";

                // Добавляем запись в лог-файл
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);

                Console.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при логировании: {ex.Message}");
            }
        }

        private static void FilterChanges(FileSystemWatcher watcher)
        {
            Console.WriteLine("Выберите тип изменения для фильтрации:");
            Console.WriteLine("1. Создание файлов");
            Console.WriteLine("2. Удаление файлов");
            Console.WriteLine("3. Переименование файлов");
            Console.WriteLine("4. Вернуться");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    watcher.Created -= OnCreated;
                    Console.WriteLine("Фильтрация изменений: Создание файлов");
                    break;
                case "2":
                    watcher.Deleted -= OnDeleted;
                    Console.WriteLine("Фильтрация изменений: Удаление файлов");
                    break;
                case "3":
                    watcher.Renamed -= OnRenamed;
                    Console.WriteLine("Фильтрация изменений: Переименование файлов");
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Повторите попытку.");
                    break;
            }
        }

        private static void ToggleRecursive(FileSystemWatcher watcher)
        {
            watcher.IncludeSubdirectories = !watcher.IncludeSubdirectories;
            Console.WriteLine($"Рекурсивное отслеживание: {(watcher.IncludeSubdirectories ? "Включено" : "Отключено")}");
        }
    }

}
