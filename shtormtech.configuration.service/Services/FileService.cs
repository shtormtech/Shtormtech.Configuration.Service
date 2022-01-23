using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using shtormtech.configuration.git;
using shtormtech.configuration.service.Config;

using System;
using System.Threading.Tasks;

namespace shtormtech.configuration.service.Services
{
    public class FileService : IFileService
    {
        private const string defaultBranch = "master";
        private const string repositoryFolder = "repo";
        private readonly ICommands Commands;
        private readonly ILogger<FileService> Logger;
        private GitConfig GitConfiguration { get; }
        public FileService(ILogger<FileService> logger, ICommands commands, IOptions<BaseConfiguration> baseConfiguration)
        {
            Logger = logger ?? throw new ArgumentException(nameof(logger)); ;
            Commands = commands ?? throw new ArgumentException(nameof(commands));
            GitConfiguration = baseConfiguration.Value.Git ?? throw new InvalidOperationException("ivalid git config");
        }

        public async Task<string> GetFileAsync(string fileName, string branch = defaultBranch)
        {
            await Commands.PullRepositoryAsync(repositoryFolder, GitConfiguration.User, GitConfiguration.Password);
            return await Commands.GetFileAsync(repositoryFolder, fileName, branch ?? defaultBranch);
        }
    }
}
