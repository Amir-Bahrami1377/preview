namespace FormerUrban_Afta.DataAccess.Model;
public class UserSession
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? UserId { get; set; }
    public string UserAgent { get; set; }
    public string? SessionId { get; set; }
    public string Ip { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(3.5);
    public DateTime? LastActivity { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string Hashed { get; set; }

    public override string ToString() => string.Join("", Id, UserId, UserAgent, Ip, SessionId, CreatedAt.Ticks, LastActivity?.Ticks ?? 0, ExpiresAt?.Ticks ?? 0).Trim();

}
