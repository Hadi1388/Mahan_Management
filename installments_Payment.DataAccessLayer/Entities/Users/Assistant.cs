using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.DataAccessLayer.Entities.Users
{
    public class Assistant
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است")]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "کد ملی الزامی است")]
        [MaxLength(10)]
        [MinLength(10)]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "شماره تلفن الزامی است")]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [MaxLength(200)]
        public string? ProfileImage { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
