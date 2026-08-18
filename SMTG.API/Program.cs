using Supabase;
using SMTG.API.Services;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// SERVER
// ======================================================

// API accessible depuis le réseau interne
// Exemple : http://10.50.31.88:5072
builder.WebHost.UseUrls("http://0.0.0.0:5072");

// ======================================================
// SERVICES
// ======================================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ======================================================
// CORS
// ======================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("SMTG", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ======================================================
// SUPABASE
// ======================================================

builder.Services.AddSingleton<SupabaseService>();

// ======================================================
// DATABASE
// ======================================================

builder.Services.AddScoped<DatabaseService>();

// ======================================================
// JWT
// ======================================================

builder.Services.AddScoped<JwtService>();

// ======================================================
// AUTH
// ======================================================

builder.Services.AddScoped<AuthService>();

// ======================================================
// BUILD
// ======================================================

var app = builder.Build();

// ======================================================
// SWAGGER
// ======================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ======================================================
// CORS
// ======================================================

app.UseCors("SMTG");

// ======================================================
// STATIC FILES
// ======================================================

// Permet de servir les fichiers présents dans wwwroot
// Exemple:
// http://10.50.31.88:5072/login.html
app.UseDefaultFiles();
app.UseStaticFiles();

// ======================================================
// AUTHORIZATION
// ======================================================

app.UseAuthorization();

// ======================================================
// CONTROLLERS / API
// ======================================================

app.MapControllers();

// ======================================================
// START SERVER
// ======================================================

app.Run();