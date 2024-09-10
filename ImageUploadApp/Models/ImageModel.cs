namespace ImageUploadApp.Models
{ 
    using System.ComponentModel.DataAnnotations;

    public class ImageModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Image Name")]
        public string ImageName { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public string ImageType { get; set; }
    }
}
