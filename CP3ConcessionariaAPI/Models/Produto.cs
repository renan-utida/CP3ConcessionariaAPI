using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP3ConcessionariaAPI.Models
{
    [Table("CP3_PRODUTO")]
    public abstract class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_PRODUTO")]
        public int IdProduto { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NM_PRODUTO")]
        public string NmProduto { get; set; }
    }
}