namespace Postex.Payment.Api.Swagger;

public static class SwaggerMiddlerwareExtension
{
    public static void UseCustomSwagger(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var provider = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger().UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            });

            app.UseReDoc(options =>
            {
                options.DocumentTitle = "Postex APIs";
                options.SpecUrl = "/swagger/v1/swagger.json";
            });
        }
    }

    public static void AddCustomVersioningSwagger(this IServiceCollection services, int majorVersion = 1, int minorVersion = 0)
    {
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(majorVersion, minorVersion);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                                    new HeaderApiVersionReader("x-api-version"),
                                                                                    new MediaTypeApiVersionReader("x-api-version"));
        });

        services.AddCustomSwagger();
    }

    public static void AddCustomSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();
            options.OperationFilter<SwaggerCustomeHeaders>();
            options.DocumentFilter<CustomSwaggerFilter>();
            options.SchemaFilter<AddReadOnlyPropertiesFilter>();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        //services.AddFluentValidationRulesToSwagger();
    }
}
