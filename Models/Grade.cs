using System.Xml.Serialization;

namespace RecordBook.Models;

public enum AssessmentType
{
    Exam,
    Credit
}

[XmlType("Grade")]
public class Grade
{
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlElement("Subject")]
    public string Subject { get; set; } = string.Empty;

    [XmlElement("Type")]
    public AssessmentType Type { get; set; }

    [XmlElement("Score")]
    public int Score { get; set; }

    [XmlElement("Date")]
    public DateTime Date { get; set; }

    [XmlElement("TeacherId")]
    public int TeacherId { get; set; }

    public override string ToString() =>
        $"[{Id}] {Subject} ({Type}) — оценка: {Score}, {Date:dd.MM.yyyy}, преподаватель #{TeacherId}";
}
