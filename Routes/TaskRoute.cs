using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                catch (Exception)
                {   
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            });

            
        }
    }
}