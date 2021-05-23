using AutoMapper;
using McWrapper.Models;

namespace McWrapper.Config.Mapping
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<JarDto, Jar>();
            CreateMap<Jar, JarDto>();

            CreateMap<PluginDto, Plugin>();
            CreateMap<Plugin, PluginDto>();
            
            CreateMap<ServerDto, Server>();
            CreateMap<Server, ServerDto>();
            
            CreateMap<PluginServerDto, PluginServer>();
            CreateMap<PluginServer, PluginServerDto>();

            CreateMap<McWrapperLib.Server, Server>();
            CreateMap<Server, McWrapperLib.Server>();

            CreateMap<McWrapperDto, Models.McWrapper>();
            CreateMap<Models.McWrapper, McWrapperDto>();
        }
    }
}