using RecordBook.Models;

namespace RecordBook.Services;

public class RecordBookService
{
    public Models.RecordBook Book { get; private set; }

    public RecordBookService(Models.RecordBook book) => Book = book;

    // ---------- Преподаватели ----------
    public Teacher AddTeacher(string fullName, string position, string department)
    {
        int id = Book.Teachers.Count == 0 ? 1 : Book.Teachers.Max(t => t.Id) + 1;
        var t = new Teacher { Id = id, FullName = fullName, Position = position, Department = department };
        Book.Teachers.Add(t);
        return t;
    }

    public bool RemoveTeacher(int id) =>
        Book.Teachers.RemoveAll(t => t.Id == id) > 0;

    public bool UpdateTeacher(int id, string? fullName = null, string? position = null, string? department = null)
    {
        var t = Book.Teachers.FirstOrDefault(x => x.Id == id);
        if (t == null) return false;
        if (fullName != null) t.FullName = fullName;
        if (position != null) t.Position = position;
        if (department != null) t.Department = department;
        return true;
    }

    // ---------- Оценки ----------
    public Grade AddGrade(string subject, AssessmentType type, int score, DateTime date, int teacherId)
    {
        if (!Book.Teachers.Any(t => t.Id == teacherId))
            throw new ArgumentException($"Преподаватель с Id={teacherId} не найден.");
        if (type == AssessmentType.Exam && (score < 2 || score > 5))
            throw new ArgumentException("Оценка за экзамен должна быть от 2 до 5.");
        if (type == AssessmentType.Credit && score != 0 && score != 1)
            throw new ArgumentException("Оценка за зачет должна быть 0 или 1.");

        int id = Book.Grades.Count == 0 ? 1 : Book.Grades.Max(g => g.Id) + 1;
        var g = new Grade { Id = id, Subject = subject, Type = type, Score = score, Date = date, TeacherId = teacherId };
        Book.Grades.Add(g);
        return g;
    }

    public bool RemoveGrade(int id) => Book.Grades.RemoveAll(g => g.Id == id) > 0;

    public bool UpdateGrade(int id, string? subject = null, int? score = null, DateTime? date = null, int? teacherId = null)
    {
        var g = Book.Grades.FirstOrDefault(x => x.Id == id);
        if (g == null) return false;
        if (subject != null) g.Subject = subject;
        if (score.HasValue) g.Score = score.Value;
        if (date.HasValue) g.Date = date.Value;
        if (teacherId.HasValue)
        {
            if (!Book.Teachers.Any(t => t.Id == teacherId.Value))
                throw new ArgumentException($"Преподаватель с Id={teacherId} не найден.");
            g.TeacherId = teacherId.Value;
        }
        return true;
    }

    // ---------- Курсовые ----------
    public CourseWork AddCourseWork(string subject, string topic, int score, DateTime date, int teacherId)
    {
        if (!Book.Teachers.Any(t => t.Id == teacherId))
            throw new ArgumentException($"Преподаватель с Id={teacherId} не найден.");

        int id = Book.CourseWorks.Count == 0 ? 1 : Book.CourseWorks.Max(c => c.Id) + 1;
        var c = new CourseWork { Id = id, Subject = subject, Topic = topic, Score = score, Date = date, TeacherId = teacherId };
        Book.CourseWorks.Add(c);
        return c;
    }

    public bool RemoveCourseWork(int id) => Book.CourseWorks.RemoveAll(c => c.Id == id) > 0;

    public bool UpdateCourseWork(int id, string? subject = null, string? topic = null,
                                 int? score = null, DateTime? date = null, int? teacherId = null)
    {
        var c = Book.CourseWorks.FirstOrDefault(x => x.Id == id);
        if (c == null) return false;
        if (subject != null) c.Subject = subject;
        if (topic != null) c.Topic = topic;
        if (score.HasValue) c.Score = score.Value;
        if (date.HasValue) c.Date = date.Value;
        if (teacherId.HasValue) c.TeacherId = teacherId.Value;
        return true;
    }

    // ---------- Диплом ----------
    public void SetDiplomaWork(string topic, int supervisorId, int score, DateTime date)
    {
        if (!Book.Teachers.Any(t => t.Id == supervisorId))
            throw new ArgumentException($"Преподаватель с Id={supervisorId} не найден.");
        Book.DiplomaWork = new DiplomaWork { Topic = topic, SupervisorId = supervisorId, Score = score, Date = date };
    }

    public bool RemoveDiplomaWork() { Book.DiplomaWork = null; return true; }

    public bool UpdateDiplomaWork(string? topic = null, int? supervisorId = null, int? score = null, DateTime? date = null)
    {
        if (Book.DiplomaWork == null) return false;
        if (topic != null) Book.DiplomaWork.Topic = topic;
        if (supervisorId.HasValue)
        {
            if (!Book.Teachers.Any(t => t.Id == supervisorId.Value))
                throw new ArgumentException($"Преподаватель с Id={supervisorId} не найден.");
            Book.DiplomaWork.SupervisorId = supervisorId.Value;
        }
        if (score.HasValue) Book.DiplomaWork.Score = score.Value;
        if (date.HasValue) Book.DiplomaWork.Date = date.Value;
        return true;
    }

    // ---------- Сортировка ----------
    public List<Grade> SortGradesBy(Func<Grade, object> key, bool descending = false)
    {
        var q = descending ? Book.Grades.OrderByDescending(key) : Book.Grades.OrderBy(key);
        return q.ToList();
    }

    public List<CourseWork> SortCourseWorksBy(Func<CourseWork, object> key, bool descending = false)
    {
        var q = descending ? Book.CourseWorks.OrderByDescending(key) : Book.CourseWorks.OrderBy(key);
        return q.ToList();
    }

    // ---------- Фильтрация ----------
    public List<Grade> FilterGrades(Func<Grade, bool> predicate) =>
        Book.Grades.Where(predicate).ToList();

    public List<CourseWork> FilterCourseWorks(Func<CourseWork, bool> predicate) =>
        Book.CourseWorks.Where(predicate).ToList();

    public List<Teacher> FilterTeachers(Func<Teacher, bool> predicate) =>
        Book.Teachers.Where(predicate).ToList();
}
