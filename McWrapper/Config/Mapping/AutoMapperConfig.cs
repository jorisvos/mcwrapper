using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace McWrapper.Config.Mapping
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            return services.AddSingleton(CreateMapper());
        }

        private static IMapper CreateMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DataMappingProfile>();
            }).CreateMapper();
        }
    }
}