using EN.Sek.Meter.Entities;
using Microsoft.EntityFrameworkCore;

namespace EN.Sek.Meter.DAL
{
	public class AccountDataProvider : IAccountDataProvider
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceProvider _serviceProvider;

		public AccountDataProvider(ApplicationDbContext context, IServiceProvider serviceProvider)
		{
			_context = context;
			_serviceProvider = serviceProvider;
		}

		public async Task<bool> AccountExists(int id)
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				var account = await context.Account
					.Include(a => a.MeterReadings)
					.AsNoTracking()
					.FirstOrDefaultAsync(a => a.Id == id);
				return account != null;
			}
		}

		public async Task<Account> CreateAccountAsync(Account account)
		{
			_context.Account.Add(account);
			await _context.SaveChangesAsync();
			return account;
		}

		public async Task<Account> UpdateAccountAsync(Account account)
		{
			_context.Account.Update(account);
			await _context.SaveChangesAsync();
			return account;
		}

		public async Task DeleteAccountAsync(int id)
		{
			var account = await _context.Account.FindAsync(id);
			if (account == null)
			{
				// Handle the case where the account is not found
				throw new ArgumentException($"Account with id {id} not found.");
			}
			_context.Account.Remove(account);
			await _context.SaveChangesAsync();
		}
	}
}