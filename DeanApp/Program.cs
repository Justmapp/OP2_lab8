using DeanOffice;
using DeanOffice.Academic;
using DeanOffice.Housing;
using DeanOffice.Persons;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

IDeanSystem registry = new DeanRegistry();

while (true)
{
    PrintHeader("ЕЛЕКТРОННИЙ ДЕКАНАТ");
    Console.WriteLine("  1. Управління студентами");
    Console.WriteLine("  2. Управління групами");
    Console.WriteLine("  3. Управління гуртожитком");
    Console.WriteLine("  4. Пошук");
    Console.WriteLine("  5. Додати демо-дані");
    Console.WriteLine("  0. Вихід");
    Console.WriteLine();

    switch (ReadChoice("Оберіть розділ", 0, 5))
    {
        case 1: StudentMenu(); break;
        case 2: GroupMenu(); break;
        case 3: DormitoryMenu(); break;
        case 4: SearchMenu(); break;
        case 5: AddDemoData(); break;
        case 0: Console.WriteLine("\n  До побачення!"); return;
    }
}

// ═══════════════════════════════════════════════════════════════
// СТУДЕНТИ
// ═══════════════════════════════════════════════════════════════

void StudentMenu()
{
    while (true)
    {
        PrintHeader("УПРАВЛІННЯ СТУДЕНТАМИ");
        Console.WriteLine("  1. Показати всіх студентів");
        Console.WriteLine("  2. Показати студента");
        Console.WriteLine("  3. Додати студента");
        Console.WriteLine("  4. Редагувати студента");
        Console.WriteLine("  5. Видалити студента");
        Console.WriteLine("  0. Назад");
        Console.WriteLine();

        switch (ReadChoice("Оберіть дію", 0, 5))
        {
            case 1:
                PrintSection("Всі студенти");
                var all = registry.GetAllStudents();
                if (all.Count == 0) { Warn("Жодного студента не зареєстровано."); break; }
                foreach (var s in all)
                    Console.WriteLine($"  [{s.Id}] {s} | Група: {s.GroupId ?? "—"}");
                Pause();
                break;

            case 2:
                PrintSection("Переглянути студента");
                var sid = Ask("ID студента");
                Try(() =>
                {
                    var s = registry.GetStudent(sid);
                    Console.WriteLine($"\n  ПІБ:    {s.GetFullName()}");
                    Console.WriteLine($"  Код:    {s.StudentCode}");
                    Console.WriteLine($"  Група:  {s.GroupId ?? "не призначено"}");
                    Pause();
                });
                break;

            case 3:
                PrintSection("Додати студента");
                Try(() =>
                {
                    var id      = Ask("ID (унікальний)");
                    var name    = Ask("Ім'я");
                    var surname = Ask("Прізвище");
                    var code    = Ask("Код студента (напр. IP-51)");
                    registry.AddStudent(new Student(id, name, surname, code));
                    Ok($"Студента {surname} {name} додано.");
                });
                break;

            case 4:
                PrintSection("Редагувати студента");
                Try(() =>
                {
                    var id      = Ask("ID студента");
                    var name    = Ask("Нове ім'я");
                    var surname = Ask("Нове прізвище");
                    registry.EditStudent(id, name, surname);
                    Ok("Дані оновлено.");
                });
                break;

            case 5:
                PrintSection("Видалити студента");
                Try(() =>
                {
                    var id = Ask("ID студента");
                    var s  = registry.GetStudent(id);
                    Console.Write($"\n  Видалити «{s.GetFullName()}»? (т/н): ");
                    if (Console.ReadLine()?.Trim().ToLower() == "т")
                    {
                        registry.RemoveStudent(id);
                        Ok("Студента видалено.");
                    }
                    else Warn("Скасовано.");
                });
                break;

            case 0: return;
        }
    }
}

// ═══════════════════════════════════════════════════════════════
// ГРУПИ
// ═══════════════════════════════════════════════════════════════

