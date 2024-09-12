using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

using System;
using SpCoreMiner.Models;
using SpCoreMiner.Serices;


namespace SpCoreMiner.Data
{
	public class HistoryTransactionDB
	{
		static SQLiteAsyncConnection Database;

		public static readonly AsyncLazy<HistoryTransactionDB> Instance = new AsyncLazy<HistoryTransactionDB>(async () =>
		{
			var instance = new HistoryTransactionDB();
			CreateTableResult result = await Database.CreateTableAsync<HistoryTransactionModel>();
			return instance;

		});

		public HistoryTransactionDB()
		{
			Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
		}

		public Task<List<HistoryTransactionModel>> GetItemsAsync()
		{
			return Database.Table<HistoryTransactionModel>().ToListAsync();
		}
		public Task<HistoryTransactionModel> GetItemAsync()
		{
			return Database.Table<HistoryTransactionModel>().FirstOrDefaultAsync();
		}
		public async Task<HistoryTransactionModel> GetAccountAsync()
		{

			var itemx = await GetItemAsync();
			try
			{
				if (itemx.id == 0 || itemx == null)
				{
					HistoryTransactionModel item = new HistoryTransactionModel();
					 
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
					HistoryTransactionModel item = new HistoryTransactionModel();
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
		public Task<int> SaveItemAsync(HistoryTransactionModel item)
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

		public Task<int> AddItemAsync(HistoryTransactionModel item)
		{
			return Database.InsertAsync(item);
		}

		public Task<int> DeleteItemAsync(HistoryTransactionModel item)
		{
			return Database.DeleteAsync(item);
		}
		public async Task DeleteAllItemAsync()
		{
			await Database.DeleteAllAsync<HistoryTransactionModel>();
		}
	}
}
