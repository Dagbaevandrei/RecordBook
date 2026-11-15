using System.Xml.Serialization;

namespace RecordBook.Models;

[XmlType("DiplomaWork")]
public class DiplomaWork
{
    [XmlElement("Topic")]
    public string Topic { get; set; } = string.Empty;

    [XmlElement("SupervisorId")]
    public int SupervisorId { get; set; }

    [XmlElement("Score")]
    public int Score { get; set; }

    [XmlElement("Date")]
    public DateTime Date { get; set; }

    public override string ToString() =>
        $"Дипломная работа: «{Topic}» — оценка: {Score}, {Date:dd.MM.yyyy}, науч. рук. #{SupervisorId}";
}
