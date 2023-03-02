namespace OrderSystemPlus.Models.BusinessActor.Commands
{
    public class ReqUserCreate
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
