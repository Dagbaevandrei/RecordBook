using RecordBook.Models;
using RecordBook.Services;

namespace RecordBook;

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 1. Загрузка из файла
        var book = XmlService.LoadFromJson("input.json");
        var service = new RecordBookService(book);

        Console.WriteLine("=== Исходные данные ===");
        Console.WriteLine("Студент: " + book.Student);
        Console.WriteLine("Преподаватели:");
        book.Teachers.ForEach(t => Console.WriteLine("  " + t));
        Console.WriteLine("Оценки:");
        book.Grades.ForEach(g => Console.WriteLine("  " + g));

        // 2. Добавление
        var newTeacher = service.AddTeacher("Кузнецов Олег Владимирович", "Ст. преподаватель", "Кафедра ИТ");
        service.AddGrade("Базы данных", AssessmentType.Exam, 5, DateTime.Parse("2024-06-20"), newTeacher.Id);
        service.AddCourseWork("Базы данных", "Проектирование БД", 5, DateTime.Parse("2024-05-25"), newTeacher.Id);
        service.SetDiplomaWork("Разработка веб-приложения", newTeacher.Id, 5, DateTime.Parse("2025-06-01"));

        // 3. Изменение
        service.UpdateGrade(id: 1, score: 4);
        service.UpdateTeacher(newTeacher.Id, position: "Доцент");
        service.UpdateDiplomaWork(topic: "Разработка корпоративного веб-приложения");

        // 4. Удаление
        service.RemoveGrade(id: 2);
        service.RemoveCourseWork(id: 1);

        // 5. Сортировка
        Console.WriteLine("\n=== Оценки, сортировка по дате (по убыванию) ===");
        foreach (var g in service.SortGradesBy(g => g.Date, descending: true))
            Console.WriteLine("  " + g);

        // 6. Фильтрация
        Console.WriteLine("\n=== Только экзамены с оценкой 5 ===");
        foreach (var g in service.FilterGrades(g => g.Type == AssessmentType.Exam && g.Score == 5))
            Console.WriteLine("  " + g);

        // 7. Вывод в XML
        XmlService.SaveToXml(book, "output.xml");
        Console.WriteLine("\nРезультат сохранён в output.xml");
    }
}
