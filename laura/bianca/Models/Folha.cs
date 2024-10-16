using bianca.Models;
namespace bianca.Migrations;

public class Folha
{
    public Folha(int quantidade, double valor, double salarioBruto) {
        Quantidade = quantidade;
        Valor = valor;
        SalarioBruto = salarioBruto;
        CalcularSalarioBruto();
        CalcularImpostoIrrf();
        CalcularImpostoInss();
        CalcularImpostoFgts();
        CalcularSalarioLiquido();
    }

    public int FolhaId {get; set;}
    public double Valor {get; set;}
    public int Quantidade {get; set;}
    public int Mes {get; set;}
    public int Ano {get; set;}
    public int FuncionarioId {get; set;}
    public Funcionario? Funcionario {get; set;}
    public double SalarioBruto {get; set;}
    public double ImpostoIrrf {get; set;}
    public double ImpostoInss {get; set;}
    public double ImpostoFgts {get; set;} 
    public double SalarioLiquido {get; set;}

    public void CalcularSalarioBruto(){
        SalarioBruto = Valor * Quantidade;
    }

    public void CalcularImpostoIrrf(){
        if (SalarioBruto <= 1903.98) {
            ImpostoIrrf = 0;
        } else if (SalarioBruto >= 1903.98 && SalarioBruto <= 2826.65) {
            ImpostoIrrf = (SalarioBruto * 0.075) - 142.80;
        } else if (SalarioBruto >= 2826.65 && SalarioBruto <= 3751.05) {
            ImpostoIrrf = (SalarioBruto * 0.15) - 354.80;
        } else if (SalarioBruto >= 3751.05 && SalarioBruto <= 4664.68) {
            ImpostoIrrf = (SalarioBruto * 0.225) - 636.13;
        } else if (SalarioBruto >= 4664.68) {
            ImpostoIrrf = (SalarioBruto * 0.275) - 869.36;
        }
    }

    public void CalcularImpostoInss(){
        if (SalarioBruto <= 1693.72) {
            ImpostoInss = SalarioBruto * 0.08;
        } else if (SalarioBruto >= 1693.72 && SalarioBruto <= 2822.90) {
            ImpostoInss = SalarioBruto * 0.09;
        } else if (SalarioBruto >= 2822.90 && SalarioBruto <= 5645.80) {
            ImpostoInss = SalarioBruto * 0.11;
        } else if (SalarioBruto >= 5645.80) {
            ImpostoInss = 621.03;
        }
    }

    public void CalcularImpostoFgts(){
        ImpostoFgts = SalarioBruto * 0.08;
        SalarioBruto -= ImpostoFgts;
    }

    public void CalcularSalarioLiquido(){
        SalarioLiquido = SalarioBruto - ImpostoIrrf - ImpostoInss;
    }
}