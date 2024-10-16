using bianca.Migrations;
using bianca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapGet("/", () => "Folha de pagamento");

app.MapPost("/api/funcionario/cadastrar", ([FromBody] Funcionario funcionario, [FromServices] AppDataContext ctx) =>
{
    ctx.Funcionarios.Add(funcionario);
    ctx.SaveChanges();
    return Results.Created("", funcionario);
});

app.MapGet("/api/funcionario/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Funcionarios.Any()) {
        return Results.Ok(ctx.Funcionarios.ToList());
    }

    return Results.NotFound();
});

app.MapPost("/api/folha/cadastrar", ([FromBody] Folha folha, [FromServices] AppDataContext ctx) =>
{   
    Funcionario? funcionario = ctx.Funcionarios.Find(folha.FuncionarioId);
    if (funcionario == null) {
        return Results.NotFound();
    }

    folha.Funcionario = funcionario;

    ctx.Folhas.Add(folha);
    ctx.SaveChanges();
    return Results.Created("", folha);
});

app.MapGet("/api/folha/listar", ([FromServices] AppDataContext ctx) =>
{
    List<Folha> folhas = ctx.Folhas.Include(f => f.Funcionario).ToList();
    
    if (ctx.Folhas.Any()) {
        return Results.Ok(ctx.Folhas.ToList());
    }

    return Results.NotFound();
});

app.MapGet("/api/folha/buscar/{cpf}/{mes}/{ano}", ([FromRoute] string? cpf, [FromRoute] int mes, [FromRoute] int ano, [FromServices] AppDataContext ctx) =>
{
    Folha? folha = ctx.Folhas.Include(f => f.Funcionario).FirstOrDefault(f => f.Funcionario.CPF == cpf && f.Mes == mes && f.Ano == ano);
    
    if (folha is null) {
        return Results.NotFound();
    }

    return Results.Ok(folha);
});

app.Run();
