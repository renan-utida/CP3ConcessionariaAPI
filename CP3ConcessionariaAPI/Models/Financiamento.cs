using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP3ConcessionariaAPI.Models
{
    public class Financiamento : Produto
    {
        [Required]
        [Column("VL_VEICULO")]
        public decimal ValorVeiculo { get; set; }

        [Required]
        [Column("VL_ENTRADA")]
        public decimal ValorEntrada { get; set; }

        [Required]
        [Column("TX_JUROS")]
        public decimal TaxaJuros { get; set; }

        [Required]
        [Column("NR_PRAZO_MESES")]
        public int PrazoMeses { get; set; }

        [Column("VL_PARCELA")]
        public decimal ValorParcela { get; set; }
    }
}