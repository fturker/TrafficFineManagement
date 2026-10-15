using TrafficFineManagement.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(
    options =>
    {
        options.Filters.Add(
            new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
    });

builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=login}/{id?}")
    .WithStaticAssets();

if (app.Environment.IsDevelopment())
{
    await app.SeedDatabaseAsync();
}


app.Run();
