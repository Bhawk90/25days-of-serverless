using ReindeerGuidance.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReindeerGuidance.Core.Repositories
{
    public interface IIncidentRepository
    {
        Task<IncidentEntity> CreateAsync(IncidentPostBindingModel model);

        Task<IList<IncidentEntity>> GetAsync();

        Task<IncidentEntity> GetByIdAsync(string id);

        Task DeleteAsync(string id);

        Task UpdateAsync(string id, IncidentPutBindingModel model);
    }
}
