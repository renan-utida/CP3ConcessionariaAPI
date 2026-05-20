using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP3ConcessionariaAPI.Models
{
    [Table("CP3_CLIENTE")]
    public abstract class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CLIENTE")]
        public int IdCliente { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NM_CLIENTE")]
        public string NmCliente { get; set; }

        [Column("ID_CONCESSIONARIA")]
        public int IdConcessionaria { get; set; }

        [ForeignKey("IdConcessionaria")]
        public Concessionaria? Concessionaria { get; set; }
    }
}