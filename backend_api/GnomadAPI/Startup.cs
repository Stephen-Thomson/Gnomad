/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Sets up the REST API, Swagger, and google authentication.
*
************************************************************************************************/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using System.IO;
using System.Reflection;
using TravelCompanionAPI.Data;
using TravelCompanionAPI.Models;

namespace TravelCompanionAPI
{
    public class Startup
    {
        private const string CorsPolicyName = "AppPolicy";

        public Startup(IConfiguration configuration)
        {
            // Set the configuration
            Configuration = configuration;

            // Get the connection string from the configuration
            // and set the DatabaseConnection singleton to use it.
#if DEBUG
            string connection_string = configuration.GetConnectionString("TestingDatabase");
#else
            string connection_string = configuration.GetConnectionString("CodenomeDatabase");
#endif
            DatabaseConnection.getInstance().setConnectionString(connection_string);
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(
                CorsPolicyName,
                builder => builder
                .WithOrigins("http://localhost:5000", 
                "https://*.google.com", 
                "https://*.googleusercontent.com", 
                "https://travel.bryceschultz.com:5001", 
                "https://travel.bryceschultz.com")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(origin =>
                {
                    return true;
                })
            ));

            services.AddControllers();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(o =>
            {
                o.IncludeErrorDetails = true;
                o.SecurityTokenValidators.Clear();
                o.SecurityTokenValidators.Add(new GoogleTokenValidator());
            });

            services.AddAuthorization();

            services.AddSwaggerGen(c =>
            {
                OpenApiInfo info = new OpenApiInfo { Title = "TravelCompanionAPI", Version = "v1" };
                c.SwaggerDoc("v1", info);

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"https://accounts.google.com/o/oauth2/auth"),
                            TokenUrl = new Uri($"https://oauth2.googleapis.com/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {
                                    "https://www.googleapis.com/auth/userinfo.email",
                                    "Email"
                                },
                                {
                                    "https://www.googleapis.com/auth/userinfo.profile",
                                    "Profile"
                                }
                            }
                        }
                    },
                    Extensions = new Dictionary<string, IOpenApiExtension>
                    {
                        {"x-tokenName", new OpenApiString("id_token")}
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "oauth2", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            //Adds dependency injection so that UserRepository gets called wherever IUserRepository gets called
            services.AddTransient<IUserRepository, UserRepository>(); //IMPORTANT: If changed, update user model since it can't do dependency injection :(
            //Adds dependency injection so that PinTagRepository gets called wherever IPinTagRepository gets called
            services.AddTransient<IPinTagRepository, PinTagRepository>();
            //Adds dependency injection so that PinRepository gets called wherever IPinRepository gets called
            services.AddTransient<IPinRepository, PinRepository>();
            //TODO: implement
            //Adds dependency injection so that StickerRepository gets called wherever IStickerRepository gets called
            //services.AddTransient<IPinRepository, StickerRepository>();
            //Adds dependency injection so that TagRepository gets called wherever ITagRepository gets called
            services.AddTransient<ITagRepository, TagRepository>();
            //Adds dependency injection so that CellularRepository gets called wherever ICellRepository gets called
            services.AddTransient<ICellularRepository, CellularRepository>();
            //Adds dependency injection so that RouteRepository gets called wherever IRouteRepository gets called
            services.AddTransient<IRouteRepository, RouteRepository>();
            //Adds a singleton to UserRepository
            services.AddSingleton<UserRepository>();
            //Adds a singleton to PinRepository
            services.AddSingleton<PinRepository>();
            //Adds a singleton to TagRepository
            services.AddSingleton<TagRepository>();
            //Adds a singleton to CellularRepository
            services.AddSingleton<CellularRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!Debugger.IsAttached)
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();

            app.UseRouting();

            app.UseCors(CorsPolicyName);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelCompanionAPI v1");
                    c.OAuthConfigObject.ClientId = Configuration["Authentication:Google:client_id"];
                    c.OAuthConfigObject.ClientSecret = Configuration["Authentication:Google:client_secret"];
                });
            }
        }
    }
}
