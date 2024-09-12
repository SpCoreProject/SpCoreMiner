using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

using System;
using SpCoreMiner.Models;
using SpCoreMiner.Serices;


namespace SpCoreMiner.Data
{
	public class AccountDB
	{
		static SQLiteAsyncConnection Database;

		public static readonly AsyncLazy<AccountDB> Instance = new AsyncLazy<AccountDB>(async () =>
		{
			var instance = new AccountDB();
			CreateTableResult result = await Database.CreateTableAsync<Accounts>();
			return instance;

		});

		public AccountDB()
		{
			Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
		}

		public Task<List<Accounts>> GetItemsAsync()
		{
			return Database.Table<Accounts>().ToListAsync();
		}
		public Task<Accounts> GetItemAsync()
		{
			return Database.Table<Accounts>().FirstOrDefaultAsync();
		}
		public async Task<Accounts> GetAccountAsync()
		{

			var itemx = await GetItemAsync();
			try
			{
				if (itemx.id == 0 || itemx == null)
				{
					Accounts item = new Accounts();
					 
					return item;
				}
				else
				{  
					return itemx;
				}

			}
			catch (Exception ex)
			{
				try
				{
					Accounts item = new Accounts();
					var msg = ex.Message;

					return item;
				}
				catch (Exception e)
				{
					var msgx = e.Message;
					return null;
				}
			}
		}
		public Task<int> SaveItemAsync(Accounts item)
		{
			if (item.id != 0)
			{
				return Database.UpdateAsync(item);
			}
			else
			{
				return Database.InsertAsync(item);
			}
		}

		public Task<int> AddItemAsync(Accounts item)
		{
			return Database.InsertAsync(item);
		}

		public Task<int> DeleteItemAsync(Accounts item)
		{
			return Database.DeleteAsync(item);
		}
		public async Task DeleteAllItemAsync()
		{
			await Database.DeleteAllAsync<Accounts>();
		}
	}
}
