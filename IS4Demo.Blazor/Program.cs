using IS4Demo.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

#region IdentityServer4 Config

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

})
    .AddCookie("Cookies", options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToReturnUrl = context =>
        {
            return Task.CompletedTask;
        };
    })
    .AddOpenIdConnect(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role",
            ValidateIssuer = false,
            ValidateAudience = false
        };

        options.Authority = "https://localhost:5001"; //Add IS4Demo.Auth Uri here

        options.ClientId = "YourWebsiteIdentifierHere";
        options.ClientSecret = "YourWebsiteIdentifierHere";
        options.ResponseType = "code";

        options.SignInScheme = "Cookies";

        options.RequireHttpsMetadata = true;

        options.UsePkce = true;

        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("YourAppScopeHere");

        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;

        options.Events.OnRemoteFailure = context =>
        {
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

#endregion


// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
});

builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub()
    .RequireAuthorization();

app.MapFallbackToPage("/_Host");

app.Run();
