using DeanOffice.Academic;
using DeanOffice.Housing;
using DeanOffice.Persons;

namespace DeanOffice;
public class DeanRegistry : IDeanSystem
{
    private readonly List<Student> _students = new();
    private readonly List<Group> _groups = new();
    private readonly List<Dormitory> _dormitories = new();
    public void AddStudent(Student student)
    {
        if (student == null) throw new ArgumentNullException(nameof(student));
        if (_students.Any(s => s.Id == student.Id))
            throw new ArgumentException(
                $"Student with ID '{student.Id}' is already registered.", nameof(student));

        _students.Add(student);
    }
    public void RemoveStudent(string studentId)
    {
        var student = FindStudent(studentId);

        foreach (var group in _groups)
        {
            try { group.RemoveStudent(studentId); }
            catch (InvalidOperationException) { }
        }

        foreach (var dormitory in _dormitories)
        foreach (var room in dormitory.GetRooms())
        {
            try { room.RemoveResident(studentId); }
            catch (InvalidOperationException) { }
        }

        student.RemoveFromGroup();
        _students.Remove(student);
    }
    public void EditStudent(string studentId, string name, string surname)
    {
        FindStudent(studentId).EditData(name, surname);
    }
    public Student GetStudent(string studentId) => FindStudent(studentId);
    public IReadOnlyList<Student> GetAllStudents() => _students.AsReadOnly();
    public void AddGroup(Group group)
    {
        if (group == null) throw new ArgumentNullException(nameof(group));
        if (_groups.Any(g => g.Id == group.Id))
            throw new ArgumentException(
                $"Group with ID '{group.Id}' is already registered.", nameof(group));

        _groups.Add(group);
    }
    public void RemoveGroup(string groupId)
    {
        var group = FindGroup(groupId);

        foreach (var student in group.GetStudents())
            student.RemoveFromGroup();

        _groups.Remove(group);
    }
    public void EditGroup(string groupId, string name, string speciality, int year)
    {
        FindGroup(groupId).EditData(name, speciality, year);
    }
    public Group GetGroup(string groupId) => FindGroup(groupId);
    public IReadOnlyList<Group> GetAllGroups() => _groups.AsReadOnly();
    public void AddStudentToGroup(string studentId, string groupId)
    {
        var student = FindStudent(studentId);
        var group = FindGroup(groupId);

        group.AddStudent(student);
        student.AssignToGroup(groupId);
    }
    public void RemoveStudentFromGroup(string studentId, string groupId)
    {
        var student = FindStudent(studentId);
        var group = FindGroup(groupId);

        group.RemoveStudent(studentId);
        student.RemoveFromGroup();
    }
    public void AddDormitory(Dormitory dormitory)
    {
        if (dormitory == null) throw new ArgumentNullException(nameof(dormitory));
        if (_dormitories.Any(d => d.Id == dormitory.Id))
            throw new ArgumentException(
                $"Dormitory with ID '{dormitory.Id}' is already registered.", nameof(dormitory));

        _dormitories.Add(dormitory);
    }
    public void EditDormitoryRoom(string dormitoryId, string roomNumber, int newMaxOccupants)
    {
        FindDormitory(dormitoryId).EditRoomCapacity(roomNumber, newMaxOccupants);
    }
    public void PlaceStudentInRoom(string studentId, string dormitoryId, string roomNumber)
    {
        var student = FindStudent(studentId);
        FindDormitory(dormitoryId).PlaceStudent(student, roomNumber);
    }
    public void CheckOutStudentFromRoom(string studentId, string dormitoryId, string roomNumber)
    {
        FindStudent(studentId);
        FindDormitory(dormitoryId).CheckOutStudent(studentId, roomNumber);
    }
    public IReadOnlyList<Student> GetDormitoryResidents(string dormitoryId)
    {
        return FindDormitory(dormitoryId).GetAllResidents();
    }
    public IReadOnlyList<Student> GetRoomResidents(string dormitoryId, string roomNumber)
    {
        return FindDormitory(dormitoryId).GetRoom(roomNumber).GetResidents();
    }
    public int GetFreeSpots(string dormitoryId)
    {
        return FindDormitory(dormitoryId).TotalFreeSpots;
    }
    public void AddRoomToDormitory(string dormitoryId, string roomNumber, int maxOccupants)
    {
        FindDormitory(dormitoryId).AddRoom(roomNumber, maxOccupants);
    }
    public IReadOnlyList<(string RoomNumber, int Current, int Max)> GetAllRoomsInfo(string dormitoryId)
    {
        return FindDormitory(dormitoryId)
            .GetRooms()
            .Select(r => (r.RoomNumber, r.CurrentOccupants, r.MaxOccupants))
            .ToList()
            .AsReadOnly();
    }
    public IReadOnlyList<Student> SearchStudentsByName(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) throw new ArgumentNullException(nameof(query));

        return _students
            .Where(s => s.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                     || s.Surname.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsReadOnly();
    }
    public IReadOnlyList<Student> SearchStudentsByName(string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(surname)) throw new ArgumentNullException(nameof(surname));

        return _students
            .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase)
                     && s.Surname.Contains(surname, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsReadOnly();
    }
    public IReadOnlyList<Student> SearchStudentsByGroup(string groupId)
    {
        return FindGroup(groupId).GetStudents();
    }
    public IReadOnlyList<Student> SearchStudentsInDormitory(string dormitoryId)
    {
        return GetDormitoryResidents(dormitoryId);
    }

    private Student FindStudent(string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId)) throw new ArgumentNullException(nameof(studentId));
        return _students.FirstOrDefault(s => s.Id == studentId)
            ?? throw new InvalidOperationException($"Student with ID '{studentId}' not found.");
    }

    private Group FindGroup(string groupId)
    {
        if (string.IsNullOrWhiteSpace(groupId)) throw new ArgumentNullException(nameof(groupId));
        return _groups.FirstOrDefault(g => g.Id == groupId)
            ?? throw new InvalidOperationException($"Group with ID '{groupId}' not found.");
    }

    private Dormitory FindDormitory(string dormitoryId)
    {
        if (string.IsNullOrWhiteSpace(dormitoryId)) throw new ArgumentNullException(nameof(dormitoryId));
        return _dormitories.FirstOrDefault(d => d.Id == dormitoryId)
            ?? throw new InvalidOperationException($"Dormitory with ID '{dormitoryId}' not found.");
    }
}
