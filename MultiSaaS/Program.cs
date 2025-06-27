var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Configure JSON serialization options
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Configure CORS for API access if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register the tenant provider
builder.Services.AddSingleton<MultiSaaS.Tenant.ITenantProvider, MultiSaaS.Tenant.TenantProvider>();

// Register repositories

// Register hierarchical model repositories
builder.Services.AddScoped<MultiSaaS.Repositories.Interfaces.IOrganizationRepository, MultiSaaS.Repositories.OrganizationRepository>();
builder.Services.AddScoped<MultiSaaS.Repositories.Interfaces.ICompanyRepository, MultiSaaS.Repositories.CompanyRepository>();
builder.Services.AddScoped<MultiSaaS.Repositories.Interfaces.IBranchRepository, MultiSaaS.Repositories.BranchRepository>();
builder.Services.AddScoped<MultiSaaS.Repositories.Interfaces.IVehicleRepository, MultiSaaS.Repositories.VehicleRepository>();
builder.Services.AddScoped<MultiSaaS.Repositories.Interfaces.IUserRepository, MultiSaaS.Repositories.UserRepository>();
builder.Services.AddScoped<MultiSaaS.Repositories.Interfaces.ISubUserRepository, MultiSaaS.Repositories.SubUserRepository>();

// Register services

// Register hierarchical model services
builder.Services.AddScoped<MultiSaaS.Services.Interfaces.IOrganizationService, MultiSaaS.Services.OrganizationService>();
builder.Services.AddScoped<MultiSaaS.Services.Interfaces.ICompanyService, MultiSaaS.Services.CompanyService>();
builder.Services.AddScoped<MultiSaaS.Services.Interfaces.IBranchService, MultiSaaS.Services.BranchService>();
builder.Services.AddScoped<MultiSaaS.Services.Interfaces.IVehicleService, MultiSaaS.Services.VehicleService>();
builder.Services.AddScoped<MultiSaaS.Services.Interfaces.IUserService, MultiSaaS.Services.UserService>();
builder.Services.AddScoped<MultiSaaS.Services.Interfaces.ISubUserService, MultiSaaS.Services.SubUserService>();

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

// Enable CORS with the API policy
app.UseCors("ApiPolicy");

app.UseAuthorization();

// Map traditional MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map API controllers
app.MapControllers();

app.Run();
