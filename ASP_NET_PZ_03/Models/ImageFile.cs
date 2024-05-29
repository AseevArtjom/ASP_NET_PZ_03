namespace ASP_NET_PZ_03.Models
{
    public class ImageFile
    {
        public int Id { get; set; }

        public string OriginalFilename { get; set; }
        public string Filename { get; set; }

        public string ImgSrc => "/uploads/images/" + Filename[0] + "/" + Filename[1] + "/" + Filename;
    }
}
