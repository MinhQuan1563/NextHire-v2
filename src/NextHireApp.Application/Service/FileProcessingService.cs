using Microsoft.Extensions.Configuration;
using NextHireApp.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;

namespace NextHireApp.Services
{
    [RemoteService(false)]
    public class FileProcessingService : NextHireAppAppService, IFileProcessingService
    {
        private readonly IConfiguration _configuration;
        
        // Mặc định giới hạn kích thước file (bytes) nếu không có trong cấu hình
        private const int DEFAULT_MAX_CV_SIZE = 5 * 1024 * 1024; // 5MB
        private const int DEFAULT_MAX_COVER_LETTER_SIZE = 2 * 1024 * 1024; // 2MB
        private const int DEFAULT_MAX_ATTACHMENT_SIZE = 5 * 1024 * 1024; // 5MB
        
        // Định dạng file được chấp nhận
        private readonly Dictionary<string, string[]> _allowedFileTypes = new Dictionary<string, string[]>
        {
            { "CV", new[] { ".pdf" } },
            { "CoverLetter", new[] { ".pdf" } },
            { "Attachment", new[] { ".pdf", ".jpg", ".jpeg" } }
        };

        public FileProcessingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Kiểm tra kích thước file và chuyển đổi thành chuỗi base64
        /// </summary>
        public async Task<string> ProcessFileToBase64Async(IRemoteStreamContent fileContent, string fileType)
        {
            if (fileContent == null)
            {
                return null;
            }
            
            // Kiểm tra kích thước file
            bool isValidSize = await ValidateFileSize(fileContent, fileType);
            if (!isValidSize)
            {
                string errorCode = fileType switch
                {
                    "CV" => NextHireAppDomainErrorCodes.CvFileTooLarge,
                    "CoverLetter" => NextHireAppDomainErrorCodes.CoverLetterFileTooLarge,
                    "Attachment" => NextHireAppDomainErrorCodes.AttachmentFileTooLarge,
                    _ => NextHireAppDomainErrorCodes.CvFileTooLarge
                };
                
                int maxSize = GetMaxFileSize(fileType) / (1024 * 1024);
                throw new BusinessException(errorCode)
                    .WithData("maxSize", $"{maxSize}MB");
            }
            
            // Kiểm tra loại file
            bool isValidType = await ValidateFileType(fileContent, fileType);
            if (!isValidType)
            {
                throw new BusinessException(NextHireAppDomainErrorCodes.InvalidFileType)
                    .WithData("allowedTypes", string.Join(", ", _allowedFileTypes[fileType]));
            }

            // Convert to Base64
            await using var inputStream = fileContent.GetStream();
            using var memoryStream = new MemoryStream();
            await inputStream.CopyToAsync(memoryStream);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        /// Kiểm tra kích thước file
        /// </summary>
        public async Task<bool> ValidateFileSize(IRemoteStreamContent fileContent, string fileType)
        {
            if (fileContent == null)
            {
                return true;
            }
            
            int maxFileSize = GetMaxFileSize(fileType);
            
            using var memoryStream = new MemoryStream();
            await fileContent.GetStream().CopyToAsync(memoryStream);
            
            // Reset vị trí stream để có thể đọc lại sau này
            fileContent.GetStream().Position = 0;
            
            return memoryStream.Length <= maxFileSize;
        }

        /// <summary>
        /// Kiểm tra loại file (extension)
        /// </summary>
        public async Task<bool> ValidateFileType(IRemoteStreamContent fileContent, string fileType)
        {
            if (fileContent == null)
            {
                return true;
            }
            
            // Nếu không có cấu hình cho loại file này, cho phép tất cả
            if (!_allowedFileTypes.ContainsKey(fileType))
            {
                return true;
            }
            
            string fileName = fileContent.FileName;
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            
            return _allowedFileTypes[fileType].Contains(extension);
        }
        
        /// <summary>
        /// Lấy kích thước tối đa cho từng loại file
        /// </summary>
        private int GetMaxFileSize(string fileType)
        {
            return fileType switch
            {
                "CV" =>  DEFAULT_MAX_CV_SIZE,
                "CoverLetter" => DEFAULT_MAX_COVER_LETTER_SIZE,
                "Attachment" => DEFAULT_MAX_ATTACHMENT_SIZE,
                _ => DEFAULT_MAX_CV_SIZE // Mặc định nếu không xác định được loại file
            };
        }
    }
}