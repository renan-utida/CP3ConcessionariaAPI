using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CP3ConcessionariaAPI.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelasIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CP3_CONCESSIONARIA",
                columns: table => new
                {
                    ID_CONCESSIONARIA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_CONCESSIONARIA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CEP = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    DS_ENDERECO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CP3_CONCESSIONARIA", x => x.ID_CONCESSIONARIA);
                });

            migrationBuilder.CreateTable(
                name: "CP3_PRODUTO",
                columns: table => new
                {
                    ID_PRODUTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_PRODUTO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TIPO_PRODUTO = table.Column<string>(type: "NVARCHAR2(13)", maxLength: 13, nullable: false),
                    VL_VEICULO = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: true),
                    VL_ENTRADA = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: true),
                    TX_JUROS = table.Column<decimal>(type: "DECIMAL(5,2)", precision: 5, scale: 2, nullable: true),
                    NR_PRAZO_MESES = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    VL_PARCELA = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CP3_PRODUTO", x => x.ID_PRODUTO);
                });

            migrationBuilder.CreateTable(
                name: "CP3_CLIENTE",
                columns: table => new
                {
                    ID_CLIENTE = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_CLIENTE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_CONCESSIONARIA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO_CLIENTE = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: true),
                    DT_NASCIMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CNPJ = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: true),
                    RAZAO_SOCIAL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CP3_CLIENTE", x => x.ID_CLIENTE);
                    table.ForeignKey(
                        name: "FK_CP3_CLIENTE_CP3_CONCESSIONARIA_ID_CONCESSIONARIA",
                        column: x => x.ID_CONCESSIONARIA,
                        principalTable: "CP3_CONCESSIONARIA",
                        principalColumn: "ID_CONCESSIONARIA",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CP3_CONTRATACAO",
                columns: table => new
                {
                    ID_CONTRATACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_CLIENTE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_PRODUTO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DT_SOLICITACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CP3_CONTRATACAO", x => x.ID_CONTRATACAO);
                    table.ForeignKey(
                        name: "FK_CP3_CONTRATACAO_CP3_CLIENTE_ID_CLIENTE",
                        column: x => x.ID_CLIENTE,
                        principalTable: "CP3_CLIENTE",
                        principalColumn: "ID_CLIENTE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CP3_CONTRATACAO_CP3_PRODUTO_ID_PRODUTO",
                        column: x => x.ID_PRODUTO,
                        principalTable: "CP3_PRODUTO",
                        principalColumn: "ID_PRODUTO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CP3_CLIENTE_ID_CONCESSIONARIA",
                table: "CP3_CLIENTE",
                column: "ID_CONCESSIONARIA");

            migrationBuilder.CreateIndex(
                name: "IX_CP3_CONTRATACAO_ID_CLIENTE",
                table: "CP3_CONTRATACAO",
                column: "ID_CLIENTE");

            migrationBuilder.CreateIndex(
                name: "IX_CP3_CONTRATACAO_ID_PRODUTO",
                table: "CP3_CONTRATACAO",
                column: "ID_PRODUTO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CP3_CONTRATACAO");

            migrationBuilder.DropTable(
                name: "CP3_CLIENTE");

            migrationBuilder.DropTable(
                name: "CP3_PRODUTO");

            migrationBuilder.DropTable(
                name: "CP3_CONCESSIONARIA");
        }
    }
}
