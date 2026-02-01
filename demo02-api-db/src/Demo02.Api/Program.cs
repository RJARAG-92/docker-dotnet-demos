 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repo thin con Npgsql (sin EF Core)
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

static string GetConnectionString(IConfiguration cfg)
    => cfg.GetConnectionString("Default")
       ?? cfg["CONNECTION_STRING"]
       ?? throw new InvalidOperationException("Missing connection string (ConnectionStrings:Default or CONNECTION_STRING).");

// Bootstrap DB (crea tabla si no existe)
await DbInitializer.EnsureCreatedAsync(GetConnectionString(app.Configuration), CancellationToken.None);

app.MapGet("/health", () => Results.Ok(new { status = "OK" }));

app.MapGet("/tasks", async (ITaskRepository repo, CancellationToken ct)
    => Results.Ok(await repo.ListAsync(ct)));

app.MapGet("/tasks/{id:guid}", async (Guid id, ITaskRepository repo, CancellationToken ct) =>
{
    var task = await repo.GetByIdAsync(id, ct);
    return task is null
        ? Results.NotFound(new { error = "task not found" })
        : Results.Ok(task);
})
.WithName("GetTaskById");

app.MapPost("/tasks", async (CreateTaskRequest req, ITaskRepository repo, CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(req.Title))
        return Results.BadRequest(new { error = "title is required" });

    var created = await repo.CreateAsync(req.Title, ct);
    return Results.CreatedAtRoute(
        routeName: "GetTaskById",
        routeValues: new { id = created.Id },
        value: created
    );
});

app.MapPut("/tasks/{id:guid}/done", async (Guid id, ITaskRepository repo, CancellationToken ct)
    => await repo.MarkDoneAsync(id, ct)
        ? Results.Ok(new { ok = true })
        : Results.NotFound(new { error = "task not found" }));

app.MapDelete("/tasks/{id:guid}", async (Guid id, ITaskRepository repo, CancellationToken ct)
    => await repo.DeleteAsync(id, ct)
        ? Results.NoContent()
        : Results.NotFound(new { error = "task not found" }));

app.Run();