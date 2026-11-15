using System.Xml.Serialization;

namespace RecordBook.Models;

[XmlRoot("RecordBook")]
public class RecordBook
{
    [XmlElement("Student")]
    public StudentInfo Student { get; set; } = new();

    [XmlArray("Teachers")]
    [XmlArrayItem("Teacher")]
    public List<Teacher> Teachers { get; set; } = new();

    [XmlArray("Grades")]
    [XmlArrayItem("Grade")]
    public List<Grade> Grades { get; set; } = new();

    [XmlArray("CourseWorks")]
    [XmlArrayItem("CourseWork")]
    public List<CourseWork> CourseWorks { get; set; } = new();

    [XmlElement("DiplomaWork")]
    public DiplomaWork? DiplomaWork { get; set; }
}
