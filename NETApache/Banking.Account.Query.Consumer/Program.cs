using Banking.Account.Command.Application.Models;
using Banking.Account.Query.Infrastructure;
using Banking.Account.Query.Infrastructure.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddSingleton<IApplicationBuilder, ApplicationBuilder>();
builder.Services.AddSingleton<IHostedService, BankAccountConsumerService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
