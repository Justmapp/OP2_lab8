using DeanOffice.Persons;

namespace DeanOffice.Academic;
public class Group
{
    private readonly List<Student> _students = new();
    public string Id { get; }
    public string Name { get; private set; }
    public string Speciality { get; private set; }
    public int Year { get; private set; }
    public Group(string id, string name, string speciality, int year)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(speciality)) throw new ArgumentNullException(nameof(speciality));
        if (year < 1 || year > 6)
            throw new ArgumentException("Year must be between 1 and 6.", nameof(year));

        Id = id;
        Name = name;
        Speciality = speciality;
        Year = year;
    }
    public void EditData(string name, string speciality, int year)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(speciality)) throw new ArgumentNullException(nameof(speciality));
        if (year < 1 || year > 6)
            throw new ArgumentException("Year must be between 1 and 6.", nameof(year));

        Name = name;
        Speciality = speciality;
        Year = year;
    }
    public void AddStudent(Student student)
    {
        if (student == null) throw new ArgumentNullException(nameof(student));
        if (_students.Any(s => s.Id == student.Id))
            throw new ArgumentException(
                $"Student with ID '{student.Id}' is already a member of group '{Name}'.", nameof(student));

        _students.Add(student);
    }
    public void RemoveStudent(string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId)) throw new ArgumentNullException(nameof(studentId));

        var student = _students.FirstOrDefault(s => s.Id == studentId)
            ?? throw new InvalidOperationException(
                $"Student with ID '{studentId}' not found in group '{Name}'.");

        _students.Remove(student);
    }
    public IReadOnlyList<Student> GetStudents() => _students.AsReadOnly();
    public override string ToString() => $"{Name} | {Speciality} | Year {Year}";
}
