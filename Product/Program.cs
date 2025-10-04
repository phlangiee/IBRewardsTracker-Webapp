using AffinityCard_Namespace.Data;
using AffinityProgram_Namespace.Data;
using Person_Namespace.Data;
using Type_Namespace.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});
builder.Services.AddRazorPages();


string databasePath = @"Data Source=C:\Users\1057247\Downloads\travelapp\Sqlite\Database.db";

builder.Services.AddScoped<AffinityCardData>(provider => new AffinityCardData(databasePath));
builder.Services.AddScoped<AffinityProgramData>(provider => new AffinityProgramData(databasePath));
builder.Services.AddScoped<PersonData>(provider => new PersonData(databasePath));
builder.Services.AddScoped<TypeData>(provider => new TypeData(databasePath));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


app.Run();

