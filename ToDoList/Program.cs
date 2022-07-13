using ToDoList;
using CsvHelper;
using System.Globalization;


Console.WriteLine("Welcome to your task list." +
    "\n - Type in 'add task_name' to add a task. " +
    "\n - Type in 'start task_id' or 'complete task_id' to change task status. " +
    "\n - Type in 'show' to show current list. " +
    "\n - Type in 'import file_name' to import .csv file. " +
    "\n - Type in 'export file_name' to export .csv file. ");

var services = new TaskService();

while (true)
{
    var input = Console.ReadLine().ToLower();
    var nameSeparator = input.Split(" ");

    if (input.StartsWith("add "))
    {
        services.Add(nameSeparator); 
    }

    else if (input == "show")
    {
        services.Show();
    }

    else if (input.StartsWith("start "))
    {
        services.Start(nameSeparator);
    }

    else if (input.StartsWith("complete "))
    {
        services.Complete(nameSeparator);
    }

    else if (input.StartsWith("export "))
    {
        services.Export(nameSeparator);
    }

    else if (input.StartsWith("import "))
    {
        services.Import(nameSeparator);
    }

    else
    {
        services.Invalid();
    }
}