void GroupMenu()
{
    while (true)
    {
        PrintHeader("УПРАВЛІННЯ ГРУПАМИ");
        Console.WriteLine("  1. Показати всі групи");
        Console.WriteLine("  2. Показати склад групи");
        Console.WriteLine("  3. Додати групу");
        Console.WriteLine("  4. Редагувати групу");
        Console.WriteLine("  5. Видалити групу");
        Console.WriteLine("  6. Додати студента до групи");
        Console.WriteLine("  7. Видалити студента з групи");
        Console.WriteLine("  0. Назад");
        Console.WriteLine();

        switch (ReadChoice("Оберіть дію", 0, 7))
        {
            case 1:
                PrintSection("Всі групи");
                var groups = registry.GetAllGroups();
                if (groups.Count == 0) { Warn("Жодної групи не зареєстровано."); break; }
                foreach (var g in groups)
                    Console.WriteLine($"  [{g.Id}] {g} | Студентів: {g.GetStudents().Count}");
                Pause();
                break;

            case 2:
                PrintSection("Склад групи");
                Try(() =>
                {
                    var gid = Ask("ID групи");
                    var g   = registry.GetGroup(gid);
                    Console.WriteLine($"\n  {g}");
                    var students = g.GetStudents();
                    if (students.Count == 0) { Warn("Група порожня."); return; }
                    foreach (var s in students)
                        Console.WriteLine($"    • [{s.Id}] {s}");
                    Pause();
                });
                break;

            case 3:
                PrintSection("Додати групу");
                Try(() =>
                {
                    var id   = Ask("ID (унікальний)");
                    var name = Ask("Назва групи (напр. IP-51)");
                    var spec = Ask("Спеціальність");
                    var year = int.Parse(Ask("Курс (1–6)"));
                    registry.AddGroup(new Group(id, name, spec, year));
                    Ok($"Групу {name} додано.");
                });
                break;

            case 4:
                PrintSection("Редагувати групу");
                Try(() =>
                {
                    var gid  = Ask("ID групи");
                    var name = Ask("Нова назва");
                    var spec = Ask("Нова спеціальність");
                    var year = int.Parse(Ask("Новий курс (1–6)"));
                    registry.EditGroup(gid, name, spec, year);
                    Ok("Дані оновлено.");
                });
                break;

            case 5:
                PrintSection("Видалити групу");
                Try(() =>
                {
                    var gid = Ask("ID групи");
                    var g   = registry.GetGroup(gid);
                    Console.Write($"\n  Видалити групу «{g.Name}»? (т/н): ");
                    if (Console.ReadLine()?.Trim().ToLower() == "т")
                    {
                        registry.RemoveGroup(gid);
                        Ok("Групу видалено.");
                    }
                    else Warn("Скасовано.");
                });
                break;

            case 6:
                PrintSection("Додати студента до групи");
                Try(() =>
                {
                    var sid = Ask("ID студента");
                    var gid = Ask("ID групи");
                    registry.AddStudentToGroup(sid, gid);
                    Ok("Студента додано до групи.");
                });
                break;

            case 7:
                PrintSection("Видалити студента з групи");
                Try(() =>
                {
                    var sid = Ask("ID студента");
                    var gid = Ask("ID групи");
                    registry.RemoveStudentFromGroup(sid, gid);
                    Ok("Студента видалено з групи.");
                });
                break;

            case 0: return;
        }
    }
}

// ═══════════════════════════════════════════════════════════════
// ГУРТОЖИТОК
// ═══════════════════════════════════════════════════════════════

