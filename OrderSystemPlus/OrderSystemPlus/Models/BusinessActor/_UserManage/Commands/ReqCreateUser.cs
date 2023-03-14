namespace OrderSystemPlus.Models.BusinessActor.Commands
{
    public class ReqCreateUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
