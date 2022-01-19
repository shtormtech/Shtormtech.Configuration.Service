using System.Threading.Tasks;

namespace shtormtech.configuration.service.Services
{
    public interface IFileService
    {
        Task<string> GetFileAsync(string fileName, string branch = "master");
    }
}