void DormitoryMenu()
{
    while (true)
    {
        PrintHeader("УПРАВЛІННЯ ГУРТОЖИТКОМ");
        Console.WriteLine("  1. Показати всі гуртожитки та кімнати");
        Console.WriteLine("  2. Показати мешканців кімнати");
        Console.WriteLine("  3. Додати гуртожиток");
        Console.WriteLine("  4. Додати кімнату до гуртожитку");
        Console.WriteLine("  5. Змінити місткість кімнати");
        Console.WriteLine("  6. Поселити студента");
        Console.WriteLine("  7. Виписати студента");
        Console.WriteLine("  0. Назад");
        Console.WriteLine();

        switch (ReadChoice("Оберіть дію", 0, 7))
        {
            case 1:
                PrintSection("Гуртожитки");
                PrintAllDormitories();
                Pause();
                break;

            case 2:
                PrintSection("Мешканці кімнати");
                Try(() =>
                {
                    var did = Ask("ID гуртожитку");
                    var rn  = Ask("Номер кімнати");
                    var res = registry.GetRoomResidents(did, rn);
                    if (res.Count == 0) { Warn("Кімната порожня."); return; }
                    foreach (var s in res)
                        Console.WriteLine($"  • [{s.Id}] {s}");
                    Pause();
                });
                break;

            case 3:
                PrintSection("Додати гуртожиток");
                Try(() =>
                {
                    var id   = Ask("ID (унікальний)");
                    var name = Ask("Назва гуртожитку");
                    registry.AddDormitory(new Dormitory(id, name));
                    Ok($"Гуртожиток «{name}» додано. Тепер додайте кімнати (пункт 4).");
                });
                break;

            case 4:
                PrintSection("Додати кімнату");
                Try(() =>
                {
                    var did  = Ask("ID гуртожитку");
                    var rn   = Ask("Номер кімнати");
                    var maxO = int.Parse(Ask("Максимальна кількість мешканців"));
                    registry.AddRoomToDormitory(did, rn, maxO);
                    Ok($"Кімнату {rn} додано.");
                });
                break;

            case 5:
                PrintSection("Змінити місткість кімнати");
                Try(() =>
                {
                    var did  = Ask("ID гуртожитку");
                    var rn   = Ask("Номер кімнати");
                    var maxO = int.Parse(Ask("Нова місткість"));
                    registry.EditDormitoryRoom(did, rn, maxO);
                    Ok("Місткість оновлено.");
                });
                break;

            case 6:
                PrintSection("Поселити студента");
                Try(() =>
                {
                    var sid = Ask("ID студента");
                    var did = Ask("ID гуртожитку");
                    var rn  = Ask("Номер кімнати");
                    registry.PlaceStudentInRoom(sid, did, rn);
                    Ok("Студента поселено.");
                });
                break;

            case 7:
                PrintSection("Виписати студента");
                Try(() =>
                {
                    var sid = Ask("ID студента");
                    var did = Ask("ID гуртожитку");
                    var rn  = Ask("Номер кімнати");
                    registry.CheckOutStudentFromRoom(sid, did, rn);
                    Ok("Студента виписано.");
                });
                break;

            case 0: return;
        }
    }
}

// ═══════════════════════════════════════════════════════════════
// ПОШУК
// ═══════════════════════════════════════════════════════════════

void SearchMenu()
{
    while (true)
    {
        PrintHeader("ПОШУК");
        Console.WriteLine("  1. За ім'ям або прізвищем");
        Console.WriteLine("  2. За ім'ям та прізвищем");
        Console.WriteLine("  3. Студенти певної групи");
        Console.WriteLine("  4. Студенти у гуртожитку");
        Console.WriteLine("  0. Назад");
        Console.WriteLine();

        switch (ReadChoice("Оберіть дію", 0, 4))
        {
            case 1:
                Try(() =>
                {
                    var q = Ask("Запит (ім'я або прізвище)");
                    PrintStudentList(registry.SearchStudentsByName(q));
                });
                break;
            case 2:
                Try(() =>
                {
                    var name    = Ask("Ім'я");
                    var surname = Ask("Прізвище");
                    PrintStudentList(registry.SearchStudentsByName(name, surname));
                });
                break;
            case 3:
                Try(() =>
                {
                    var gid = Ask("ID групи");
                    PrintStudentList(registry.SearchStudentsByGroup(gid));
                });
                break;
            case 4:
                Try(() =>
                {
                    var did = Ask("ID гуртожитку");
                    PrintStudentList(registry.SearchStudentsInDormitory(did));
                });
                break;
            case 0: return;
        }
    }
}

// ═══════════════════════════════════════════════════════════════
// ДЕМО-ДАНІ
// ═══════════════════════════════════════════════════════════════

