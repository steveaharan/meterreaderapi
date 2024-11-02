using EN.Sek.Meter.Entities;
using Microsoft.EntityFrameworkCore;

namespace EN.Sek.Meter.DAL
{
	public class AccountDataProvider : IAccountDataProvider
	{
		private readonly ApplicationDbContext _context;

		public AccountDataProvider(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<bool> AccountExists(int id)
		{
			var account = await _context.Account
				.Include(a => a.MeterReadings)
				.FirstOrDefaultAsync(a => a.Id == id);

			if (account == null)
			{
				return false;
			}

			return true;
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