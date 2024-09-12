using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

using System;
using SpCoreMiner.Models;
using SpCoreMiner.Serices;


namespace SpCoreMiner.Data
{
	public class MinerSettingDB
	{
		static SQLiteAsyncConnection Database;

		public static readonly AsyncLazy<MinerSettingDB> Instance = new AsyncLazy<MinerSettingDB>(async () =>
		{
			var instance = new MinerSettingDB();
			CreateTableResult result = await Database.CreateTableAsync<MinerSettingModel>();
			return instance;

		});

		public MinerSettingDB()
		{
			Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
		}

		public Task<List<MinerSettingModel>> GetItemsAsync()
		{
			return Database.Table<MinerSettingModel>().ToListAsync();
		}
		public Task<MinerSettingModel> GetItemAsync()
		{
			return Database.Table<MinerSettingModel>().FirstOrDefaultAsync();
		}

        public async Task<MinerSettingModel> CheckItemAsync(string pin)
        {
            var db = await Database.Table<MinerSettingModel>().FirstOrDefaultAsync();
            try
            {
                if (db == null)
                {
                    MinerSettingModel item = new MinerSettingModel();
                    item.AutoDownload = true;

                    item.MiningType = "Core";
                    item.MiningStrategy = "Solo CPU Mining";
                    item.HardWareManage = "Protective Systems";
                    item.PowerManage = "Show";
                    item.PoolName = "";
                    item.PoolApiCode = "";
                    item.ProxyUser = "";
                    item.ProxyIp = "";
                    item.AutoDownload = true;
                    item.Notification = "Off"; 
                    // اینجا   اطلاعات دستگاه رو ذخیره میکنم 
                    await Database.InsertAsync(item);
                    return item;
                }
                else
                {
                    return db;
                }

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return null;
            }

        }

        public async Task<MinerSettingModel> GetAccountAsync()
		{

			var itemx = await GetItemAsync();
			try
			{
				if (itemx.id == 0 || itemx == null)
				{
					MinerSettingModel item = new MinerSettingModel();
					 
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
					MinerSettingModel item = new MinerSettingModel();
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
		public Task<int> SaveItemAsync(MinerSettingModel item)
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

		public Task<int> AddItemAsync(MinerSettingModel item)
		{
			return Database.InsertAsync(item);
		}

		public Task<int> DeleteItemAsync(MinerSettingModel item)
		{
			return Database.DeleteAsync(item);
		}
		public async Task DeleteAllItemAsync()
		{
			await Database.DeleteAllAsync<MinerSettingModel>();
		}
	}
}
