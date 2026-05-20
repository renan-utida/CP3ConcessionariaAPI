using CP3ConcessionariaAPI.Models;

namespace CP3ConcessionariaAPI.Services
{
    public class FinanciamentoService
    {
        // Fórmula Price (juros compostos):
        // PMT = PV * (i * (1+i)^n) / ((1+i)^n - 1)
        // PV  = valor financiado (valorVeiculo - valorEntrada)
        // i   = taxa de juros mensal (ex: 1.5 -> 0.015)
        // n   = prazo em meses
        public decimal CalcularParcela(decimal valorVeiculo, decimal valorEntrada, decimal taxaJuros, int prazoMeses)
        {
            if (prazoMeses <= 0)
                throw new ArgumentException("Prazo deve ser maior que zero.");

            if (valorVeiculo <= 0)
                throw new ArgumentException("Valor do veículo deve ser maior que zero.");

            if (valorEntrada < 0 || valorEntrada >= valorVeiculo)
                throw new ArgumentException("Valor de entrada inválido.");

            if (taxaJuros <= 0)
                throw new ArgumentException("Taxa de juros deve ser maior que zero.");

            decimal valorFinanciado = valorVeiculo - valorEntrada;
            decimal i = taxaJuros / 100;
            decimal fator = (decimal)Math.Pow((double)(1 + i), prazoMeses);
            decimal parcela = valorFinanciado * (i * fator) / (fator - 1);

            return Math.Round(parcela, 2);
        }

        public string AvaliarScore(decimal valorVeiculo, decimal valorEntrada)
        {
            if (valorVeiculo <= 0)
                throw new ArgumentException("Valor do veículo deve ser maior que zero.");

            decimal percentualEntrada = (valorEntrada / valorVeiculo) * 100;

            // Score baseado no percentual de entrada
            if (percentualEntrada >= 30)
                return "APROVADO";

            if (percentualEntrada >= 10)
                return "ANALISE";

            return "REPROVADO";
        }

        public Financiamento PreencherFinanciamento(Financiamento financiamento)
        {
            financiamento.ValorParcela = CalcularParcela(
                financiamento.ValorVeiculo,
                financiamento.ValorEntrada,
                financiamento.TaxaJuros,
                financiamento.PrazoMeses
            );

            return financiamento;
        }
    }
}