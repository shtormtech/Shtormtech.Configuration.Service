using System.Threading.Tasks;

namespace shtormtech.configuration.service.Services
{
    public interface IRepositoryService
    {
        Task CloneRepositoryAsync(string repoFolder);
    }
}
