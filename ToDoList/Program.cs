using ToDoList;
using CsvHelper;
using System.Globalization;


Console.WriteLine($"Welcome to your task list. {Environment.NewLine} - Type in 'add task_name' to add a task. {Environment.NewLine} - Type in 'start task_id' or 'complete task_id' to change task status. {Environment.NewLine} - Type in 'show' to show current list. {Environment.NewLine} - Type in 'import file_name' to import .csv file. {Environment.NewLine} - Type in 'export file_name' to export .csv file. ");

var taskService = new TaskService();

while (true)
{
    var input = Console.ReadLine().ToLower();
    var nameSeparator = input.Split(" ");

    switch (input)
    {
        case { } when input.StartsWith("add"):
            taskService.Add(nameSeparator);
            break;

        case { } when input.StartsWith("show"):
            taskService.Show();
            break;

        case { } when input.StartsWith("start"):
            taskService.Start(nameSeparator);
            break;

        case { } when input.StartsWith("complete"):
            taskService.Complete(nameSeparator);
            break;

        case { } when input.StartsWith("export"):
            taskService.Export(nameSeparator);
            break;

        case { } when input.StartsWith("import"):
            taskService.Import(nameSeparator);
            break;

        default:
            taskService.Invalid();
            break;
    }
}
