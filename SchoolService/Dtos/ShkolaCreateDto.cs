using System.ComponentModel.DataAnnotations;

namespace SchoolService.Dtos
{
    public class ShkolaCreateDto
    {
        [Required]
        public string Name { get; set; }       

        [Required]
        public string Publisher { get; set; }   

        [Required]
        public string Grade { get; set; }
    }
}