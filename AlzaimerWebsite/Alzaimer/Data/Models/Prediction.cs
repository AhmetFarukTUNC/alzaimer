using System.ComponentModel.DataAnnotations;

namespace AlzaimerApp.Data.Models
{

 

    
        public class Prediction
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string FileName { get; set; }

            [Required]
            public byte[] ImageData { get; set; }

            [Required]
            public string PredictedClass { get; set; }

            [Required]
            public float Confidence { get; set; }

            // 🔹 Kullanıcı kimliği (Foreign Key)
            [Required]
            public int UserId { get; set; }

            // 🔹 Oluşturulma zamanı
            public DateTime CreatedAt { get; set; } = DateTime.Now;
        }
    }






