using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace NextHireApp.Service
{
    public interface IFileProcessingService : ITransientDependency
    {
        /// <summary>
        /// Kiểm tra kích thước file và chuyển đổi thành chuỗi base64
        /// </summary>
        /// <param name="fileContent">Nội dung file cần xử lý</param>
        /// <param name="fileType">Loại file (CV, CoverLetter, Attachment, ...)</param>
        /// <returns>Chuỗi base64 của file</returns>
        Task<string> ProcessFileToBase64Async(IRemoteStreamContent fileContent, string fileType);
        
        /// <summary>
        /// Kiểm tra kích thước file
        /// </summary>
        /// <param name="fileContent">Nội dung file cần kiểm tra</param>
        /// <param name="fileType">Loại file (CV, CoverLetter, Attachment, ...)</param>
        /// <returns>True nếu kích thước hợp lệ, False nếu vượt quá giới hạn</returns>
        Task<bool> ValidateFileSize(IRemoteStreamContent fileContent, string fileType);
        
        /// <summary>
        /// Kiểm tra loại file (extension)
        /// </summary>
        /// <param name="fileContent">Nội dung file cần kiểm tra</param>
        /// <param name="fileType">Loại file (CV, CoverLetter, Attachment, ...)</param>
        /// <returns>True nếu định dạng hợp lệ, False nếu không hợp lệ</returns>
        Task<bool> ValidateFileType(IRemoteStreamContent fileContent, string fileType);
    }
}
