namespace CodeNet.Mapper.Tests.Models;

internal class Personel
{
    public int PersonNo { get; set; }
    public string PersonName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<PersonelDetail> Details { get; set; }
    public Note Note { get; set; }
    public Status Status { get; set; }
    public Bolum Department { get; set; }
    public List<int> Ids { get; set; }
    public PersonelDetail Detail { get; set; }
}

internal enum Bolum
{
    //IT = 1,
    HR = 2,
    BU = 3
}

internal class PersonelDetail
{
    public string Description { get; set; }
}