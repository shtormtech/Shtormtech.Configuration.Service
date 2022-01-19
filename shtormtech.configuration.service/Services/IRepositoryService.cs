using System.Threading.Tasks;

namespace shtormtech.configuration.service.Services
{
    public interface IRepositoryService
    {
        Task CloneRepositoryAsync(string repoUri, string user = "", string password = "");
    }
}
