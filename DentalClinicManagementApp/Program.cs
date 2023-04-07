using DentalClinicManagementApp;
using DentalClinicManagementApp.Data;
using DentalClinicManagementApp.Data.Seed;
using DentalClinicManagementApp.Lib;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#region Resources - Language

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


const string defaultCulture = "pt";

var ptCI = new CultureInfo(defaultCulture);

var supportedCultures = new[]
{
    ptCI,
    new CultureInfo("en")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(defaultCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services
    .AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    })
    .AddNToastNotifyToastr(new NToastNotify.ToastrOptions //NToastNotify - notifica��es
    {
        ProgressBar = true,
        TimeOut = 3000
    });
#endregion


#region Login
//IdentityUser: definição por omissão do ASP.net
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    //Confirm signin account
    options.SignIn.RequireConfirmedAccount = true;

    //Password
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 4;
})
    .AddRoles<IdentityRole>() //Roles
    .AddEntityFrameworkStores<ApplicationDbContext>(); //onde guarda informações

//Apply policies a group of users
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppConstants.APP_POLICY, policy => policy.RequireRole(AppConstants.APP_POLICY_ROLES));
    options.AddPolicy(AppConstants.APP_ADMIN_POLICY, policy => policy.RequireRole(AppConstants.AAPP_ADMIN_POLICY_ROLES));
});
#endregion


builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//For�ar a utilizar localiza��o definida por n�s
app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value
);

//NToastNotify
app.UseNToastNotify();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

Seed();

app.Run();


#region SeedDatabaseWithUsers
//Fill a default user admin and worker
void Seed()
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;


    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        SeedDatabase.Seed(dbContext, userManager, roleManager);
    }
    catch (Exception ex)
    {

    }
}
#endregion