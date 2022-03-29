﻿using System.Net;
using System.Net.Mail;
using Havit.Extensions.DependencyInjection.Abstractions;
using Havit.Bonusario.Contracts.Infrastructure;
using Microsoft.Extensions.Options;

namespace Havit.Bonusario.Services.Mailing;

[Service]
public class MailingService : IMailingService
{
	private readonly MailingOptions options;

	public MailingService(
		IOptions<MailingOptions> options)
	{
		this.options = options.Value;
	}

	public void Send(MailMessage mailMessage)
	{
		using (SmtpClient smtpClient = new SmtpClient())
		{
			smtpClient.Host = options.SmtpServer;
			if (options.SmtpPort != null)
			{
				smtpClient.Port = options.SmtpPort.Value;
			}
			smtpClient.EnableSsl = options.UseSsl;
			if (options.HasCredentials())
			{
				smtpClient.Credentials = new NetworkCredential(options.SmtpUsername, options.SmtpPassword);
			}

			if ((mailMessage.From == null)
				|| String.IsNullOrWhiteSpace(mailMessage.From.Address))
			{
				mailMessage.From = new MailAddress(options.From);
			}

			smtpClient.Send(mailMessage);
		}
	}
}
