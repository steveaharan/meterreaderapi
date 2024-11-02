using EN.Sek.Meter.Entities;
using Microsoft.EntityFrameworkCore;

namespace EN.Sek.Meter.DAL
{
	public interface IAccountDataProvider
	{
		Task<bool> AccountExists(int id);
		Task<Account> CreateAccountAsync(Account account);
		Task<Account> UpdateAccountAsync(Account account);
		Task DeleteAccountAsync(int id);

	}
}