using shtormtech.configuration.common.Enums;

using System.Threading.Tasks;

namespace shtormtech.configuration.git
{
    public interface ICommands
    {
        Task<string> CloneRepositoryAsync();
        Task<MergeStatus> PullRepositoryAsync();
        string CloneRepository();
        MergeStatus PullRepository();
        Task<string> GetFileAsync(string fileName, string branch = "master");
    }
}
