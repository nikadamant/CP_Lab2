using System.Text;

namespace CP_Lab1
{
    // ----------------------------------------------------------------------
    // 1. ПЕРЕРАХУВАННЯ СТАТУСІВ
    // ----------------------------------------------------------------------
    public enum TaskStatus
    {
        Planned,    // Заплановано
        InProgress, // В роботі
        Completed   // Виконано
    }

    // ----------------------------------------------------------------------
    // 2. КЛАС СТРУКТУРИ ДАНИХ
    // ----------------------------------------------------------------------
    public class LearningTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Course { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }

        public LearningTask(int id, string title, string course, DateTime dueDate, TaskStatus status = TaskStatus.Planned)
        {
            Id = id;
            Title = title;
            Course = course;
            DueDate = dueDate;
            Status = status;
        }

        public override string ToString()
        {
            string statusString = Status switch
            {
                TaskStatus.Completed => "[ВИКОНАНО]",
                TaskStatus.InProgress => "[В РОБОТІ]",
                TaskStatus.Planned => "[ЗАПЛАНОВАНО]",
                _ => "[НЕВИЗНАЧЕНО]"
            };

            // Формат: ID. [СТАТУС] | Назва (Курс) до: Дата
            return $"{Id}. {statusString} | {Title} ({Course}) до: {DueDate:dd.MM.yyyy}";
        }
    }

    // ----------------------------------------------------------------------
    // 3. ОСНОВНА ЛОГІКА ПРОГРАМИ
    // ----------------------------------------------------------------------
    public static class Program
    {
        private static List<LearningTask> tasks = new List<LearningTask>();
        private static int nextTaskId = 1;

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            InitializeData();

            while (true)
            {
                DisplayMenu();
                string? choice = Console.ReadLine()?.ToLower();

                switch (choice)
                {
                    case "1":
                        DisplayTasks();
                        break;
                    case "2":
                        AddTask();
                        break;
                    case "3":
                        ToggleTaskStatus();
                        break;
                    case "4":
                        DeleteTask();
                        break;
                    case "x":
                        Console.WriteLine("Дякуємо за використання EduPlan. До побачення!");
                        return;
                    default:
                        Console.WriteLine("Невідома команда. Спробуйте ще раз.");
                        Console.WriteLine("\nНатисніть Enter для продовження...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void InitializeData()
        {
            tasks.Add(new LearningTask(nextTaskId++, "Лабораторна 1 (Консоль)", "Кросплатформне програмування", DateTime.Now.AddDays(7), TaskStatus.InProgress));
            tasks.Add(new LearningTask(nextTaskId++, "Підготуватись до колоквіуму", "Алгоритми", DateTime.Now.AddDays(14), TaskStatus.Completed));
            tasks.Add(new LearningTask(nextTaskId++, "Завершити курсову роботу", "Бази даних", DateTime.Now.AddDays(30), TaskStatus.Planned));
        }

        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("=============================================");
            Console.WriteLine("          EduPlan: Менеджер завдань      ");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. Показати всі завдання (вибір сортування)");
            Console.WriteLine("2. Додати нове завдання");
            Console.WriteLine("3. Змінити статус завдання");
            Console.WriteLine("4. Видалити завдання");
            Console.WriteLine("X. Вихід");
            Console.Write("\nВведіть свій вибір: ");
        }

        private static void DisplayTasks()
        {
            while (true)
            {
                if (!tasks.Any())
                {
                    Console.WriteLine("\nСписок завдань порожній.");
                    Console.WriteLine("\nНатисніть Enter для повернення до головного меню...");
                    Console.ReadLine();
                    return;
                }

                Console.Clear();
                Console.WriteLine("\n--- ВИБІР ПОРЯДКУ СОРТУВАННЯ ---");
                Console.WriteLine("1. Сортувати за терміном (DueDate)");
                Console.WriteLine("2. Сортувати за ID (порядком додавання)");
                Console.WriteLine("X. Повернутися до головного меню");
                Console.Write("\nВведіть свій вибір: ");

                string? sortChoice = Console.ReadLine()?.ToLower();
                IEnumerable<LearningTask> tasksToDisplay;
                string sortDescription = "";

                switch (sortChoice)
                {
                    case "1":
                        tasksToDisplay = tasks.OrderBy(t => t.DueDate);
                        sortDescription = "за терміном";
                        break;
                    case "2":
                        tasksToDisplay = tasks.OrderBy(t => t.Id);
                        sortDescription = "за ID";
                        break;
                    case "x":
                        return;
                    default:
                        Console.WriteLine("Невірна опція. Натисніть Enter для повторного вибору...");
                        Console.ReadLine();
                        continue;
                }

                Console.WriteLine($"\n--- СПИСОК ЗАВДАНЬ ({sortDescription}) ---");
                foreach (var task in tasksToDisplay)
                {
                    Console.WriteLine(task.ToString());
                }

                Console.WriteLine("\nНатисніть Enter, щоб повернутися до головного меню...");
                Console.ReadLine();
                return;
            }
        }

        private static void DisplayTasksForAction()
        {
            Console.WriteLine("\n--- АКТУАЛЬНИЙ СПИСОК ЗАВДАНЬ (за ID) ---");
            var sortedTasks = tasks.OrderBy(t => t.Id).ToList();
            foreach (var task in sortedTasks)
            {
                Console.WriteLine(task.ToString());
            }
        }

        private static void AddTask()
        {
            while (true)
            {
                Console.WriteLine("\n--- ДОДАВАННЯ НОВОГО ЗАВДАННЯ ---");

                Console.Write("Введіть назву завдання (або 'x' для скасування): ");
                string? title = Console.ReadLine()?.Trim();
                if (title?.ToLower() == "x") return;

                Console.Write("Введіть назву курсу: ");
                string? course = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(course))
                {
                    Console.WriteLine("\n Помилка: Назва та курс не можуть бути порожніми. Спробуйте ще раз.");
                    Console.WriteLine("Натисніть Enter для повторного введення...");
                    Console.ReadLine();
                    continue;
                }

                DateTime dueDate;
                Console.Write("Введіть кінцевий термін (формат РРРР-ММ-ДД): ");
                if (!DateTime.TryParse(Console.ReadLine(), System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dueDate))
                {
                    Console.WriteLine("\n Помилка: Некоректний формат дати. Спробуйте ще раз.");
                    Console.WriteLine("Натисніть Enter для повторного введення...");
                    Console.ReadLine();
                    continue;
                }

                var newTask = new LearningTask(nextTaskId++, title, course, dueDate, TaskStatus.Planned);
                tasks.Add(newTask);
                Console.WriteLine($"\n Завдання '{title}' успішно додано зі статусом [ЗАПЛАНОВАНО].");

                Console.WriteLine("\nНатисніть Enter для повернення до головного меню...");
                Console.ReadLine();
                return;
            }
        }

        private static void ToggleTaskStatus()
        {
            while (true)
            {
                if (!tasks.Any())
                {
                    Console.WriteLine("\nСписок завдань порожній. Немає чого змінювати.");
                    Console.WriteLine("\nНатисніть Enter для повернення до головного меню...");
                    Console.ReadLine();
                    return;
                }

                DisplayTasksForAction();

                Console.Write("\nВведіть ID завдання, статус якого бажаєте змінити (або 'x' для скасування): ");
                string? input = Console.ReadLine()?.ToLower();
                if (input == "x") return;

                if (int.TryParse(input, out int taskId))
                {
                    var task = tasks.FirstOrDefault(t => t.Id == taskId);
                    if (task != null)
                    {
                        // Логіка циклічного перемикання
                        task.Status = task.Status switch
                        {
                            TaskStatus.Planned => TaskStatus.InProgress,
                            TaskStatus.InProgress => TaskStatus.Completed,
                            TaskStatus.Completed => TaskStatus.Planned,
                            _ => TaskStatus.Planned
                        };

                        Console.WriteLine($"\n Статус завдання '{task.Title}' змінено на: [{task.Status.ToString().ToUpper()}].");

                        Console.WriteLine("\nНатисніть Enter для повернення до головного меню...");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("\n Помилка: Завдання з таким ID не знайдено.");
                        Console.WriteLine("Натисніть Enter для повторного введення ID...");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("\n Помилка: Некоректний формат ID.");
                    Console.WriteLine("Натисніть Enter для повторного введення ID...");
                    Console.ReadLine();
                }
            }
        }

        private static void DeleteTask()
        {
            while (true)
            {
                if (!tasks.Any())
                {
                    Console.WriteLine("\nСписок завдань порожній. Немає чого видаляти.");
                    Console.WriteLine("\nНатисніть Enter для повернення до головного меню...");
                    Console.ReadLine();
                    return;
                }

                DisplayTasksForAction();

                Console.Write("\nВведіть ID завдання, яке бажаєте видалити (або 'x' для скасування): ");
                string? input = Console.ReadLine()?.ToLower();
                if (input == "x") return;

                if (int.TryParse(input, out int taskId))
                {
                    var task = tasks.FirstOrDefault(t => t.Id == taskId);
                    if (task != null)
                    {
                        tasks.Remove(task);
                        Console.WriteLine($"\n Завдання '{task.Title}' видалено.");

                        Console.WriteLine("\nНатисніть Enter для повернення до головного меню...");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("\n Помилка: Завдання з таким ID не знайдено.");
                        Console.WriteLine("Натисніть Enter для повторного введення ID...");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("\n Помилка: Некоректний формат ID.");
                    Console.WriteLine("Натисніть Enter для повторного введення ID...");
                    Console.ReadLine();
                }
            }
        }
    }
}