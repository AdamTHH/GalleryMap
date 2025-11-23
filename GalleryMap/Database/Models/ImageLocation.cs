using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace GalleryMap.Models
{
    [Table("imagelocations")]
    public class ImageLocation
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] ImageData { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; }

        private ImageSource? _cachedImageSource;

        [NotMapped]
        public ImageSource ImageSource
        {
            get
            {
                if (_cachedImageSource == null)
                {
                    _cachedImageSource = ImageSource.FromStream(() =>
                        new MemoryStream(ImageData));
                }
                return _cachedImageSource;
            }
        }
    }
}
