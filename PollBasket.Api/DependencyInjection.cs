using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PollBasket.Api.Authentication;
using PollBasket.Api.Presistence;
using System.Text;

namespace PollBasket.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services ,IConfiguration configuration) 
    {

       services.AddControllers();

        services.AddAuthConfig(configuration);

        var connectioString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("invalidConnections");

        services.AddDbContext<ApplicationDbContext>(options => 
        options.UseSqlServer(connectioString));


        services
             .AddSwaggerServices()
            .AddMapsterconfig()
            .fluentValidations();
       




       services.AddScoped<IPollServices, PollServices>();
       services.AddScoped<IAuthServices, AuthServices>();

        return services;
    }
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {     
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
    
        return services;
    }
    public static IServiceCollection AddMapsterconfig(this IServiceCollection services)
    {
        var mappingconfig = TypeAdapterConfig.GlobalSettings;
        mappingconfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingconfig));

        return services;
    }
    public static IServiceCollection fluentValidations(this IServiceCollection services)
    {
        //add fluentvalidation 
        services
             .AddFluentValidationAutoValidation()
             .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
    public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtProvider, JwtProvider>();
        //map config from setting to ioptions
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));  
        //عشان يعرف يقراها تحت 
        var jwtSetting=configuration.GetSection("JwtOptions").Get<JwtOptions>();

        services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key)),
                ValidIssuer = jwtSetting.Issuer,
                ValidAudience = jwtSetting.Audience,
            };
        });

        return services;
    }
}
