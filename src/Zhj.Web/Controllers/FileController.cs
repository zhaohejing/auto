using System.IO;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Authorization;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using MyCompanyName.AbpZeroTemplate.Dto;
using System;
using MyCompanyName.AbpZeroTemplate.Storage;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Web.Controllers
{
    public class FileController : AbpZeroTemplateControllerBase
    {
        private readonly IAppFolders _appFolders;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public FileController(IAppFolders appFolders, IBinaryObjectManager binaryObjectManager)
        {
            _appFolders = appFolders;
            _binaryObjectManager = binaryObjectManager;
        }

        [AbpMvcAuthorize]
        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            CheckModelState();

            var filePath = Path.Combine(_appFolders.TempFileDownloadFolder, file.FileToken);
            if (!System.IO.File.Exists(filePath))
            {
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);
            return File(fileBytes, file.FileType, file.FileName);
        }
        [AbpMvcAuthorize]
        [DisableAuditing]
        public async Task<ActionResult> DownloadFile(Guid file) {
            CheckModelState();
            var model = await _binaryObjectManager.GetOrNullAsync(file);
            if (model==null) {
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));
            }
           
            var filePath = AppDomain.CurrentDomain.BaseDirectory+ model.Url;
            if (!System.IO.File.Exists(filePath)) {
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream","附件文件");
        }
    }
}