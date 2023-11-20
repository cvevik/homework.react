using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUrl = builder.Configuration["KeyVaultConfiguration:KeyVaultURL"]!;
var keyVaultClientId = builder.Configuration["KeyVaultConfiguration:ClientId"]!;
var keyVaultClientSecret = builder.Configuration["KeyVaultConfiguration:ClientSecret"]!;

builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl),
        new ClientSecretCredential(
            "98f64ac9-ff7e-42f1-afef-06a12247cb4a",
            keyVaultClientId,
            keyVaultClientSecret ));
        //keyVaultClientId,
        //keyVaultClientSecret);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAndValidate<OmdbClientOptions>(options =>
        builder.Configuration
            .GetRequiredSection(OmdbClientOptions.SectionName)
            .Bind(options));
builder.Services.AddScoped<OmdbClient>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();