namespace verfiyemail.Models
{
    public class Token
    {

        //public Token(string Email, string resetToken, DateTime expiryDate)
        //{
        //    this.Email = Email;
        //    this.resetToken = resetToken;
        //    ExpiryDate = expiryDate;
        //}

        public string Email { get; set; }
        public string ResetToken { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

}

