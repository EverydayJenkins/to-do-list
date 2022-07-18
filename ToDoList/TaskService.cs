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
        private const string INVALID_TASK_NAME_MSG = "Task name cannot be empty or contain spaces.";

        private List<ToDoTask> _tasks;

        private readonly string _path;

        public TaskService()
        {
            _tasks = new List<ToDoTask>();
            _path = @"CSV\";
        }

        public void Add(string[] commandArray)
        {
            if (commandArray.Length != 2 || string.IsNullOrWhiteSpace(commandArray[1]))
            {
                Console.WriteLine(INVALID_TASK_NAME_MSG);
                return;
            }

            var task = new ToDoTask
            {
                Name = commandArray[1],
                DateAdded = DateTime.Now,
                Status = "New",
                Id = _tasks.Count + 1
            };

            _tasks.Add(task);
            Console.WriteLine("Task with id {0} added successfully.", task.Id);
        }

        public void Show()
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine("List is currently empty.");
            }
            else
            {
                foreach (var i in _tasks)
                {
                    if (i.Status == "Completed")
                    {
                        Console.WriteLine("Id: {0}, Name: {1} Added: {2}, Status: {3}, Finished: {4}", i.Id, i.Name, i.DateAdded, i.Status, i.DateFinished);
                    }
                    else
                    {
                        Console.WriteLine("Id: {0}, Name: {1}, Added: {2}, Status: {3}", i.Id, i.Name, i.DateAdded, i.Status);
                    }
                }
            }
        }

        public void Start(string[] array)
        {
            var isNumeric = int.TryParse(array[1], out var id);
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task is null)
            {
                Console.WriteLine("Invalid Id.");
                return;
            }

            if (task.Status == "In progress")
            {
                Console.WriteLine("Task is already in progress.");
                return;
            }
            else
            {
                task.Status = "In progress";
                Console.WriteLine("Task with id {0} in progress.", task.Id);
            }
        }

        public void Complete(string[] array)
        {
            var isNumeric = int.TryParse(array[1], out var id);

            if (isNumeric && id > 0 && id <= _tasks.Count)
            {
                var task = _tasks.First(t => t.Id == id);

                if (task.Status == "Completed")
                {
                    Console.WriteLine("Task has already been completed.");
                }
                else
                {
                    task.Status = "Completed";
                    task.DateFinished = DateTime.Now;
                    Console.WriteLine("Task with id {0} completed.", task.Id);
                }
            }
            else
            {
                Console.WriteLine("Invalid Id.");
            }
        }

        public void Export(string[] array)
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine("List is currently empty.");
            }

            else if (array.Length > 2)
            {
                Console.WriteLine("File name cannot contain spaces.");
            }

            else
            {
                var fileName = array[1];

                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }

                using (var writer = new StreamWriter(_path + fileName + ".csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(_tasks);
                }

                Console.WriteLine("File saved.");
            }
        }

        public void Import(string[] array)
        {
            var fileName = array[1];
            var fullPath = _path + fileName + ".csv";

            if (!File.Exists(fullPath))
            {
                Console.WriteLine("File named '{0}' does not exist.", fileName);
            }

            else
            {
                using (var reader = new StreamReader(fullPath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<ToDoTask>();
                    _tasks = records.ToList();
                }

                Console.WriteLine("File imported.");
            }
        }

        public static void Invalid()
        {
            Console.WriteLine("Invalid command.");
        }
    }
}
