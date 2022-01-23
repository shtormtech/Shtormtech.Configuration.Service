using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using shtormtech.configuration.common.Enums;
using shtormtech.configuration.common.Exceptions;
using shtormtech.configuration.common.Extensions;
using shtormtech.configuration.git;
using shtormtech.configuration.service.Config;

using System;
using System.IO;
using System.Threading.Tasks;

namespace shtormtech.configuration.service.Services
{
    public class RepositoryService : IRepositoryService
    {
        private string RepositoryFolder { get; } = "repo";
        private GitConfig GitConfiguration { get; }
        private ICommands Commands { get; }
        private ILogger<RepositoryService> Logger { get; }
            
        public RepositoryService(ILogger<RepositoryService> logger, ICommands commands, IOptions<BaseConfiguration> baseConfiguration)
        {
            Logger = logger ?? throw new ArgumentException(nameof(logger)); ;
            Commands = commands ?? throw new ArgumentException(nameof(commands));
            GitConfiguration = baseConfiguration.Value.Git ?? throw new InvalidOperationException("ivalid git config");
        }

        public async Task<string> CloneRepositoryAsync()
        {
            return await Commands.CloneRepositoryAsync(GitConfiguration.Uri, RepositoryFolder, GitConfiguration.User, GitConfiguration.Password);
        }

        public async Task<MergeStatus> PullRepositoryAsync()
        {
            try
            {
                return await Commands.PullRepositoryAsync(RepositoryFolder, GitConfiguration.User, GitConfiguration.Password);
            }
            catch(NotValidGitRepoException)
            {
                Logger.LogWarning($"Failed pull repository. Try ReClone repository");
                new DirectoryInfo(RepositoryFolder).RecursiveDelete();
                await Commands.CloneRepositoryAsync(GitConfiguration.Uri, RepositoryFolder, GitConfiguration.User, GitConfiguration.Password);
                return MergeStatus.UpToDate;
            }
        }

        public string CloneRepository()
        {
            return Commands.CloneRepository(GitConfiguration.Uri, RepositoryFolder, GitConfiguration.User, GitConfiguration.Password);
        }

        public MergeStatus PullRepository()
        {
            return Commands.PullRepository(RepositoryFolder, GitConfiguration.User, GitConfiguration.Password);
        }
    }
}
