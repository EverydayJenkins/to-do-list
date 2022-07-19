using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class TaskService
    {
        private const string INVALID_NAME_MSG = "Name cannot be empty or contain spaces.";
        private const string INVALID_TASK_ID_MSG = "Invalid Id.";
        private const string LIST_IS_EMPTY_MSG = "List is currently empty.";

        private readonly List<ToDoTask> _tasks;
        private readonly string _path;

        public TaskService()
        {
            _tasks = new List<ToDoTask>();
            _path = @"CSV\";
        }

        private static string GetStatus(ToDoTask task)
        {
            return task.Status switch
            {
                TaskStatus.New => "New",
                TaskStatus.InProgress => "In progress",
                TaskStatus.Completed => "Completed",
                _ => "New",
            };
        }

        public void Add(string[] commandArray)
        {
            if (commandArray.Length != 2 || string.IsNullOrWhiteSpace(commandArray[1]))
            {
                Console.WriteLine(INVALID_NAME_MSG);
                return;
            }

            var task = new ToDoTask
            {
                Name = commandArray[1],
                DateAdded = DateTime.Now,
                Status = TaskStatus.New,
                Id = _tasks.Count + 1
            };

            _tasks.Add(task);
            Console.WriteLine("Task with id {0} added successfully.", task.Id);
        }

        public void Show()
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine(LIST_IS_EMPTY_MSG);
                return;
            }

            foreach (var i in _tasks)
            {
                var stringStatus = GetStatus(i);

                if (i.Status == TaskStatus.Completed)
                {
                    Console.WriteLine("Id: {0}, Name: {1} Added: {2}, Status: {3}, Finished: {4}", i.Id, i.Name, i.DateAdded, stringStatus, i.DateFinished);
                }
                else
                {
                    Console.WriteLine("Id: {0}, Name: {1}, Added: {2}, Status: {3}", i.Id, i.Name, i.DateAdded, stringStatus);
                }
            }
        }

        public void Start(string[] commandArray)
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine(LIST_IS_EMPTY_MSG);
                return;
            }

            if (commandArray.Length == 1)
            {
                Console.WriteLine(INVALID_TASK_ID_MSG);
                return;
            }

            var isNumeric = int.TryParse(commandArray[1], out var id);
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task is null)
            {
                Console.WriteLine(INVALID_TASK_ID_MSG);
            }
            else if (task.Status == TaskStatus.InProgress)
            {
                Console.WriteLine("Task is already in progress.");
            }
            else
            {
                task.Status = TaskStatus.InProgress;
                Console.WriteLine("Task with id {0} in progress.", task.Id);
            }
        }

        public void Complete(string[] commandArray)
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine(LIST_IS_EMPTY_MSG);
                return;
            }

            if (commandArray.Length == 1)
            {
                Console.WriteLine(INVALID_TASK_ID_MSG);
                return;
            }

            var isNumeric = int.TryParse(commandArray[1], out var id);
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task is null)
            {
                Console.WriteLine(INVALID_TASK_ID_MSG);
            }
            else if (task.Status == TaskStatus.Completed)
            {
                Console.WriteLine("Task has already been completed.");
            }
            else
            {
                task.Status = TaskStatus.Completed;
                task.DateFinished = DateTime.Now;
                Console.WriteLine("Task with id {0} completed.", task.Id);
            }
        }

        public void Export(string[] commandArray)
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine(LIST_IS_EMPTY_MSG);
                return;
            }

            if (commandArray.Length != 2)
            {
                Console.WriteLine(INVALID_NAME_MSG);
                return;
            }

            try
            {
                var fileName = commandArray[1];

                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }

                using var writer = new StreamWriter(_path + fileName + ".csv");
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(_tasks);
                Console.WriteLine("File saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
                return;
            }
        }

        public void Import(string[] commandArray)
        {
            if (commandArray.Length != 2)
            {
                Console.WriteLine(INVALID_NAME_MSG);
                return;
            }

            var fileName = commandArray[1];
            var fullPath = _path + fileName + ".csv";

            if (!File.Exists(fullPath))
            {
                Console.WriteLine("File named '{0}' does not exist.", fileName);
                return;
            }

            try
            {
                using var reader = new StreamReader(fullPath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<ToDoTask>();
                var importedList = records.ToList();

                foreach (var i in importedList)
                {
                    i.Id = _tasks.Count + 1;
                    _tasks.Add(i);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import failed: {ex.Message}");
                return;
            }

            Console.WriteLine("File imported.");
        }

        public static void Invalid()
        {
            Console.WriteLine("Invalid command.");
        }
    }
}
