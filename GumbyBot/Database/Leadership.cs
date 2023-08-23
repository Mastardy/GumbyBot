namespace GumbyBot.Database;

public class Leadership
{
    public int Id { get; set; }
    public User Leader { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}