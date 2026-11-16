using RecordBook.Models;
using RecordBook.Services;

namespace RecordBook;

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Загружаем данные из файла
        RecordBook.Models.RecordBook book;
        try
        {
            book = XmlService.LoadFromJson("input.json");
        }
        catch
        {
            book = new RecordBook.Models.RecordBook();
            Console.WriteLine("Файл input.json не найден. Создана новая пустая зачетка.");
        }

        var service = new RecordBookService(book);

        Console.WriteLine("Электронная зачетная книжка студента");
        Console.WriteLine($"Владелец: {book.Student.FullName}");
        Console.WriteLine("Доступные команды: add, update, delete, list, sort, filter, save, exit");

        while (true)
        {
            Console.Write("\n> ");
            var input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(input)) continue;

            switch (input)
            {
                case "add":
                    Console.WriteLine("Что добавить? (teacher/grade/coursework/diploma)");
                    var addType = Console.ReadLine()?.Trim().ToLower();
                    HandleAdd(service, addType);
                    break;

                case "update":
                    Console.WriteLine("Что изменить? (teacher/grade/coursework/diploma)");
                    var updateType = Console.ReadLine()?.Trim().ToLower();
                    HandleUpdate(service, updateType);
                    break;

                case "delete":
                    Console.WriteLine("Что удалить? (teacher/grade/coursework/diploma)");
                    var deleteType = Console.ReadLine()?.Trim().ToLower();
                    HandleDelete(service, deleteType);
                    break;

                case "list":
                    Console.WriteLine("\n=== СТУДЕНТ ===");
                    Console.WriteLine(book.Student);
                    Console.WriteLine("\n=== ПРЕПОДАВАТЕЛИ ===");
                    book.Teachers.ForEach(t => Console.WriteLine(t));
                    Console.WriteLine("\n=== ОЦЕНКИ ===");
                    book.Grades.ForEach(g => Console.WriteLine(g));
                    Console.WriteLine("\n=== КУРСОВЫЕ ===");
                    book.CourseWorks.ForEach(c => Console.WriteLine(c));
                    Console.WriteLine("\n=== ДИПЛОМ ===");
                    Console.WriteLine(book.DiplomaWork?.ToString() ?? "Нет дипломной работы");
                    break;

                case "sort":
                    Console.WriteLine("Сортировать оценки по: (date/score/subject)");
                    var sortBy = Console.ReadLine()?.Trim().ToLower();
                    var sorted = sortBy switch
                    {
                        "score" => service.SortGradesBy(g => g.Score),
                        "subject" => service.SortGradesBy(g => g.Subject),
                        _ => service.SortGradesBy(g => g.Date, descending: true)
                    };
                    sorted.ForEach(g => Console.WriteLine("  " + g));
                    break;

                case "filter":
                    Console.WriteLine("Фильтр: (exams/credits/fives)");
                    var filterType = Console.ReadLine()?.Trim().ToLower();
                    var filtered = filterType switch
                    {
                        "exams" => service.FilterGrades(g => g.Type == AssessmentType.Exam),
                        "credits" => service.FilterGrades(g => g.Type == AssessmentType.Credit),
                        "fives" => service.FilterGrades(g => g.Score == 5),
                        _ => service.FilterGrades(g => true)
                    };
                    filtered.ForEach(g => Console.WriteLine("  " + g));
                    break;

                case "save":
                    XmlService.SaveToXml(book, "output.xml");
                    Console.WriteLine("Данные успешно сохранены в output.xml");
                    break;

                case "exit":
                    Console.WriteLine("Выход из программы...");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда. Попробуйте: add, update, delete, list, sort, filter, save, exit");
                    break;
            }
        }
    }
    
    static void HandleAdd(RecordBookService service, string? type)
    {
        try
        {
            if (type == "teacher")
            {
                Console.Write("ФИО преподавателя: ");
                string name = Console.ReadLine() ?? "";
                Console.Write("Должность: ");
                string pos = Console.ReadLine() ?? "";
                Console.Write("Кафедра: ");
                string dep = Console.ReadLine() ?? "";
                var t = service.AddTeacher(name, pos, dep);
                Console.WriteLine($"Добавлен преподаватель: {t}");
            }
            else if (type == "grade")
            {
                Console.Write("Предмет: ");
                string subj = Console.ReadLine() ?? "";
                Console.Write("Тип (Exam/Credit): ");
                var gType = Console.ReadLine()?.Trim().ToLower() == "credit" ? AssessmentType.Credit : AssessmentType.Exam;
                Console.Write("Оценка: ");
                int score = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("ID преподавателя: ");
                int tId = int.Parse(Console.ReadLine() ?? "0");
                var g = service.AddGrade(subj, gType, score, DateTime.Now, tId);
                Console.WriteLine($"Добавлена оценка: {g}");
            }
            else if (type == "coursework")
            {
                Console.Write("Предмет: ");
                string subj = Console.ReadLine() ?? "";
                Console.Write("Тема: ");
                string topic = Console.ReadLine() ?? "";
                Console.Write("Оценка: ");
                int score = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("ID преподавателя: ");
                int tId = int.Parse(Console.ReadLine() ?? "0");
                var c = service.AddCourseWork(subj, topic, score, DateTime.Now, tId);
                Console.WriteLine($"Добавлена курсовая: {c}");
            }
            else if (type == "diploma")
            {
                Console.Write("Тема: ");
                string topic = Console.ReadLine() ?? "";
                Console.Write("ID научного руководителя: ");
                int tId = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Оценка: ");
                int score = int.Parse(Console.ReadLine() ?? "0");
                service.SetDiplomaWork(topic, tId, score, DateTime.Now);
                Console.WriteLine($"Дипломная работа добавлена.");
            }
            else Console.WriteLine("Неизвестный тип.");
        }
        catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
    }

    static void HandleUpdate(RecordBookService service, string? type)
    {
        try
        {
            Console.Write("Введите ID для изменения: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            if (type == "grade")
            {
                Console.Write("Новая оценка (оставьте пустым, чтобы не менять): ");
                string scoreStr = Console.ReadLine() ?? "";
                int? score = string.IsNullOrWhiteSpace(scoreStr) ? null : int.Parse(scoreStr);
                
                if (service.UpdateGrade(id, score: score))
                    Console.WriteLine("Оценка обновлена.");
                else
                    Console.WriteLine("Оценка не найдена.");
            }
            else if (type == "teacher")
            {
                Console.Write("Новая должность: ");
                string pos = Console.ReadLine() ?? "";
                if (service.UpdateTeacher(id, position: pos))
                    Console.WriteLine("Преподаватель обновлен.");
                else
                    Console.WriteLine("Преподаватель не найден.");
            }
            else Console.WriteLine("Обновление этого типа пока не реализовано в меню.");
        }
        catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
    }

    static void HandleDelete(RecordBookService service, string? type)
    {
        try
        {
            Console.Write("Введите ID для удаления: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            bool success = type switch
            {
                "grade" => service.RemoveGrade(id),
                "teacher" => service.RemoveTeacher(id),
                "coursework" => service.RemoveCourseWork(id),
                "diploma" => service.RemoveDiplomaWork(),
                _ => false
            };

            if (success) Console.WriteLine("Успешно удалено.");
            else Console.WriteLine("Не найдено или неизвестный тип.");
        }
        catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
    }
}