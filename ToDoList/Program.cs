using ToDoList;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text;
using System.Globalization;
using System.IO;
using System.Linq;

Console.WriteLine("Welcome to your task list." +
    "\n - Type in 'add task_name' to add a task. " +
    "\n - Type in 'start task_id' or 'complete task_id' to change task status. " +
    "\n - Type in 'show' to show current list. " +
    "\n - Type in 'import path\\file_name' to import .csv file. " +
    "\n - Type in 'export path\\file_name' to export .csv file. ");

var list = new List<Item>();

while (true)
{
    var input = Console.ReadLine().ToLower();
    var item = new Item();
    int splitId;
    string[] split = input.Split(" ");
    
    // Add command
    if (input.StartsWith("add "))
    {   
        if (split.Length > 2)
        {
            Console.WriteLine("Task name cannot contain spaces.");
        }

        else
        {
            item.Name = split[1];
            item.DateAdded = DateTime.Now;
            item.Status = "New";
            item.Id = list.Count + 1;
            list.Add(item);
            Console.WriteLine("Task number {0} added successfully", item.Id);
        }
        
    }

    // Show command
    else if (input == "show")
    {   
        if (list.Count <= 0)
        {
            Console.WriteLine("List is currently empty.");
        }
        else
        {
            foreach (var i in list)
            {
                if (i.Status == "Completed")
                {
                    Console.WriteLine("{0}, Added: {1}, Status: {2}, Finished: {3}", i.Name, i.DateAdded, i.Status, i.DateFinished);

                }
                else
                {
                    Console.WriteLine("{0}, Added: {1}, Status: {2}", i.Name, i.DateAdded, i.Status);
                }
            }
        }
    }

    //Start command
    else if (input.StartsWith("start "))
    {
        var isNumeric = int.TryParse(split[1], out _);
        splitId = Convert.ToInt32(split[1]);

        if (isNumeric)
        {
            foreach (var i in list)
            {
                if (splitId == i.Id)
                {
                    if (i.Status == "In progress")
                    {
                        Console.WriteLine("Task is already in progress");
                    }
                    else
                    {
                        i.Status = "In progress";
                        Console.WriteLine("Task number {0} in progress.", i.Id);
                    }
                }
            }
        }

        else if (!isNumeric)
        {
            Console.WriteLine("Id is not valid");
        }

        else if (splitId > list.Count || splitId < list[0].Id)
        {
            Console.WriteLine("Id does not exist");
        }
    }

    //Complete command
    else if (input.StartsWith("complete "))
    {
        var isNumeric = int.TryParse(split[1], out _);
        splitId = Convert.ToInt32(split[1]);
        
        if (isNumeric)
        {
            foreach (var i in list)
            {
                if (splitId == i.Id)
                {   
                    if (i.Status == "Completed")
                    {
                        Console.WriteLine("Task has already been completed");
                    }

                    else
                    {
                        i.Status = "Completed";
                        i.DateFinished = DateTime.Now;
                        Console.WriteLine("Task number {0} completed.", i.Id);
                    }
                }
            }
        }

        else if (!isNumeric)
        {
            Console.WriteLine("Id is not valid");
        }

        else if (splitId > list.Count || splitId < list[0].Id)
        {
            Console.WriteLine("Id does not exist");
        }
    }

    //Import command
    else if (input.StartsWith("import "))
    {
        string[] pathSplit = split[1].Split(@"\");
        var fileName = pathSplit.Last();
        var pathList = pathSplit.ToList();
        pathList.Remove(fileName);
        var path = string.Join(@"\", pathList);

        if (!Directory.Exists(path))
        {
            Console.WriteLine("Directory does not exist.");
        }

        using (var reader = new StreamReader(path + @"\" + fileName + ".csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Item>();
            list = records.ToList();
        }
        Console.WriteLine("File import succeeded");

    }

    //Export command
    else if (input.StartsWith("export "))
    {   
        if (list.Count <= 0)
        {
            Console.WriteLine("List is currently empty.");
        }

        else if (split.Length > 2)
        {
            Console.WriteLine("File name cannot contain spaces.");
        }

        else
        {   
            string[] pathSplit = split[1].Split(@"\");
            var fileName = pathSplit.Last();
            var pathList = pathSplit.ToList();
            pathList.Remove(fileName);
            var path = string.Join(@"\", pathList);

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Directory does not exist.");
            }

            using (var writer = new StreamWriter(path + fileName + ".csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(list);
            }

            Console.WriteLine("File save succeeded");
        }
    }

    else
    {
        Console.WriteLine("Invalid command.");
    }
}
