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
        const string _repositoryFolder = "repo";
        //const string _repositoryFolder = @"D:\develop\uralSbyt\cec.projects.configs";

        public async Task CloneRepositoryAsync(string repoFolder, string repoUri, string user = "", string password = "")
        {
            var byteArray = Encoding.ASCII.GetBytes(":" + password);
            var encodedToken = Convert.ToBase64String(byteArray);

            var co = new CloneOptions();
            co.FetchOptions = new FetchOptions
            {
                CustomHeaders = new[]
                {
                    $"Authorization: Basic {encodedToken}"
                }
            };
            Repository.Clone(repoUri, _repositoryFolder, co);            
        }

        public async Task<string> GetFileAsync(string fileName, string branch)
        {
            string commitContent;

            using (var repo = new Repository(_repositoryFolder))
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
