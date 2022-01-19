using System.Threading.Tasks;

namespace shtormtech.configuration.git
{
    public interface ICommands
    {
        Task CloneRepositoryAsync(string repoFolder, string repoUri, string user = "", string password = "");
        Task<string> GetFileAsync(string fileName, string branch = "master");
    }
}
