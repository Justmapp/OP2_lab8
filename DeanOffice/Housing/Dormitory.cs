using DeanOffice.Persons;

namespace DeanOffice.Housing;
public class Dormitory
{
    private readonly List<Room> _rooms = new();
    public string Id { get; }
    public string Name { get; private set; }
    public int TotalCapacity => _rooms.Sum(r => r.MaxOccupants);
    public int TotalFreeSpots => _rooms.Sum(r => r.FreeSpots);
    public int TotalResidents => _rooms.Sum(r => r.CurrentOccupants);
    public Dormitory(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

        Id = id;
        Name = name;
    }
    public void EditData(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        Name = name;
    }
    public void AddRoom(string roomNumber, int maxOccupants)
    {
        if (_rooms.Any(r => r.RoomNumber == roomNumber))
            throw new ArgumentException(
                $"Room '{roomNumber}' already exists in dormitory '{Name}'.", nameof(roomNumber));

        _rooms.Add(new Room(roomNumber, maxOccupants));
    }
    public void EditRoomCapacity(string roomNumber, int maxOccupants)
    {
        GetRoom(roomNumber).UpdateMaxOccupants(maxOccupants);
    }
    public void PlaceStudent(Student student, string roomNumber)
    {
        GetRoom(roomNumber).AddResident(student);
    }
    public void CheckOutStudent(string studentId, string roomNumber)
    {
        GetRoom(roomNumber).RemoveResident(studentId);
    }
    public Room GetRoom(string roomNumber)
    {
        return _rooms.FirstOrDefault(r => r.RoomNumber == roomNumber)
            ?? throw new InvalidOperationException(
                $"Room '{roomNumber}' not found in dormitory '{Name}'.");
    }
    public IReadOnlyList<Room> GetRooms() => _rooms.AsReadOnly();
    public IReadOnlyList<Student> GetAllResidents()
    {
        return _rooms.SelectMany(r => r.GetResidents()).ToList().AsReadOnly();
    }
    public override string ToString() =>
        $"'{Name}' | Residents: {TotalResidents}/{TotalCapacity} | Free spots: {TotalFreeSpots}";
}
