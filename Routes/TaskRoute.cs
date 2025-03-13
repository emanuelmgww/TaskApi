using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TaskApi.Data;
using TaskApi.Models;

namespace TaskApi.Routes
{
    public static class TaskRoute
    {
        public static void TaskRoutes(this WebApplication app)
        {
            var route = app.MapGroup("task");

            route.MapPost("", async (TaskRequest req, TaskDbContext context) => 
            {
                if (string.IsNullOrWhiteSpace(req.Descricao))
                {
                    return Results.BadRequest("A descrição da tarefa não pode estar vazia!");
                }

                try
                {
                    var task = new TaskModel(req.Descricao);
                    await context.AddAsync(task);
                    await context.SaveChangesAsync();
                    return Results.Created($"/{task.Id}", task);
                    
                }
                catch(DbUpdateException e)
                {
                    return Results.BadRequest($"Erro ao criar a tarefa: {e.Message}");
                }
                catch (Exception)
                {   
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            });

            route.MapGet("", async (TaskDbContext context) => 
            {
                var tasks = await context.Tasks.ToListAsync();
                return Results.Ok(tasks);
            });

            route.MapGet("{id:guid}", async (Guid id, TaskDbContext context) => 
            {
                try
                {
                    var task = await context.Tasks.FindAsync();

                    if (task == null)
                    {
                        return Results.NotFound();
                    }

                    return Results.Ok(task);
                    
                }
                catch (Exception)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            });

            route.MapPut("{id:guid}", async (Guid id, TaskRequest req, TaskDbContext context) =>
            {
                var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

                if (task == null)
                {
                    return Results.NotFound();
                }

                if (!string.IsNullOrWhiteSpace(req.Descricao))
                {
                    task.Descricao = req.Descricao;
                }

                try
                {
                    await context.SaveChangesAsync();
                    return Results.Ok(task);  
                }
                catch (Exception)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }

            });

            route.MapDelete("{id:guid}", async (Guid id, TaskDbContext context) => 
            {
                var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

                if (task == null)
                {
                    return Results.NotFound();
                }

                try
                {
                    context.Tasks.Remove(task);
                    await context.SaveChangesAsync();
                    return Results.NoContent(); 
                }
                catch (DbUpdateException e)
                {
                    return Results.BadRequest($"Erro ao deletar tarefa: {e.Message}");
                }
                catch (Exception)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            });
        }
    }
}