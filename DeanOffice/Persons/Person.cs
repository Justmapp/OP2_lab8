namespace DeanOffice.Persons;
public abstract class Person
{
    public string Id { get; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    protected Person(string id, string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(surname)) throw new ArgumentNullException(nameof(surname));

        Id = id;
        Name = name;
        Surname = surname;
    }
    public void EditData(string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(surname)) throw new ArgumentNullException(nameof(surname));

        Name = name;
        Surname = surname;
    }
    public string GetFullName() => $"{Surname} {Name}";
    public override string ToString() => GetFullName();
}
