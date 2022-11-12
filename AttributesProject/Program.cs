using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AttributesProject
{
    class EmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return true;
        }
    }
    class PostIndexAttribute : ValidationAttribute
    {
        int count;
        public PostIndexAttribute(int count = 6)
        {
            this.count = count;
        }
        public override bool IsValid(object? value)
        {
            //string? s;
            //if (value is string)
            //    s = value as string;

            if(value is string str)
            {
                bool isDigit = str.Length == count;
                if(isDigit) 
                    for (int i = 0; i < str.Length; i++)
                        isDigit = isDigit && Char.IsDigit(str[i]);
                
                if (isDigit)
                    return true;
                else
                {
                    ErrorMessage = "Почтовый индекс неверный";
                    return false;
                }
                    
            }
            else
            {
                return false;
            }
                
        }
    }

    [Email]
    class Email
    {
        [Required(AllowEmptyStrings = true)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Length of login 3 - 50")]
        public string Login { set; get; }

        [PostIndex(5)]
        public string Index { set; get; }

        [Required]
        public string Password { set; get; }
        
        [Compare("Password")]
        public string PasswordRepeat { set; get; }
        
        [RegularExpression(@"^\+[1-9]\(\d{3}\) \d{3}-\d{2}-\d{2}$", ErrorMessage = "Pattern phone: +X(XXX) XXX-XX-XX")]
        public string Phone { set; get; }

        [Range(1, 1000)]
        public int Id { set; get; }
        Email(string login, string password, string passwordRepeat, int id, string phone, string index)
        {
            Login = login;
            Password = password;
            PasswordRepeat = passwordRepeat;
            Id = id;
            Phone = phone;
            Index = index;
        }

        public static Email CreatEmail()
        {
            Email email = new Email("", "qwerty", "qwerty", 10, "+7(999) 123-45-68", "12345");

            var context = new ValidationContext(email);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(email, context, results, true))
            {
                Console.WriteLine("email create with errors:");
                foreach(var result in results)
                    Console.WriteLine(result.ErrorMessage);
            }

            return email;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Email email = Email.CreatEmail();
        }
    }
}