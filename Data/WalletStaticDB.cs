using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using SPBHashing;
using System;
using SpCoreMiner.Models;
using SpCoreMiner.Serices;


namespace SpCoreMiner.Data
{
    public class WalletStaticDB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<WalletStaticDB> Instance = new AsyncLazy<WalletStaticDB>(async () =>
        {
            var instance = new WalletStaticDB();
            CreateTableResult result = await Database.CreateTableAsync<WalletStaticModel>();
            return instance;

        });

        public WalletStaticDB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public async Task<WalletStaticModel> CheckItemAsync(string pin)
        {
            var db = await Database.Table<WalletStaticModel>().FirstOrDefaultAsync();
            try
            {
                if (db == null)
                {
                    WalletStaticModel item = new WalletStaticModel();

                    Random random = new Random();

                    int randomNumber1 = random.Next(1, 115);
                    int randomNumber2 = random.Next(1, 12);
                    string temp = randomNumber1.ToString() + MyDevices.GetMacAddr() + Guid.NewGuid().ToString() + randomNumber2.ToString();
                    var spWallet = SPB128.Hash(temp);

                    randomNumber1 = random.Next(1, 115);
                    randomNumber2 = random.Next(1, 12);
                    temp = randomNumber1.ToString() + MyDevices.GetMacAddr() + Guid.NewGuid().ToString() + randomNumber2.ToString();
                    var sppass = SPB128.Hash(temp);

                    item.Amount = 0.0000000;
                    item.Wallet = spWallet;
                    item.Password = sppass;
                    item.Type = "MinerOs";
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

        public Task<List<WalletStaticModel>> GetItemsAsync()
        {
            return Database.Table<WalletStaticModel>().ToListAsync();
        }
        public Task<WalletStaticModel> GetItemAsync()
        {
            return Database.Table<WalletStaticModel>().FirstOrDefaultAsync();
        }
        public async Task<WalletStaticModel> GetAccountAsync()
        {

            var itemx = await GetItemAsync();
            try
            {
                if (itemx.id == 0 || itemx == null)
                {
                    WalletStaticModel item = new WalletStaticModel();

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
                    WalletStaticModel item = new WalletStaticModel();
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
        public Task<int> SaveItemAsync(WalletStaticModel item)
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

        public Task<int> AddItemAsync(WalletStaticModel item)
        {
            return Database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(WalletStaticModel item)
        {
            return Database.DeleteAsync(item);
        }
        public async Task DeleteAllItemAsync()
        {
            await Database.DeleteAllAsync<WalletStaticModel>();
        }
    }
}
