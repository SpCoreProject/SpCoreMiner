using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

using System;
using SpCoreMiner.Models;
using SpCoreMiner.Serices;


namespace SpCoreMiner.Data
{
    public class SettingDB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<SettingDB> Instance = new AsyncLazy<SettingDB>(async () =>
        {
            var instance = new SettingDB();
            CreateTableResult result = await Database.CreateTableAsync<SettingModel>();
            return instance;

        });

        public SettingDB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<SettingModel>> GetItemsAsync()
        {
            return Database.Table<SettingModel>().ToListAsync();
        }
        public async Task<SettingModel> GetItemAsync()
        {
            return await Database.Table<SettingModel>().FirstOrDefaultAsync();
        }

        public async Task<SettingModel> CheckItemAsync()
        {
            var db = await Database.Table<SettingModel>().FirstOrDefaultAsync();
            try
            {
                if (db == null)
                {
                    SettingModel item = new SettingModel();
                    item.AutoDownload = true;
                    item.pin = 0;
                    item.Balance = 0.00000000;
                    item.Theme = "Dark";
                    item.Display = "HDMI";
                    item.WlanName = "";
                    item.WlanPass = "";
                    item.VpnPass = "";
                    item.VpnUser = "";
                    item.VpnIp = "";
                    item.AutoDownload = true;
                    item.Autoupdate = true;
                    item.ShowBalance = true;
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

        public Task<int> SaveItemAsync(SettingModel item)
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

        public Task<int> AddItemAsync(SettingModel item)
        {
            return Database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(SettingModel item)
        {
            return Database.DeleteAsync(item);
        }
        public async Task DeleteAllItemAsync()
        {
            await Database.DeleteAllAsync<SettingModel>();
        }
    }
}
