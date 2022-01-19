using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using shtormtech.configuration.git;
using shtormtech.configuration.service.Config;

using System;
using System.Threading.Tasks;

namespace shtormtech.configuration.service.Services
{
    public class RepositoryService : IRepositoryService
    {
        private GitConfig GitConfiguration { get; }
        private readonly ICommands Commands;
        private readonly ILogger<RepositoryService> Logger;
        public RepositoryService(ILogger<RepositoryService> logger, ICommands commands, IOptions<BaseConfiguration> baseConfiguration)
        {
            Logger = logger ?? throw new ArgumentException(nameof(logger)); ;
            Commands = commands ?? throw new ArgumentException(nameof(commands));
            GitConfiguration = baseConfiguration.Value.Git ?? throw new InvalidOperationException("ivalid git config");
        }

        public async Task CloneRepositoryAsync(string repoFolder = "repo")
        {
            await Commands.CloneRepositoryAsync(repoFolder, GitConfiguration.Uri, GitConfiguration.User, GitConfiguration.Password);
        }
    }
}
