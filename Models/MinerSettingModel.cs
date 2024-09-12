using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpCoreMiner.Models
{
    public class MinerSettingModel
	{
		[PrimaryKey, AutoIncrement]
		public int? id { get; set; } 
		public string? MiningType { get; set; } 
		public string? MiningStrategy { get; set; } 
		public string? HardWareManage { get; set; }
		public string? PowerManage { get; set; } 
		public string? PoolName { get; set; } 
		public string? PoolApiCode { get; set; } 
		public string? ProxyUser { get; set; }
		public string? ProxyIp { get; set; }
		public bool AutoDownload { get; set; }
		public string? Notification { get; set; }

	}
}
