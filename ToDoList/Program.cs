using ToDoList;

Console.WriteLine($"Welcome to your task list. " +
    $"{Environment.NewLine} - Type in 'add task_name' to add a task. " +
    $"{Environment.NewLine} - Type in 'start task_id' or 'complete task_id' to change task status. " +
    $"{Environment.NewLine} - Type in 'show' to show current list. " +
    $"{Environment.NewLine} - Type in 'import file_name' to import .csv file. " +
    $"{Environment.NewLine} - Type in 'export file_name' to export .csv file. ");

var taskService = new TaskService();

while (true)
{
    var input = Console.ReadLine().ToLower();
    var inputSplitted = input.Split(" ");
    var command = inputSplitted[0];

    switch (command)
    {
        case "add":
            taskService.Add(inputSplitted);
            break;

        case "show":
            taskService.Show();
            break;

        case "start":
            taskService.Start(inputSplitted);
            break;

        case "complete":
            taskService.Complete(inputSplitted);
            break;

        case "export":
            taskService.Export(inputSplitted);
            break;

        case "import":
            taskService.Import(inputSplitted);
            break;

        default:
            TaskService.Invalid();
            break;
    }
}
