using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TestDropBoxApp.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        #region test
        public static async Task Run()
        {
            using (var dbx = new DropboxClient("As1AQyu_qJAAAAAAAAAAHJmONLCS5kU_FlJQcJhdWmcwpkixJrEyt_60I3qWLpNP"))
            {
                var full = await dbx.Users.GetCurrentAccountAsync();
                Console.WriteLine("{0} - {1}", full.Name.DisplayName, full.Email);
            }

        }
        public async Task ListRootFolder(DropboxClient dbx)
        {
            var list = await dbx.Files.ListFolderAsync(string.Empty);

            // show folders then files
            foreach (var item in list.Entries.Where(i => i.IsFolder))
            {
                Console.WriteLine("D  {0}/", item.Name);
            }

            foreach (var item in list.Entries.Where(i => i.IsFile))
            {
                Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
            }
        }
        public async Task Download(DropboxClient dbx, string folder, string file)
        {
            using (var response = await dbx.Files.DownloadAsync("/" + folder + "/" + file))
            {
                Console.WriteLine(await response.GetContentAsStringAsync());
                Task<string> str = response.GetContentAsStringAsync();
            }
        }
        #endregion

        public ActionResult Index()
        {
            //await Run();
            //var dbx = new DropboxClient("As1AQyu_qJAAAAAAAAAAHJmONLCS5kU_FlJQcJhdWmcwpkixJrEyt_60I3qWLpNP");
            //await ListRootFolder(dbx);
            //await Download(dbx, "Newfolder", "EmployeeCallMethod.txt");
            //await Upload(dbx, "Newfolder", "text.txt", "Hello test");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(FormCollection fc, HttpPostedFileBase file)
        {
            //Check file is not null and ContentLength should be greater than 0
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    //Call Upload Method By Passing FolderName, FileName and HttpPostedFileBase Object
                    await Upload("Newfolder", file);
                }
                catch (Exception ex)
                {

                }
            }
            else { }

            return View();
        }
        async Task Upload(string folder, HttpPostedFileBase file)
        {
            //Create object of DropboxClient Class by passing AccessToken
            var dbx = new DropboxClient("As1AQyu_qJAAAAAAAAAAHJmONLCS5kU_FlJQcJhdWmcwpkixJrEyt_60I3qWLpNP");
            try
            {
                //Get Select File Name
                string fileName = file.FileName;

                //Get Stream of Selected File
                Stream fs = file.InputStream;

                //Convert Stream to Binary
                BinaryReader br = new BinaryReader(fs);

                //Convert Binary to bytes
                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                //Conert bytes to MemoryStream
                System.IO.MemoryStream dataStream = new System.IO.MemoryStream(bytes);

                //Upload file to Dropbox
                var updated = await dbx.Files.UploadAsync("/" + folder + "/" + fileName, WriteMode.Overwrite.Instance, body: dataStream);
                if (updated != null)
                {
                    TempData["flag"] = 1;
                }
                else
                {
                    TempData["flag"] = 0;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //Destroy object if not null
                if (dbx != null)
                {
                    dbx.Dispose();
                }
            }
        }
    }
}