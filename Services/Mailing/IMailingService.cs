using System.Net.Mail;

namespace Havit.Bonusario.Services.Mailing;

public interface IMailingService
{
	void Send(MailMessage mailMessage);
}
