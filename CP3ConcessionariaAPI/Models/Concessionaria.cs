using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP3ConcessionariaAPI.Models
{
    [Table("CP3_CONCESSIONARIA")]
    public class Concessionaria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CONCESSIONARIA")]
        public int IdConcessionaria { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NM_CONCESSIONARIA")]
        public string NmConcessionaria { get; set; }

        [Required]
        [StringLength(8)]
        [Column("CEP")]
        public string Cep { get; set; }

        [Required]
        [StringLength(200)]
        [Column("DS_ENDERECO")]
        public string DsEndereco { get; set; }
    }
}