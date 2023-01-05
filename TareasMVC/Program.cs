using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TareasMVC;
using TareasMVC.Data;
using TareasMVC.Servicios;

var builder = WebApplication.CreateBuilder(args);

//Aqui genrio la politica para los usuaarios AUtenticados para luego pasarsela a MVC Core.
var politicaUsuarioAutenticados = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

// Add services to the container.
builder.Services.AddControllersWithViews(opciones =>
{
    //aqui le pesao la politica de autenticación a MVC Core.
    opciones.Filters.Add(new AuthorizeFilter(politicaUsuarioAutenticados));
}).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
.AddDataAnnotationsLocalization(opciones =>
{
    opciones.DataAnnotationLocalizerProvider = (_, factorial) => factorial.Create(typeof(RecursoCompartido));
});



builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddAuthentication();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
{
    opciones.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//Aqui configuro trabajar con mis propias vistas y no con las del Entity.
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>
{
    opciones.LogoutPath = "/usuarios/login";
    opciones.AccessDeniedPath = "/usuarios/login/";
});


//Para multiple idiomas
builder.Services.AddLocalization(opciones =>
{
    opciones.ResourcesPath = "Recursos";
});

builder.Services.AddTransient<IServiciosUsuarios, ServiciosUsuarios>();
//Aqui adicciono automapper al proyecto del asembly TareasMVC(program)
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();


//Culturas soportadas:
//var culturasUISoportadas = new[] { "en", "es" };

app.UseRequestLocalization(opciones =>
{
    opciones.DefaultRequestCulture = new RequestCulture("en");
    opciones.SupportedCultures = Constantes.CulturasUISoportadas.Select(cultura => new CultureInfo(cultura.Value)).ToList();
});

//app.UseRequestLocalization(opciones =>
//{
//    opciones.DefaultRequestCulture = new RequestCulture("en");
//    opciones.SupportedCultures = culturasUISoportadas.Select(cultura => new CultureInfo(cultura)).ToList();
//});

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
    pattern: "{controller=Usuarios}/{action=Login}/{id?}");

app.Run();
