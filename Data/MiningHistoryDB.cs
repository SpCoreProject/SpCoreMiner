using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

using System;
using SpCoreMiner.Models;
using SpCoreMiner.Serices;


namespace SpCoreMiner.Data
{
	public class MiningHistoryDB
	{
		static SQLiteAsyncConnection Database;

		public static readonly AsyncLazy<MiningHistoryDB> Instance = new AsyncLazy<MiningHistoryDB>(async () =>
		{
			var instance = new MiningHistoryDB();
			CreateTableResult result = await Database.CreateTableAsync<MiningHistoryModel>();
			return instance;

		});

		public MiningHistoryDB()
		{
			Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
		}

		public Task<List<MiningHistoryModel>> GetItemsAsync()
		{
			return Database.Table<MiningHistoryModel>().ToListAsync();
		}
		public Task<MiningHistoryModel> GetItemAsync()
		{
			return Database.Table<MiningHistoryModel>().FirstOrDefaultAsync();
		}
		public async Task<MiningHistoryModel> GetAccountAsync()
		{

			var itemx = await GetItemAsync();
			try
			{
				if (itemx.id == 0 || itemx == null)
				{
					MiningHistoryModel item = new MiningHistoryModel();
					 
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
					MiningHistoryModel item = new MiningHistoryModel();
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
		public Task<int> SaveItemAsync(MiningHistoryModel item)
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

		public Task<int> AddItemAsync(MiningHistoryModel item)
		{
			return Database.InsertAsync(item);
		}

		public Task<int> DeleteItemAsync(MiningHistoryModel item)
		{
			return Database.DeleteAsync(item);
		}
		public async Task DeleteAllItemAsync()
		{
			await Database.DeleteAllAsync<MiningHistoryModel>();
		}
	}
}
