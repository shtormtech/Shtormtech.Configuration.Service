using shtormtech.configuration.common.Enums;

using System.Threading.Tasks;

namespace shtormtech.configuration.git
{
    public interface ICommands
    {
        Task<string> CloneRepositoryAsync(string repoUri, string repoFolder, string username, string password);
        Task<MergeStatus> PullRepositoryAsync(string repoFolder, string username, string password);
        string CloneRepository(string repoUri, string repoFolder, string username, string password);
        MergeStatus PullRepository(string repoFolder, string username, string password);
        Task<string> GetFileAsync(string repoFolder, string fileName, string branch = "master");
    }
}
