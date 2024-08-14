using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AttunedWebApi.Utils;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public static class ImageUtils
{
    public static byte[] ResizeImage(byte[] imageBytes, int maxDim, int quality)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return imageBytes;
        }
        
        using var stream = new MemoryStream(imageBytes);

        var image = Image.FromStream(stream);

        var (width, height) = GetNewSize(image, maxDim);

        var resized = Resize(image, width, height);

        return EncodeToQuality(resized, quality);
    }
    
    private static (int width, int height) GetNewSize(Image image, int maxDim)
    {
        var aspectRatio = image.Height / Convert.ToDouble(image.Width);

        int width;
        int height;
        
        if (image.Width < maxDim && image.Height < maxDim)
        {
            return (width: image.Width, height: image.Height);
        }
        
        if (image.Width > image.Height)
        {
            width = image.Width < maxDim ? image.Width : maxDim;
            height = Convert.ToInt32(aspectRatio * width);
        }
        else
        {
            height = image.Height < maxDim ? image.Height : maxDim;
            width = Convert.ToInt32(height / aspectRatio);
        }

        return (width, height);
    }

    private static Image Resize(Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using var graphics = Graphics.FromImage(destImage);
        
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        using var wrapMode = new ImageAttributes();
        
        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);

        return destImage;
    }

    private static byte[] EncodeToQuality(Image image, int quality)
    {
        var jpgEncoder = GetEncoder(ImageFormat.Jpeg);

        using var outStream = new MemoryStream();

        var parameters = new EncoderParameters();

        parameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
        
        image.Save(outStream, jpgEncoder, parameters);

        return outStream.ToArray();
    }
    
    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        var codecs = ImageCodecInfo.GetImageDecoders();

        return codecs.First(codec => codec.FormatID == format.Guid);
    }
}