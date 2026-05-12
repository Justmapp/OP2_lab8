using DeanOffice.Academic;
using DeanOffice.Housing;
using DeanOffice.Persons;

namespace DeanOffice;
public interface IDeanSystem
{
    void AddStudent(Student student);
    void RemoveStudent(string studentId);
    void EditStudent(string studentId, string name, string surname);
    Student GetStudent(string studentId);
    IReadOnlyList<Student> GetAllStudents();
    void AddGroup(Group group);
    void RemoveGroup(string groupId);
    void EditGroup(string groupId, string name, string speciality, int year);
    Group GetGroup(string groupId);
    IReadOnlyList<Group> GetAllGroups();
    void AddStudentToGroup(string studentId, string groupId);
    void RemoveStudentFromGroup(string studentId, string groupId);
    void AddDormitory(Dormitory dormitory);
    void EditDormitoryRoom(string dormitoryId, string roomNumber, int newMaxOccupants);
    void PlaceStudentInRoom(string studentId, string dormitoryId, string roomNumber);
    void CheckOutStudentFromRoom(string studentId, string dormitoryId, string roomNumber);
    IReadOnlyList<Student> GetDormitoryResidents(string dormitoryId);
    IReadOnlyList<Student> GetRoomResidents(string dormitoryId, string roomNumber);
    int GetFreeSpots(string dormitoryId);
    void AddRoomToDormitory(string dormitoryId, string roomNumber, int maxOccupants);
    IReadOnlyList<(string RoomNumber, int Current, int Max)> GetAllRoomsInfo(string dormitoryId);
    IReadOnlyList<Student> SearchStudentsByName(string query);
    IReadOnlyList<Student> SearchStudentsByName(string name, string surname);
    IReadOnlyList<Student> SearchStudentsByGroup(string groupId);
    IReadOnlyList<Student> SearchStudentsInDormitory(string dormitoryId);
}
