using ScrumBoardApi.Models;
using ScrumBoardApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<CardService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

app.MapGet("/", () => "Scrum Board API está no ar!");

app.MapGet("/api/cards", (CardService svc) =>
{
    return Results.Ok(svc.GetAll());
});

app.MapGet("/api/cards/{id:guid}", (Guid id, CardService svc) =>
{
    var card = svc.GetById(id);
    return card is not null ? Results.Ok(card) : Results.NotFound();
});

app.MapGet("/api/cards/status/{status}", (string status, CardService svc) =>
{
    var cards = svc.GetByStatus(status);
    return Results.Ok(cards);
});

app.MapPost("/api/cards", (Card card, CardService svc) =>
{
    var created = svc.Add(card);
    return Results.Created($"/api/cards/{created.Id}", created);
});

app.MapPut("/api/cards/{id:guid}", (Guid id, Card updated, CardService svc) =>
{
    var card = svc.Update(id, updated);
    return card is not null ? Results.Ok(card) : Results.NotFound();
});

app.MapPatch("/api/cards/{id:guid}/move", (Guid id, MoveRequest request, CardService svc) =>
{
    string[] validStatuses = { "Backlog", "ToDo", "Doing", "Testing", "Done" };

    if (!validStatuses.Contains(request.Status, StringComparer.OrdinalIgnoreCase))
    {
        return Results.BadRequest(new
        {
            error = "Status inválido",
            validStatuses
        });
    }

    var card = svc.MoveCard(id, request.Status);
    return card is not null ? Results.Ok(card) : Results.NotFound();
});

app.MapDelete("/api/cards/{id:guid}", (Guid id, CardService svc) =>
{
    return svc.Delete(id) ? Results.NoContent() : Results.NotFound();
});

app.Run();

public record MoveRequest(string Status);