using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using shtormtech.configuration.common.Exceptions;
using shtormtech.configuration.service.Services;

using System;
using System.Threading.Tasks;

namespace shtormtech.configuration.service.Extensions
{
    public static class InitializationWebHostExtension
    {
        public static IHost Initialize(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var repoSrv = scope.ServiceProvider.GetService<IRepositoryService>();
                
                try
                {
                    repoSrv.CloneRepository();
                }
                catch (DirectoryNotEmptyException e)
                {
                    repoSrv.PullRepository();
                }
            }
            return host;
        }
    }
}
