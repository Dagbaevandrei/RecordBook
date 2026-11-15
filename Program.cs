using RecordBook.Models;

namespace RecordBook;

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var book = new Models.RecordBook
        {
            Student = new StudentInfo
            {
                FullName = "Иванов Иван Иванович",
                Faculty = "Факультет информатики",
                Department = "Дневное отделение",
                Specialty = "Программная инженерия",
                RecordBookNumber = "ЗК-21-045"
            }
        };

        book.Teachers.Add(new Teacher { Id = 1, FullName = "Петров П.П.", Position = "Доцент", Department = "Кафедра программирования" });

        Console.WriteLine("Студент: " + book.Student);
        book.Teachers.ForEach(t => Console.WriteLine("  " + t));
    }
}
