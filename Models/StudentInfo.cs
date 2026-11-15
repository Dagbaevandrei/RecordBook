using System.Xml.Serialization;

namespace RecordBook.Models;

[XmlType("StudentInfo")]
public class StudentInfo
{
    [XmlElement("FullName")]
    public string FullName { get; set; } = string.Empty;

    [XmlElement("Faculty")]
    public string Faculty { get; set; } = string.Empty;

    [XmlElement("Department")]
    public string Department { get; set; } = string.Empty;

    [XmlElement("Specialty")]
    public string Specialty { get; set; } = string.Empty;

    [XmlElement("RecordBookNumber")]
    public string RecordBookNumber { get; set; } = string.Empty;

    public override string ToString() =>
        $"{FullName}, фак-т: {Faculty}, отд.: {Department}, спец.: {Specialty}, № ЗК: {RecordBookNumber}";
}
