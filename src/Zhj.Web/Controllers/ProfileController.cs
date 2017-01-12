using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Abp.Web.Mvc.Models;
using MyCompanyName.AbpZeroTemplate.Authorization.Users;
using MyCompanyName.AbpZeroTemplate.IO;
using MyCompanyName.AbpZeroTemplate.Net.MimeTypes;
using MyCompanyName.AbpZeroTemplate.Storage;
using System.Web;

namespace MyCompanyName.AbpZeroTemplate.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : AbpZeroTemplateControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IAppFolders _appFolders;

        public ProfileController(
            UserManager userManager,
            IBinaryObjectManager binaryObjectManager,
            IAppFolders appFolders)
        {
            _userManager = userManager;
            _binaryObjectManager = binaryObjectManager;
            _appFolders = appFolders;
        }

        //[DisableAuditing]
        //public async Task<FileResult> GetProfilePicture()
        //{
        //    var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());
        //    //if (user.ProfilePictureId == null)
        //    //{
        //    //    return GetDefaultProfilePicture();
        //    //}

        //    return await GetProfilePictureById(user.ProfilePictureId.Value);
        //}

        [DisableAuditing]
        public async Task<FileResult> GetProfilePictureById(string id = "")
        {
            if (id.IsNullOrEmpty())
            {
                return GetDefaultProfilePicture();
            }

            return await GetProfilePictureById(Guid.Parse(id));
        }
        [UnitOfWork]
        [DisableAuditing]//不输出日志

        public async virtual Task<JsonResult> MultipleUpload() {
            try {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null) {
                    throw new UserFriendlyException("请选择图片上传");
                }

                var file = Request.Files[0];

                if (file.ContentLength > 307200) //30KB.
                {
                    throw new UserFriendlyException("图片尺寸过大");
                }
                var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                //Files/2/
                var path = ZhjConsts.GlobalFilePath + AbpSession.TenantId ?? "host";
                var bigfile = Guid.NewGuid();
                var bigpath = path + "/" + bigfile + ".jpg";
                ImageHelper.FileSaving(file.InputStream.GetAllBytes(), applicationPath + bigpath,null);
                var biginfo = new FileInfo(bigfile, bigpath);
                //Save new picture
                var bigstore = new BinaryObject(bigfile, bigpath);
             
                await _binaryObjectManager.SaveAsync(bigstore);
               // Return success
                return Json(new { state = 4,guid = biginfo.Guid });
            }

            catch (UserFriendlyException ex) {
                //Return error message
                return Json(new MvcAjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        [UnitOfWork]
        public virtual async Task<JsonResult> ChangeProfilePicture()
        {
            try
            {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                var file = Request.Files[0];

                if (file.ContentLength > 30720) //30KB.
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit"));
                }

                //Get user
                var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());

                //Delete old picture
                //if (user.ProfilePictureId.HasValue)
                //{
                //    await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
                //}
                var fileinfo = SavingImage(file);
                //Save new picture
                var storedFile = new BinaryObject(fileinfo.Guid, fileinfo.Url);
                await _binaryObjectManager.SaveAsync(storedFile);


                //Update new picture on the user
              //  user.ProfilePictureId = storedFile.Id;

                //Return success
                return Json(new MvcAjaxResponse());
            }
            catch (UserFriendlyException ex)
            {
                //Return error message
                return Json(new MvcAjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        private FileInfo SavingImage(HttpPostedFileBase file) {
            var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            //Files/2/
            var path = ZhjConsts.GlobalFilePath + AbpSession.TenantId ?? "host";
            var filename = Guid.NewGuid();
            var bigpath = path + "/" + filename + ".jpg";
            ImageHelper.FileSaving(file.InputStream.GetAllBytes(), applicationPath + bigpath, null);
            return new FileInfo(filename, bigpath);
        }
        public async virtual Task< JsonResult> UploadProfilePicture()
        {
            try
            {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                var file = Request.Files[0];

                if (file.ContentLength > 5242880) //1MB.
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit"));
                }

                //Check file type & format
                var fileImage = Image.FromStream(file.InputStream);
                if (!fileImage.RawFormat.Equals(ImageFormat.Jpeg) && !fileImage.RawFormat.Equals(ImageFormat.Png))
                {
                    throw new ApplicationException("Uploaded file is not an accepted image file !");
                }

                //Delete old temp profile pictures
                AppFileHelper.DeleteFilesInFolderIfExists(_appFolders.TempFileDownloadFolder, "userProfileImage_" + AbpSession.GetUserId());

                var fileinfo = SavingImage(file);
                //Save new picture
                var storedFile = new BinaryObject(fileinfo.Guid, fileinfo.Url);
                await _binaryObjectManager.SaveAsync(storedFile);
                return Json(new { state = 4, imageurl = fileinfo.Url, guid = fileinfo.Guid });

            }
            catch (UserFriendlyException ex)
            {
                return Json(new MvcAjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        private FileResult GetDefaultProfilePicture()
        {
            return File(Server.MapPath("~/Common/Images/default-profile-picture.png"), MimeTypeNames.ImagePng);
        }
        
        [AbpMvcAuthorize]
        [DisableAuditing]
        public async Task<JsonResult> FileUpload() {
            try {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null) {
                    throw new UserFriendlyException("请选择文件再次上传");
                }
                var file = Request.Files[0];

                if (file.ContentLength > 3072000) //3M.
                {
                    throw new UserFriendlyException("文件过大");
                }

                CheckModelState();
                var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                //Files/2/
                var path = ZhjConsts.GlobalFilePath + AbpSession.TenantId ?? "host";
                var filename = Guid.NewGuid();
                var ext = file.FileName.Split('.')[1];
                var savepath =$"/{filename}.{ext}";
            //    return new FileInfo(filename, bigpath);
               // var filePath = Path.Combine(_appFolders.TempFileDownloadFolder, file.FileName);
                if (!Directory.Exists(applicationPath+path)) {
                    Directory.CreateDirectory(applicationPath + path);
                }

              
                file.SaveAs(applicationPath+path+savepath);
                await _binaryObjectManager.SaveAsync(new BinaryObject() { Id = filename, Url = path + savepath });

                //保存成功 并且保存到数据库之后   删除文件
                //   System.IO.File.Delete(path + file.FileName);
                //Return success
                return Json(new { state = 4, guid=filename,url=path+savepath });
            }
            catch (Exception ex) {
                //Return error message
                return Json(new { state = -1 });

            }


        }
        private async Task<FileResult> GetProfilePictureById(Guid profilePictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return GetDefaultProfilePicture();
            }

            return File(file.Url, MimeTypeNames.ImageJpeg);
        }
    }
    public class FileInfo {
        public FileInfo(Guid guid, string url) {
            Guid = guid; Url = url;
        }
        public Guid Guid { get; set; }
        public string Url { get; set; }
    }
}