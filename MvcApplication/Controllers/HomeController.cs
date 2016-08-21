using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string LoadCSV() {
            try {
                string json = string.Empty;
                for (int i = 0; i < Request.Files.Count; i++) {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                    //Use the following properties to get file's name, size and MIMEType
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;
                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    //To save file, use SaveAs method
                    file.SaveAs(Server.MapPath("~/Content/Uploads/") + fileName);

                    var serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;

                    var csv = new List<string[]>(); // or, List<YourClass>
                    var lines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/Uploads/") + fileName);
                    foreach (string line in lines)
                        csv.Add(line.Split(',')); // or, populate YourClass   
                    json = serializer.Serialize(csv);
                }
                return json;
            } catch (Exception ex) {
                return "Error ocurred while loading the file." + Environment.NewLine + "Error: " + ex.Message;
            }
        }
    }
}
