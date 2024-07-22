namespace StokTakip.Customer.Model;

public class QueueData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Content { get; set; } = string.Empty;
}
