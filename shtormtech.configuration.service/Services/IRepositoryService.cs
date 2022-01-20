using shtormtech.configuration.common.Enums;

using System.Threading.Tasks;

namespace shtormtech.configuration.service.Services
{
    public interface IRepositoryService
    {
        Task<string> CloneRepositoryAsync();
        Task<MergeStatus> PullRepositoryAsync();
        string CloneRepository();
        MergeStatus PullRepository();
    }
}
