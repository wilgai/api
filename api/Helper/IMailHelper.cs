using api.Responses;

namespace api.Helper
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
