using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using shtormtech.configuration.service.Services;

using System;
using System.Threading.Tasks;

namespace shtormtech.configuration.service.Controllers
{
    [ApiController]
    public class FileController : Controller
    {        
        private readonly ILogger<FileController> Logger;
        private readonly IFileService FileService;

        public FileController(ILogger<FileController> logger, IFileService fileService)
        {
            Logger = logger ?? throw new ArgumentException(nameof(logger)); ;
            FileService = fileService ?? throw new ArgumentException(nameof(fileService)); ;
        }

        [HttpGet("/files/{**path}")]
        public async Task<string> Path([FromRoute] string path, [FromQuery] string branch)
        {
            return await FileService.GetFileAsync(path, branch);
        }
    }
}
