using BugTrackerService.Data;
using BugTrackerService.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BugTrackerService.Extensions
{
    public static class FileUploadHelperExtensions
    {
        private static FileStream fileStream = null;
        private static List<FileDetail> fileDetails = new List<FileDetail>();
        public static async Task<List<FileDetail>> UploadFileAsync(this IHostingEnvironment _host,
            ApplicationDbContext _context, 
            int Id, 
            IFormFileCollection files)
        {
            for(int i=0; i<files.Count; i++)
            {
                var file = files[i];
                if(file != null && file.Length > 0)
                {
                    try
                    {
                        FileDetail fileDetail = new FileDetail()
                        {
                            Id = Guid.NewGuid(),
                            FileName = Path.GetFileName(file.FileName),
                            Extension = Path.GetExtension(file.FileName).ToLower(),
                            TicketId = Id,
                        };
                        var uploads = Path.Combine(_host.WebRootPath, "Uploads");
                        var filePath = Path.Combine(uploads, fileDetail.Id + fileDetail.Extension);
                        fileStream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(fileStream);
                        fileDetails.Add(fileDetail);
                        _context.FileDetail.Add(fileDetail);
                    }
                    catch (IOException)
                    {
                        throw;
                    }
                    finally
                    {
                        fileStream.Close();
                    }
                }
            }
            return fileDetails;
        }
    }
}
