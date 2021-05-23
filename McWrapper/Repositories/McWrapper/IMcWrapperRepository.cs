using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapperSetting = McWrapper.Models.McWrapper;

namespace McWrapper.Repositories.McWrapper
{
    public interface IMcWrapperRepository
    {
        Task<IEnumerable<McWrapperSetting>> All();
        Task<McWrapperSetting> Add(McWrapperSetting mcWrapperSetting);
        Task<McWrapperSetting> Get(string key);
        Task<McWrapperSetting> Update(McWrapperSetting mcWrapperSetting);
        Task Remove(string key);
    }
}