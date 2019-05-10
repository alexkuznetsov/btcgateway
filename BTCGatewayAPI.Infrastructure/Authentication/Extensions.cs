using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BTCGatewayAPI.Common.Authentication
{
    public static class AuthenticationExtensions
    {
        private static readonly string SectionName = "jwt";

        public static void AddJwt(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var section = configuration.GetSection(SectionName);
            var options = configuration.GetOptions<JwtOptions>(SectionName);
            
            services.Configure<JwtOptions>(section);
            services.AddSingleton(options);
            services.AddSingleton<IJwtHandler, JwtHandler>();
            //services.AddSingleton<IUserJwtTokenService, TUserJwtTokenService>();
            //services.AddSingleton<IJwtBuilder, JwtBuilder>();

            services.AddTransient<IAccessTokenService, AccessTokenService>();
            services.AddTransient<AccessTokenValidatorMiddleware>();
            //services.AddTransient<TokenGeneratorMiddleware>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                        ValidIssuer = options.Issuer,
                        ValidAudience = options.ValidAudience,
                        ValidateAudience = options.ValidateAudience,
                        ValidateLifetime = options.ValidateLifetime
                    };
                });
        }

        public static IApplicationBuilder UseAccessTokenValidator(this IApplicationBuilder app/*, string loginPath= "/auth/login"*/)
        {
            app.UseMiddleware<AccessTokenValidatorMiddleware>();
            //app.UseWhen(context => context.Request.Path.StartsWithSegments(loginPath), appBldr =>
            //{
            //    appBldr.UseMiddleware<TokenGeneratorMiddleware>();
            //});

            return app;
        }

        public static long ToTimestamp(this DateTime dateTime)
        {
            var centuryBegin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expectedDate = dateTime.Subtract(new TimeSpan(centuryBegin.Ticks));

            return expectedDate.Ticks / 10000;
        }
    }
}
