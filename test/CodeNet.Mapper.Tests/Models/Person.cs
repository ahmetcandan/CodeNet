namespace CodeNet.Mapper.Tests.Models;

internal class Person
{
    public int PersonNo { get; set; }
    public List<PersonDetail> Details { get; set; } = [];
    public string PersonName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Note? Note { get; set; }
    public Status Status { get; set; }
    public Department Department { get; set; }
    public List<int> Ids { get; set; } = [];
    public PersonDetail? Detail { get; set; }
}

internal enum Status
{
    Active = 1,
    Inactive = 2
}

internal enum Department
{
    IT = 1,
    HR = 2,
    BU = 3
}

internal class PersonDetail
{
    public string Description { get; set; } = string.Empty;
}

internal class Note
{
    public string Context { get; set; } = string.Empty;
}
