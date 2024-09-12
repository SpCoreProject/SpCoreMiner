using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpCoreMiner.Models
{
    public class SettingModel
	{
		[PrimaryKey, AutoIncrement]
		public int? id { get; set; }
		public int? pin { get; set; } 
		public double? Balance { get; set; } 
		public string? Theme { get; set; }
		public string? Display { get; set; }
		public string? WlanName { get; set; }
		public string? WlanPass { get; set; }
		public string? VpnPass { get; set; }
		public string? VpnUser { get; set; }
		public string? VpnIp { get; set; }
		public bool AutoDownload { get; set; }
		public bool Autoupdate { get; set; }
		public bool ShowBalance { get; set; }
	}
}
