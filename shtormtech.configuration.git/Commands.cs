using LibGit2Sharp;

using commonExceptions = shtormtech.configuration.common.Exceptions;
using commonEnums = shtormtech.configuration.common.Enums;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using shtormtech.configuration.common.Exceptions;

namespace shtormtech.configuration.git
{
    public class Commands : ICommands
    {        
        // private string RepositoryFolder { get; } = @"D:\develop\uralSbyt\cec.projects.configs";

        public string CloneRepository(string repoUri, string repoFolder, string username, string password)
        {
            var byteArray = Encoding.ASCII.GetBytes($":{password}");
            var encodedToken = Convert.ToBase64String(byteArray);

            var co = new CloneOptions();
            co.FetchOptions = new FetchOptions
            {
                CustomHeaders = new[]
                {
                    $"Authorization: Basic {encodedToken}"
                }
            };

            try
            {
                return Repository.Clone(repoUri, repoFolder, co);
            }
            catch(NameConflictException ex)
            {
                throw new DirectoryNotEmptyException(ex.Message, ex);
            }
        }

        public commonEnums.MergeStatus PullRepository(string repoFolder, string username, string password)
        {
            var byteArray = Encoding.ASCII.GetBytes($":{password}");
            var encodedToken = Convert.ToBase64String(byteArray);
            if (!Repository.IsValid(repoFolder))
            {
                throw new NotValidGitRepoException($"Repository folder \"RepositoryFolder\" is not valig Git repository");
            }
            using (var repo = new Repository(repoFolder))
            {
                // Credential information to fetch
                PullOptions options = new PullOptions();
                options.FetchOptions = new FetchOptions
                {
                    CustomHeaders = new[]
                        {
                            $"Authorization: Basic {encodedToken}"
                        }
                }; ;

                // User information to create a merge commit
                var signature = new Signature(
                    new Identity("MERGE_USER_NAME", "MERGE_USER_EMAIL"), DateTimeOffset.Now);

                // Pull                
                return (commonEnums.MergeStatus)LibGit2Sharp.Commands.Pull(repo, signature, options).Status;
            }
        }
        
        public async Task<string> GetFileAsync(string repoFolder, string fileName, string branch = "master")
        {
            string commitContent;

            using (var repo = new Repository(repoFolder))
            {
                var repoBbranch = repo.Branches[branch]
                    ?? repo.Branches[$"{repo.Head.RemoteName}/{branch}"]
                    ?? throw new commonExceptions.BranchNotFoundException($"Branch \"{branch}\" not found");
                var file = repoBbranch.Tip[fileName]?.Target as Blob 
                    ?? throw new commonExceptions.FileNotFoundException($"File \"{fileName}\" not found in branch \"{branch}\"");
                
                using (var content = new StreamReader(file.GetContentStream(), Encoding.UTF8))
                {
                    commitContent = content.ReadToEnd();
                }
            }

            return commitContent;;
        }

        public Task<string> CloneRepositoryAsync(string repoUri, string repoFolder, string username, string password)
        {
            return Task.Run(() => CloneRepository(repoUri, repoFolder, username, password));
        }

        public Task<commonEnums.MergeStatus> PullRepositoryAsync(string repoFolder, string username, string password)
        {
            return Task.Run(() => PullRepository(repoFolder, username, password));
        }
    }
}
