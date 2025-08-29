using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "لطفاً نام و نام خانوادگی را وارد کنید")]
    [Display(Name = "نام و نام خانوادگی")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "لطفاً شماره تلفن را وارد کنید")]
    [Phone(ErrorMessage = "شماره تلفن وارد شده معتبر نیست")]
    [Display(Name = "شماره تلفن")]
    public string PrimaryPhoneNumber { get; set; }

    [Required(ErrorMessage = "لطفاً رمز عبور را وارد کنید")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; }
}
