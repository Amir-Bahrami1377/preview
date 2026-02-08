using System.ComponentModel.DataAnnotations.Schema;

namespace FormerUrban_Afta.DataAccess.Model;

public class Audit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public EnumFormName Form { get; set; }

    [Required]
    public string EntityId { get; set; }

    [Required]
    public EnumOperation Action { get; set; }

    [Required]
    public string Field { get; set; }

    [Required]
    public string OriginValue { get; set; }

    [Required]
    public string CurrentValue { get; set; }

    [Required]
    [MaxLength(100)]
    public string ChangedBy { get; set; } = string.Empty;

    [Required]
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow.AddHours(3.5);

    [MaxLength(45)]
    public string? IpAddress { get; set; }

    public string Hashed { get; set; }

    public override string ToString() => string.Join("", Form, EntityId, Action, Field, OriginValue, CurrentValue, ChangedBy, ChangedAt, IpAddress);

}
