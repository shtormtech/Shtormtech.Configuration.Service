using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using shtormtech.configuration.common.Enums;
using shtormtech.configuration.git;
using shtormtech.configuration.service.Config;

using System;
using System.Threading.Tasks;

namespace shtormtech.configuration.service.Services
{
    public class RepositoryService : IRepositoryService
    {
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
            return await Commands.CloneRepositoryAsync();
        }

        public async Task<MergeStatus> PullRepositoryAsync()
        {
            return await Commands.PullRepositoryAsync();
        }

        public string CloneRepository()
        {
            return Commands.CloneRepository();
        }

        public MergeStatus PullRepository()
        {
            return Commands.PullRepository();
        }
    }
}
