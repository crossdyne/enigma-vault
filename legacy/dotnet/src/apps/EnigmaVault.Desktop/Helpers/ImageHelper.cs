using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows.Media;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

namespace EnigmaVault.Desktop.Helpers
{
    public static class ImageHelper
    {
        private static readonly HttpClient _httpClient = new();

        public static async Task<DrawingImage?> LoadSvgFromUrlAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            try
            {
                var svgContent = await _httpClient.GetStringAsync(url);

                var settings = new WpfDrawingSettings
                {
                    IncludeRuntime = true,
                    TextAsGeometry = false
                };

                var reader = new FileSvgReader(settings);

                using var stringReader = new StringReader(svgContent);
                var drawingGroup = reader.Read(stringReader);

                if (drawingGroup == null)
                    return null;

                var drawingImage = new DrawingImage(drawingGroup);
                drawingImage.Freeze();

                return drawingImage;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SvgImageHelper] Ошибка загрузки SVG: {ex.Message}");
                return null;
            }
        }
    }
}