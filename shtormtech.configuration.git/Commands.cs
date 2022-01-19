using LibGit2Sharp;

using commonExceptions = shtormtech.configuration.common.Exceptions;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace shtormtech.configuration.git
{
    public class Commands : ICommands
    {
        // const string _repositoryFolder = "repo";
        private string RepositoryFolder { get; } = @"D:\develop\uralSbyt\cec.projects.configs";
        private string GitRepoUri {get; }
        private string UserName { get; }
        private string Password { get; }

        public Commands(string gitRepoUri, string userName, string password)
        {
            GitRepoUri = gitRepoUri ?? throw new ArgumentException(nameof(gitRepoUri));
            UserName = userName ?? throw new ArgumentException(nameof(userName));
            Password = password ?? throw new ArgumentException(nameof(password));
        }

        public async Task CloneRepositoryAsync(string repoFolder)
        {
            var byteArray = Encoding.ASCII.GetBytes(":" + Password);
            var encodedToken = Convert.ToBase64String(byteArray);

            var co = new CloneOptions();
            co.FetchOptions = new FetchOptions
            {
                CustomHeaders = new[]
                {
                    $"Authorization: Basic {encodedToken}"
                }
            };
            Repository.Clone(GitRepoUri, RepositoryFolder, co);            
        }

        public async Task PullRepositoryAsync()
        {
            var byteArray = Encoding.ASCII.GetBytes(":" + Password);
            var encodedToken = Convert.ToBase64String(byteArray);
            using (var repo = new Repository(RepositoryFolder))
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
                LibGit2Sharp.Commands.Pull(repo, signature, options);
            }
        }
        
        public async Task<string> GetFileAsync(string fileName, string branch)
        {
            string commitContent;

            using (var repo = new Repository(RepositoryFolder))
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
    }
}
