using ICSharpCode.SharpZipLib.Zip;
using SeekAndDestroy.GeoCoding.Services.Google;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.packager;
using Umbraco.Core.IO;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace SeekAndDestroy.Umbraco.Installer
{
    public partial class Install : UmbracoUserControl
    {
        private const string REPO_GUID = "65194810-1f85-11dd-bd0b-0800200c9a66";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGeoCode_Click(object sender, EventArgs e)
        {
            IList<string> successList = new List<string>();
            IList<string> failedList = new List<string>();

            try
            {

                int docType = 0;
                if (int.TryParse(docmapper.SelectedDocType, out docType))
                {
                    string docTypeAlias = Services.ContentTypeService.GetAllContentTypes().Where(ct => ct.Id == docType).FirstOrDefault().Alias;

                    UmbracoHelper help = new UmbracoHelper(UmbracoContext);

                    foreach (var node in help.TypedContentAtRoot().First().Descendants(docTypeAlias))
                    {
                        //geocode
                        var address = string.Empty;
                        foreach (var alias in docmapper.SelectedAddressProperties.Split(','))
                        {
                            if (alias != string.Empty)
                                address += node.GetPropertyValue<string>(alias);
                        }

                        var r = GoogleGeoCoder.CallGeoWS(address);

                        if (r.Results.Any())
                        {
                            var cs = Services.ContentService;
                            var content = cs.GetById(node.Id);

                            content.SetValue(docmapper.SelectedLocationProperty, r.Results[0].Geometry.Location.Lat.ToString(Utility.NumberFormatInfo) + "," + r.Results[0].Geometry.Location.Lng.ToString(Utility.NumberFormatInfo) + ",13");
                            cs.SaveAndPublish(content);
                        }
                    }


                    successList.Add("Content geocoded");
                }

            }
            catch (Exception)
            {
                // Append package to failed list
                failedList.Add("Failed to geocode content");
            }

           
            // Show message
            if (successList.Count > 0)
            {
                //Successfull
                feedback.type = umbraco.uicontrols.Feedback.feedbacktype.success;
                feedback.Text = string.Format("{0} installed successfully", successList.First());
            }
            else
            {
                //Failed
                feedback.type = umbraco.uicontrols.Feedback.feedbacktype.error;
                feedback.Text = string.Format("{0} failed to install", successList.First());
            }
        }
        protected void btnGoogleMapsDataType_Click(object sender, EventArgs e)
        {
            IList<string> successList = new List<string>();
            IList<string> failedList = new List<string>();

            var url = "http://our.umbraco.org/FileDownload?id=4425&release=1";

            try
            {
                var tmpFileName = Guid.NewGuid().ToString() + ".umb";
                var tmpFilePath = IOHelper.MapPath(SystemDirectories.Data + Path.DirectorySeparatorChar + tmpFileName);

                // Download file
                new WebClient().DownloadFile(url, tmpFilePath);

                // Extract package guid from zip
                var packageGuid = GetPackageGuidFromZip(tmpFilePath);
                if (packageGuid == default(Guid))
                    packageGuid = Guid.NewGuid();

                var packageGuidString = packageGuid.ToString("D");

                // Check package isn't already installed
                if (!InstalledPackage.isPackageInstalled(packageGuidString))
                {
                    // Rename file
                    var packageFileName = packageGuidString + ".umb";
                    var packageFilePath = IOHelper.MapPath(SystemDirectories.Data + Path.DirectorySeparatorChar + packageFileName);
                    System.IO.File.Move(tmpFilePath, packageFilePath);

                    // Install package
                    var installer = new umbraco.cms.businesslogic.packager.Installer();
                    var tempDir = installer.Import(packageFileName);
                    installer.LoadConfig(tempDir);
                    var packageId = installer.CreateManifest(tempDir, packageGuidString, REPO_GUID);
                    installer.InstallFiles(packageId, tempDir);
                    installer.InstallBusinessLogic(packageId, tempDir);
                    installer.InstallCleanUp(packageId, tempDir);

                    // Append package to success list
                    successList.Add("Google maps datatype");

                   
                }
                else
                {
                    // Append package to failed list
                    failedList.Add("Google maps datatype");
                }

            }
            catch (Exception)
            {
                // Append package to failed list
                failedList.Add("Google maps datatype");
            }


            // Show message
            if (successList.Count > 0)
            {
               //Successfull
                feedback.type = umbraco.uicontrols.Feedback.feedbacktype.success;
                feedback.Text = string.Format("{0} installed successfully", successList.First());
            }
            else
            {                
                //Failed
                feedback.type = umbraco.uicontrols.Feedback.feedbacktype.error;
                feedback.Text = string.Format("{0} failed to install", successList.First());
             }
        }

        private Guid GetPackageGuidFromZip(string path)
        {
            var packageGuid = default(Guid);
            ZipInputStream zip = new ZipInputStream(System.IO.File.OpenRead(path));
            ZipEntry zipEntry;
            while ((zipEntry = zip.GetNextEntry()) != null)
            {
                var dir = Path.GetDirectoryName(zipEntry.Name);
                try
                {
                    packageGuid = new Guid(dir);
                    break;
                }
                catch (FormatException)
                {
                    continue;
                }
            }
            zip.Close();

            return packageGuid;
        }
    }
}