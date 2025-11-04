using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryMap.Models
{
    [Table("imagelocations")]
    public class ImageLocation
    {
        [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public ImageSource ImageSource => ImageSource.FromStream(() =>
            new MemoryStream(Convert.FromBase64String(ImageUrl)));
    }
}
