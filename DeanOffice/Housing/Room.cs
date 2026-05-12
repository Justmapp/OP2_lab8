using DeanOffice.Persons;

namespace DeanOffice.Housing;
public class Room
{
    private readonly List<Student> _residents = new();
    public string RoomNumber { get; }
    public int MaxOccupants { get; private set; }
    public int CurrentOccupants => _residents.Count;
    public int FreeSpots => MaxOccupants - _residents.Count;
    public bool IsFull => _residents.Count >= MaxOccupants;
    internal Room(string roomNumber, int maxOccupants)
    {
        if (string.IsNullOrWhiteSpace(roomNumber)) throw new ArgumentNullException(nameof(roomNumber));
        if (maxOccupants < 1)
            throw new ArgumentException("MaxOccupants must be at least 1.", nameof(maxOccupants));

        RoomNumber = roomNumber;
        MaxOccupants = maxOccupants;
    }
    public void UpdateMaxOccupants(int maxOccupants)
    {
        if (maxOccupants < _residents.Count)
            throw new ArgumentException(
                $"Cannot reduce capacity to {maxOccupants}: room already has {_residents.Count} residents.",
                nameof(maxOccupants));

        MaxOccupants = maxOccupants;
    }
    public void AddResident(Student student)
    {
        if (student == null) throw new ArgumentNullException(nameof(student));
        if (IsFull)
            throw new InvalidOperationException($"Room {RoomNumber} is at full capacity ({MaxOccupants}).");
        if (_residents.Any(r => r.Id == student.Id))
            throw new InvalidOperationException(
                $"Student '{student.GetFullName()}' is already a resident of room {RoomNumber}.");

        _residents.Add(student);
    }
    public void RemoveResident(string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId)) throw new ArgumentNullException(nameof(studentId));

        var student = _residents.FirstOrDefault(r => r.Id == studentId)
            ?? throw new InvalidOperationException(
                $"Student with ID '{studentId}' is not a resident of room {RoomNumber}.");

        _residents.Remove(student);
    }
    public IReadOnlyList<Student> GetResidents() => _residents.AsReadOnly();
    public override string ToString() => $"Room {RoomNumber} ({CurrentOccupants}/{MaxOccupants})";
}
