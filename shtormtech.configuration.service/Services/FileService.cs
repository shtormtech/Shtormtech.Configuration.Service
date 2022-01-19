using Microsoft.Extensions.Logging;

using shtormtech.configuration.git;

using System;
using System.Threading.Tasks;

namespace shtormtech.configuration.service.Services
{
    public class FileService : IFileService
    {

        private const string _defaultBranch = "master";
        private readonly ICommands Commands;
        private readonly ILogger<FileService> Logger;
        public FileService(ILogger<FileService> logger, ICommands commands)
        {
            Logger = logger ?? throw new ArgumentException(nameof(logger)); ;
            Commands = commands ?? throw new ArgumentException(nameof(commands));
        }

        public async Task<string> GetFileAsync(string fileName, string branch = _defaultBranch)
        {
            return await Commands.GetFileAsync(fileName, branch ?? _defaultBranch);
        }
    }
}
