using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using PhyGen.Application.Questions.Interfaces;

public class CloudinaryImageStorage : IImageStorage
{
    private readonly Cloudinary _cloudinary;
    private readonly IConfiguration _config;

    public CloudinaryImageStorage(Cloudinary cloudinary, IConfiguration config)
    {
        _cloudinary = cloudinary;
        _config = config;
    }

    public async Task<string> SaveAsync(byte[] data, string contentType, string fileNameHint = null)
    {
        // NEW: chuẩn hoá định dạng ảnh
        data = ImageConvertHelper.ToCloudinaryFriendlyPngIfNeeded(
            data, contentType, out var normalizedContentType, out var ext);

        var folderBase = _config["Cloudinary:Folder"] ?? "uploads";
        var folder = $"{folderBase}/{DateTime.UtcNow:yyyy/MM}";
        var publicId = Guid.NewGuid().ToString("N");

        using var ms = new MemoryStream(data);
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription((fileNameHint ?? "image") + ext, ms),
            Folder = folder,
            PublicId = publicId,
            UseFilename = false,
            UniqueFilename = false,
            Overwrite = false,

            // tuỳ chọn: tạo bản web-optimized
            EagerTransforms = new List<Transformation> {
                new Transformation().Quality("auto").FetchFormat("auto")
            }
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        if (result.Error != null)
            throw new Exception($"Cloudinary upload failed: {result.Error.Message}");

        return result.SecureUrl?.ToString() ?? result.Url?.ToString();
    }
}