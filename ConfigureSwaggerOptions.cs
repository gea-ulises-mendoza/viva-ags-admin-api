using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VivaAguascalientesAPI
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    /// <see cref="https://tim.covatrix.com/posts/api-ver/"/>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT"
            });


            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Atractivos API",
                Version = description.ApiVersion.ToString(),
                Description = "Servicios web para administrador de atractivos.",
                Contact = new OpenApiContact() { Name = "Marco Navarro", Email = "marco.navarro@aguascalientes.gob.mx" },
                License = new OpenApiLicense() { Name = "SAE", Url = new System.Uri("http://aguascalientes.gob.mx/SAE") },
                
            };

            if (description.IsDeprecated)
            {
                info.Description += "\nVersion obsoleta.";
            }

            return info;
        }
    }

    //public class SwaggerDocumentFilter : IDocumentFilter
    //{
    //    private readonly string _host;

    //    public SwaggerDocumentFilter(IHttpContextAccessor httpContext)
    //    {
    //        var scheme = httpContext.HttpContext.Request.Scheme;
    //        var host = httpContext.HttpContext.Request.Host;
    //        var pathBase = httpContext.HttpContext.Request.PathBase;
    //        _host = $"{scheme}://{host}{pathBase}";
    //    }

    //    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    //    {
    //        swaggerDoc.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = _host} };
    //    }
    //}
}