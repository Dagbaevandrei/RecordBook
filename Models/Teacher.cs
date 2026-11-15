using System.Xml.Serialization;

namespace RecordBook.Models;

[XmlType("Teacher")]
public class Teacher
{
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlElement("FullName")]
    public string FullName { get; set; } = string.Empty;

    [XmlElement("Position")]
    public string Position { get; set; } = string.Empty;

    [XmlElement("Department")]
    public string Department { get; set; } = string.Empty;

    public override string ToString() =>
        $"[{Id}] {FullName} — {Position}, {Department}";
}
