namespace DeanOffice.Persons;
public class Student : Person
{
    public string StudentCode { get; }
    public string? GroupId { get; private set; }
    public Student(string id, string name, string surname, string studentCode)
        : base(id, name, surname)
    {
        if (string.IsNullOrWhiteSpace(studentCode)) throw new ArgumentNullException(nameof(studentCode));
        StudentCode = studentCode;
    }
    public void AssignToGroup(string groupId)
    {
        if (string.IsNullOrWhiteSpace(groupId)) throw new ArgumentNullException(nameof(groupId));
        GroupId = groupId;
    }
    public void RemoveFromGroup() => GroupId = null;
    public override string ToString() => $"{GetFullName()} [{StudentCode}]";
}
