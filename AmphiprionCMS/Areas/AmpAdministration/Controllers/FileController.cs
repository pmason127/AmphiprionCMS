using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using AmphiprionCMS.Components;
using AmphiprionCMS.Models;
using Microsoft.Ajax.Utilities;

namespace AmphiprionCMS.Areas.AmpAdministration.Controllers
{
    public class FileController : Controller
    {
        //
        // GET: /File/
        private IImagingService _imaging;
        public FileController(IImagingService imaging)
        {
            _imaging = imaging;
        }
        public ActionResult Browser()
        {
            var path = this.HttpContext.Request.QueryString["p"];
            if (string.IsNullOrEmpty(path))
                path = "~/media";
            else
            {
                path = HttpUtility.UrlDecode(path);
            }

            var filter = this.HttpContext.Request.QueryString["t"];
            if (string.IsNullOrEmpty(filter))
                filter = "all";
            else
            {
                filter = HttpUtility.UrlDecode(filter);
            }


            FileBrowserModel model = new FileBrowserModel();
            model.Filter = filter;
            model.CurrentPath = path;
            var directory = new DirectoryInfo(Server.MapPath(path));
            foreach (var dir in directory.GetDirectories())
            {

                Folder folder = new Folder();
                folder.Name = dir.Name;
                folder.HasFiles = dir.GetFiles().Any();
                model.Folders.Add(folder);
            }

            foreach (var fileInfo in directory.GetFiles())
            {
                bool isImg = _imaging.IsImage(fileInfo.Extension);
                if ((filter.ToLower() == "images" && !isImg ) || fileInfo.Extension.ToLower() == ".config")
                    continue;

                AmphiprionCMS.Models.File file = new AmphiprionCMS.Models.File();
                file.Name = fileInfo.Name;
                file.Extension = file.Extension;
                file.IsImage = isImg;
                model.Files.Add(file);
            }
            if (Request.IsAjaxRequest())
                return PartialView("_files", model);

            return View(model);
        }

        public ActionResult View()
        {
            var path = this.HttpContext.Request.QueryString["p"];
            var decodedPath = HttpUtility.UrlDecode(path);

            var ext = Path.GetExtension(decodedPath);
            var ct = _imaging.GetMimeType(ext);
            if (_imaging.IsImage(ext))
            {
                var dim = this.HttpContext.Request.QueryString["d"];
                if (!string.IsNullOrEmpty(dim))
                {
                    var dimParts = dim.Split(new Char[] { 'x', 'X' });

                    int width = -1;
                    int height = -1;
                    width = Convert.ToInt32(dimParts[0]);
                    height = Convert.ToInt32(dimParts[1]);

                    var oImg = Image.FromFile(Server.MapPath(decodedPath));
                    var newImg = _imaging.ScaleImage(oImg, width, height,true);
                    Stream ms = new MemoryStream();
                    newImg.Save(ms,_imaging.GetFormat(ext));
                    ms.Position = 0;
                    return File(ms, ct);
                }
            }

            return File(decodedPath, ct);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, string path)
        {
            string fileName = upload.FileName;

            string basePath = Server.MapPath("~/media");
            upload.SaveAs(basePath + "\\" + fileName);
            return Content("Upload successful");
        }

      

    }
}