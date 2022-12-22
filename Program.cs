using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UltraTix2022.API.SignalR;
using UltraTix2022.API.UltraTix2022.Business.Services.ArtistRequestServices;
using UltraTix2022.API.UltraTix2022.Business.Services.ArtistService;
using UltraTix2022.API.UltraTix2022.Business.Services.EmailService;
using UltraTix2022.API.UltraTix2022.Business.Services.FeedbackServices;
using UltraTix2022.API.UltraTix2022.Business.Services.OrganizerService;
using UltraTix2022.API.UltraTix2022.Business.Services.PaymentService;
using UltraTix2022.API.UltraTix2022.Business.Services.PostService;
using UltraTix2022.API.UltraTix2022.Business.Services.SecretServices;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowCategoryService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowOrderService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowRequestService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowStaffService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowTransactionService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowTypeService;
using UltraTix2022.API.UltraTix2022.Business.Services.SystemAdminService;
using UltraTix2022.API.UltraTix2022.Business.Services.UserService;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.AppTransactionRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistFollowerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistRequestRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CustomerPurchaseTransactionRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CustomerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CustomerWalletRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.FeedbackRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.FeedbackTypeRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.LocationRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.MoMoRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.OrganizerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostCommentRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostFollowerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowCategoryRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRequestRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowReviewRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowStaffRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTransactionHistoryRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTypeRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.StaffShowDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SystemAdminRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.TickTypeRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Add Db Context
builder.Services.AddDbContext<UltraTixDBContext>(options => options.UseSqlServer(KeyVaultServices.GetConnectionString()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "UltraTix.API",
        Description = "APIs for UltraTix"
    });

    var securityScheme = new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme. " +
                        "\n\nEnter 'Bearer' [space] and then your token in the text input below. " +
                          "\n\nExample: 'Bearer 12345abcde'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference()
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        securityScheme,
                        new string[]{ }
                    }
                });

});

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("ultratix-api-version"),
                                                    new MediaTypeApiVersionReader("ultratix-api-version"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["Jwt:Firebase:ValidIssuer"];
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Firebase:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:Firebase:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Firebase:PrivateKey"]))
    };
});
// Add Scope

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISystemAdminService, SystemAdminService>();
builder.Services.AddScoped<IOrganizerService, OrganizerService>();
builder.Services.AddScoped<IShowStaffService, ShowStaffService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IShowTypeService, ShowTypeService>();
builder.Services.AddScoped<IShowService, ShowService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IShowRequestService, ShowRequestService>();
builder.Services.AddScoped<IShowCategoryService, ShowCategoryService>();
builder.Services.AddScoped<IMoMoService, MoMoServices>();
builder.Services.AddScoped<IShowOrderService, ShowOrderService>();
builder.Services.AddScoped<IShowTransactionService, ShowTransactionService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFeedbackService, FeedbackServices>();
builder.Services.AddScoped<IArtistRequestService, ArtistRequestService>();


// Add Transient
builder.Services.AddTransient<IPostRepo, PostRepo>();
builder.Services.AddTransient<IMoMoRepo, MoMoRepo>();
builder.Services.AddTransient<IFeedbackRepo, FeedbackRepo>();
builder.Services.AddTransient<IFeedbackTypeRepo, FeedbackTypeRepo>();
builder.Services.AddTransient<IPostCommentRepo, PostCommentRepo>();
builder.Services.AddTransient<IPostFollowerRepo, PostFollowerRepo>();
builder.Services.AddTransient<IArtistFollowerRepo, ArtistFollowerRepo>();
builder.Services.AddTransient<IArtistRequestRepo, ArtistRequestRepo>();

//--App User
builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddTransient<ISystemAdminRepo, SystemAdminRepo>();
builder.Services.AddTransient<IOrganizerRepo, OrganizerRepo>();
builder.Services.AddTransient<IShowStaffRepo, ShowStaffRepo>();
builder.Services.AddTransient<IArtistRepo, ArtistRepo>();
builder.Services.AddTransient<ICustomerRepo, CustomerRepo>();



//--Show
builder.Services.AddTransient<IShowTypeRepo, ShowTypeRepo>();
builder.Services.AddTransient<IShowRepo, ShowRepo>();
builder.Services.AddTransient<ITicketTypeRepo, TicketTypeRepo>();
builder.Services.AddTransient<ILocationRepo, LocationRepo>();
builder.Services.AddTransient<ISaleStageRepo, SaleStageRepo>();
builder.Services.AddTransient<ICampaignRepo, CampaignRepo>();
builder.Services.AddTransient<ICampaignDetailRepo, CampaignDetailRepo>();
builder.Services.AddTransient<IStaffShowDetailRepo, StaffShowDetailRepo>();
builder.Services.AddTransient<IShowRequestRepo, ShowRequestRepo>();
builder.Services.AddTransient<IShowCategoryRepo, ShowCategoryRepo>();
builder.Services.AddTransient<ISaleStageDetailRepo, SaleStageDetailRepo>();
builder.Services.AddTransient<IShowReviewRepo, ShowReviewRepo>();

//--Show Order
builder.Services.AddTransient<IShowOrderRepo, ShowOrderRepo>();
builder.Services.AddTransient<IShowOrderDetailRepo, ShowOrderDetailRepo>();

//--Transaction
builder.Services.AddTransient<IShowTransactionHistoryRepo, ShowTransactionHistoryRepo>();
builder.Services.AddTransient<IAppTransactionRepo, AppTransactionRepo>();
builder.Services.AddTransient<ICustomerPurchaseTransactionRepo, CustomerPurchaseTransactionRepo>();
builder.Services.AddTransient<ICustomerWalletRepo, CustomerWalletRepo>();
// Add CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        b => b.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .WithExposedHeaders(new string[] { "Authorization", "authorization" }));
});

// Add SignalR

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}


app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.MapHub<NotifyHub>("/notifyhub");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
