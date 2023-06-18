using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

List<Receipt> list = new List<Receipt>();

app.MapPost("/jsonHere", (string json) =>
{
    Receipt r = new Receipt(json);
    list.Add(r);
});

app.MapGet("/receipt", () =>
{
    if (list != null && list.Count > 0)
    {
        return Task.FromResult<IActionResult>(new OkObjectResult(list));
    }
    else
    {
        return Task.FromResult<IActionResult>(new NotFoundObjectResult("No receipts available."));
    }
});

app.Run();


public class Receipt
{
    public string receiptOutput { get; set; }

    public Receipt(string jsonString)
    {
        // Parse the JSON array
        JArray jsonArray = JArray.Parse(jsonString);

        // Extract the first object from the array
        JObject firstObject = (JObject)jsonArray[0];

        // Extract the value of the "description" field as a string
        string description = firstObject["description"].ToString();

        this.receiptOutput = description;
    }
}
