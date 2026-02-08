using System.ComponentModel.DataAnnotations.Schema;

namespace FormerUrban_Afta.DataAccess.Model.BaseEntity
{
    public abstract class BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Identity { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreateUser { get; set; }
        public Guid? ModifiedUser { get; set; }
        public string Hashed { get; set; }
    }
}