void AddDemoData()
{
    PrintSection("Додавання демо-даних");

    int skipped = 0;
    void Safe(Action a) { try { a(); } catch { skipped++; } }

    Safe(() => registry.AddStudent(new Student("s1", "Олексій",  "Іваненко",   "IP-51")));
    Safe(() => registry.AddStudent(new Student("s2", "Марія",    "Коваленко",  "IP-51")));
    Safe(() => registry.AddStudent(new Student("s3", "Петро",    "Сидоренко",  "IP-52")));
    Safe(() => registry.AddStudent(new Student("s4", "Анна",     "Мельник",    "IP-52")));
    Safe(() => registry.AddStudent(new Student("s5", "Дмитро",   "Шевченко",   "IP-53")));
    Safe(() => registry.AddStudent(new Student("s6", "Оксана",   "Бондаренко", "IP-53")));

    Safe(() => registry.AddGroup(new Group("g1", "IP-51", "Інженерія програмного забезпечення", 1)));
    Safe(() => registry.AddGroup(new Group("g2", "IP-52", "Інженерія програмного забезпечення", 2)));
    Safe(() => registry.AddGroup(new Group("g3", "IP-53", "Комп'ютерні науки", 3)));

    Safe(() => registry.AddStudentToGroup("s1", "g1"));
    Safe(() => registry.AddStudentToGroup("s2", "g1"));
    Safe(() => registry.AddStudentToGroup("s3", "g2"));
    Safe(() => registry.AddStudentToGroup("s4", "g2"));
    Safe(() => registry.AddStudentToGroup("s5", "g3"));
    Safe(() => registry.AddStudentToGroup("s6", "g3"));

    var dorm1 = new Dormitory("d1", "Гуртожиток №1");
    dorm1.AddRoom("101", 2);
    dorm1.AddRoom("102", 3);
    dorm1.AddRoom("103", 2);
    Safe(() => registry.AddDormitory(dorm1));

    var dorm2 = new Dormitory("d2", "Гуртожиток №2");
    dorm2.AddRoom("201", 2);
    dorm2.AddRoom("202", 2);
    Safe(() => registry.AddDormitory(dorm2));

    Safe(() => registry.PlaceStudentInRoom("s1", "d1", "101"));
    Safe(() => registry.PlaceStudentInRoom("s3", "d1", "101"));
    Safe(() => registry.PlaceStudentInRoom("s4", "d1", "102"));
    Safe(() => registry.PlaceStudentInRoom("s5", "d2", "201"));

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("  ✓ Демо-дані додано!");
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine("  Студенти:    s1 Іваненко, s2 Коваленко, s3 Сидоренко,");
    Console.WriteLine("               s4 Мельник, s5 Шевченко, s6 Бондаренко");
    Console.WriteLine("  Групи:       g1=IP-51, g2=IP-52, g3=IP-53");
    Console.WriteLine("  Гуртожитки: d1 (кімнати 101,102,103), d2 (кімнати 201,202)");
    if (skipped > 0)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n  ! {skipped} записів вже існували — пропущено.");
        Console.ResetColor();
    }
    Pause();
}

// ═══════════════════════════════════════════════════════════════
// ДОПОМІЖНІ МЕТОДИ
// ═══════════════════════════════════════════════════════════════

void PrintAllDormitories()
{
    var knownIds = new[] { "d1", "d2", "d3", "d4", "d5" };
    bool any = false;
    foreach (var did in knownIds)
    {
        try
        {
            var free = registry.GetFreeSpots(did);
            var rooms = registry.GetAllRoomsInfo(did);
            Console.WriteLine($"\n  [{did}] Вільних місць: {free}");
            foreach (var (rn, cur, max) in rooms)
            {
                Console.WriteLine($"    Кімната {rn}: {cur}/{max}");
                foreach (var s in registry.GetRoomResidents(did, rn))
                    Console.WriteLine($"      • [{s.Id}] {s}");
            }
            any = true;
        }
        catch { }
    }
    if (!any) Warn("Гуртожитків не зареєстровано.");
}

static void PrintHeader(string title)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════════════════╗");
    Console.WriteLine($"║  {title,-48}║");
    Console.WriteLine("╚══════════════════════════════════════════════════╝");
    Console.WriteLine();
}

static void PrintSection(string title)
{
    Console.WriteLine();
    Console.WriteLine($"  ── {title} ──");
    Console.WriteLine();
}

static string Ask(string prompt)
{
    Console.Write($"  {prompt}: ");
    return Console.ReadLine()?.Trim() ?? "";
}

static int ReadChoice(string prompt, int min, int max)
{
    while (true)
    {
        Console.Write($"  {prompt} ({min}–{max}): ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
            return choice;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  ! Введіть число від {min} до {max}.");
        Console.ResetColor();
    }
}

static void Ok(string msg)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\n  ✓ {msg}");
    Console.ResetColor();
    Pause();
}

static void Warn(string msg)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n  ! {msg}");
    Console.ResetColor();
    Pause();
}

static void Pause()
{
    Console.WriteLine();
    Console.Write("  [Enter щоб продовжити]");
    Console.ReadLine();
}

static void PrintStudentList(IReadOnlyList<Student> list)
{
    if (list.Count == 0) { Warn("Нікого не знайдено."); return; }
    Console.WriteLine();
    foreach (var s in list)
        Console.WriteLine($"  • [{s.Id}] {s} | Група: {s.GroupId ?? "—"}");
    Pause();
}

static void Try(Action action)
{
    try { action(); }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n  ✗ Помилка: {ex.Message}");
        Console.ResetColor();
        Pause();
    }
}
