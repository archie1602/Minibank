var builder = WebApplication.CreateBuilder(args);

// Add services to the container

var services = builder.Services;

services.AddControllers();
services.AddHostedService<MigrationHostedService>();
services
    .AddData(builder.Configuration)
    .AddCore();

services.AddAutoMapper(typeof(MinibankWebMapper));

services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Audience = "api";
        options.Authority = "https://demo.duendesoftware.com";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            // disable this option, as our middleware: CustomAuthenticationMiddleware enabled
            ValidateLifetime = false,
            ValidateAudience = false
        };
    });

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("MinibankOpenAPISpec",
                            new OpenApiInfo()
                            {
                                Title = "Minibank API",
                                Version = "1"
                            });

                            var xmlCommentFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                            var xmlCmtsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);

                            options.IncludeXmlComments(xmlCmtsFullPath);

    options.AddSecurityDefinition("oauth2",
        new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows()
            {
                ClientCredentials = new OpenApiOAuthFlow()
                {
                    TokenUrl = new Uri("https://demo.duendesoftware.com/connect/token"),
                    Scopes = new Dictionary<string, string>()
                }
            }
        });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = SecuritySchemeType.OAuth2.GetDisplayName()
                }
            },
            new List<string>()
        }
    });
});

// Configure the HTTP request pipeline
var app = builder.Build();

app.UseMiddleware<CustomAuthenticationMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/MinibankOpenAPISpec/swagger.json", "Minibank API");
    options.RoutePrefix = "";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();