using System.Threading.Tasks;

namespace shtormtech.configuration.git
{
    public interface ICommands
    {
        Task CloneRepositoryAsync(string repoFolder);
        Task PullRepositoryAsync();
        Task<string> GetFileAsync(string fileName, string branch = "master");
    }
}
