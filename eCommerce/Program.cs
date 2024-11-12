using eCommerce.Data;
using eCommerce.DataAccess.Repository;
using eCommerce.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    ///builder.AllowAnyOrigin();
    ///builder.AllowAnyMethod();
    ///builder.AllowAnyMethod();
    ///builder.AllowCredentials();
    builder.WithOrigins("http://0.0.0.0:80").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    ///builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    ///builder.WithOrigins("http://10.1.1.185:8080").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    ///builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));

builder.Services.AddDbContext<eCommerceDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")));
///    
///builder.Services.AddDbContext<eCommerceDbContext>(options => options.UseSqlServer(
///    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();
/**
app.UseCors(options =>
    options.WithOrigins("http://localhost:4200")
    ///options.WithOrigins("http://10.1.1.185:8082")
    ///options.WithOrigins("*")
    .AllowAnyMethod()
    .AllowAnyHeader()
    ///.AllowAnyOrigin()
    );
*/

///app.UseDefaultFiles();
///app.UseStaticFiles();

app.UseCors("corsapp");
// Configure the HTTP request pipeline.
///if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
