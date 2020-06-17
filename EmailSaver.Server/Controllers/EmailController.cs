using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using EmailSaver.Core;

namespace EmailSaver.Server.Controllers
{
	[ApiController]
	[Route("api/emails")]
	public class EmailController : Controller
	{
		private readonly IEmailSupplier _emailSupplier;
		private readonly ILogger<EmailController> _logger;

		public EmailController(IEmailSupplier emailSupplier, ILogger<EmailController> logger)
		{
			_emailSupplier = emailSupplier;
			_logger = logger;
		}

		[HttpGet("id")]
		public async Task<IActionResult> GetEmail([FromQuery(Name = "value")] Guid id)
		{
			try
			{
				return Ok(await _emailSupplier.GetAsync(id));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.ToString());
				return StatusCode(500);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetEmails()
		{
			_logger.LogInformation("HttpGet : GetEmails");

			try
			{
				var result = await _emailSupplier.GetAllAsync();
				_logger.LogInformation($"HttpGet : GetEmails : Returned {result.Count} items");

				return Ok(result);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.ToString());
				return StatusCode(500);
			}
		}

		[HttpGet("tag")]
		public async Task<IActionResult> GetEmailsByTag([FromQuery(Name = "value")] String tag)
		{
			try
			{
				return Ok(await _emailSupplier.GetByTagAsync(tag));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.ToString());
				return StatusCode(500);
			}
		}

		[HttpGet("sender")]
		public async Task<IActionResult> GetEmailsBySender([FromQuery(Name = "value")] String sender)
		{
			try
			{
				return Ok(await _emailSupplier.GetBySenderAsync(sender));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.ToString());
				return StatusCode(500);
			}
		}

		[HttpGet("recipient")]
		public async Task<IActionResult> GetEmailsByRecipient([FromQuery(Name = "value")] String recipient)
		{
			try
			{
				return Ok(await _emailSupplier.GetByRecipientAsync(recipient));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.ToString());
				return StatusCode(500);
			}
		}

		[HttpGet("for_period")]
		public async Task<IActionResult> GetEmailsForPeriod([FromQuery(Name = "start")] DateTime start,
			[FromQuery(Name = "end")] DateTime end)
		{
			try
			{
				return Ok(await _emailSupplier.GetForPeriodAsync(start, end));
			}
			catch
			{
				return StatusCode(500);
			}
		}

		[HttpPost]
		public async Task<IActionResult> AddEmail([FromBody] Email email)
		{
			_logger.LogInformation("HttpPost : AddEmail");

			try
			{
				Guid id = await _emailSupplier.AddAsync(email);
				_logger.LogInformation($"HttpPost : AddEmail : New email was added : ID {id} will be returned");

				return Ok(id);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.ToString());
				return StatusCode(500);
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateEmail([FromBody] Email email)
		{
			try
			{
				return Ok(await _emailSupplier.UpdateAsync(email));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.ToString());
				return StatusCode(500);
			}
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteEmail([FromBody] Guid id)
		{
			try
			{
				return Ok(await _emailSupplier.DeleteAsync(id));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.ToString());
				return StatusCode(500);
			}
		}
	}
}