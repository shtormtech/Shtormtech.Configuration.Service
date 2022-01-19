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
        private GitConfig Configuration { get; }
        private readonly ICommands Commands;
        private readonly ILogger<RepositoryService> Logger;
        public RepositoryService(ILogger<RepositoryService> logger, ICommands commands, IOptions<BaseConfiguration> baseConfiguration)
        {
            Logger = logger ?? throw new ArgumentException(nameof(logger)); ;
            Commands = commands ?? throw new ArgumentException(nameof(commands));
            Configuration = baseConfiguration.Value.Git ?? throw new InvalidOperationException("ivalid git config");
        }

        public async Task CloneRepositoryAsync(string repoUri, string user = "", string password = "")
        {
            await Commands.CloneRepositoryAsync(repoUri, user, password);
        }
    }
}
