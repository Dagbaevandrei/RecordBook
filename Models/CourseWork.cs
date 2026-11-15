using System.Xml.Serialization;

namespace RecordBook.Models;

[XmlType("CourseWork")]
public class CourseWork
{
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlElement("Subject")]
    public string Subject { get; set; } = string.Empty;

    [XmlElement("Topic")]
    public string Topic { get; set; } = string.Empty;

    [XmlElement("Score")]
    public int Score { get; set; }

    [XmlElement("Date")]
    public DateTime Date { get; set; }

    [XmlElement("TeacherId")]
    public int TeacherId { get; set; }

    public override string ToString() =>
        $"[{Id}] {Subject}: «{Topic}» — оценка: {Score}, {Date:dd.MM.yyyy}, преподаватель #{TeacherId}";
}
