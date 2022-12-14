using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Serwisy;

const string CookieScheme = "YourSchemeName";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("PortfelContext");
builder.Services.AddDbContext<PortfelContext>(opts => opts.UseSqlServer(connectionString));
builder.Services.AddTransient<IPortfelSerwis, PortfelSerwis>();

builder.Services.AddMvc();

builder.Services.AddAuthentication(CookieScheme) // Sets the default scheme to cookies
    .AddCookie(CookieScheme, options =>
    {
        options.AccessDeniedPath = "/account/denied";
        options.LoginPath = "/uzytkownik/StronaLogowania";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
