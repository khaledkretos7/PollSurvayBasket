
using Microsoft.EntityFrameworkCore;
using PollBasket.Api.Persistence;

namespace PollBasket.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services ,IConfiguration configuration) 
    {

       services.AddControllers();

        var connectioString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("invalidConnections");

        services.AddDbContext<ApplicationDbContext>(options => 
        options.UseSqlServer(connectioString));


        services
             .AddSwaggerServices()
            .AddMapsterconfig()
            .fluentValidations();

        

       services.AddScoped<IPollServices, PollServices>();
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
}
