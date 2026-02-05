using CleanArchitecture.Application.Products.Commands.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // A veces necesario para Results

namespace CleanArchitecture.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products");

        // POST api/products
        group.MapPost("/", async (ISender sender, [FromBody] CreateProductCommand command) =>
        {
            var id = await sender.Send(command);
            return Results.Ok(id);
        })
        .WithName("CreateProduct")
        .WithOpenApi();
    }
}