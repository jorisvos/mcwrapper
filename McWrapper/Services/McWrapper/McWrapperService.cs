using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McWrapper.Repositories.McWrapper;

namespace McWrapper.Services.McWrapper
{
    public class McWrapperService : IMcWrapperService
    {
        private readonly IMcWrapperRepository _mcWrapperRepository;

        public McWrapperService(IMcWrapperRepository mcWrapperRepository)
        {
            _mcWrapperRepository = mcWrapperRepository;
        }

        public async Task<IEnumerable<Models.McWrapper>> GetAll() => await _mcWrapperRepository.All();

        public async Task<Models.McWrapper> Add(Models.McWrapper mcWrapper)
        {
            var mcWrapperSettings = await _mcWrapperRepository.All();
            if (mcWrapperSettings.Any(m => m.Key == mcWrapper.Key))
                throw new Exception("You can't have settings with the same key.");
            
            return await _mcWrapperRepository.Add(mcWrapper);
        }
        
        public async Task<Models.McWrapper> Update(Models.McWrapper mcWrapper) => await _mcWrapperRepository.Update(mcWrapper);
        
        public async Task<Models.McWrapper> Get(string key) => await _mcWrapperRepository.Get(key);

        public async Task Remove(string key) => await _mcWrapperRepository.Remove(key);
    }
}