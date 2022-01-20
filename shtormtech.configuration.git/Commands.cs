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
        private string RepositoryFolder { get; } = "repo";
        // private string RepositoryFolder { get; } = @"D:\develop\uralSbyt\cec.projects.configs";
        private string GitRepoUri {get; }
        private string UserName { get; }
        private string Password { get; }

        public Commands(string gitRepoUri, string userName, string password)
        {
            GitRepoUri = gitRepoUri ?? throw new ArgumentException(nameof(gitRepoUri));
            UserName = userName ?? throw new ArgumentException(nameof(userName));
            Password = password ?? throw new ArgumentException(nameof(password));
        }

        public string CloneRepository()
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
            // $exception	{"'repo' exists and is not an empty directory"}	LibGit2Sharp.NameConflictException
            try
            {
                return Repository.Clone(GitRepoUri, RepositoryFolder, co);
            }
            catch(NameConflictException ex)
            {
                throw new DirectoryNotEmptyException(ex.Message, ex);
            }
        }

        public commonEnums.MergeStatus PullRepository()
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
                return (commonEnums.MergeStatus)LibGit2Sharp.Commands.Pull(repo, signature, options).Status;
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

        public Task<string> CloneRepositoryAsync()
        {
            return Task.Run(() => CloneRepository());
        }

        public Task<commonEnums.MergeStatus> PullRepositoryAsync()
        {
            return Task.Run(() => PullRepository());
        }
    }
}
