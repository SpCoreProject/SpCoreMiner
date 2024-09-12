using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpCoreMiner.Models
{
    public class WalletStaticModel
    {
		[PrimaryKey, AutoIncrement]
		public int? id { get; set; }
		public double? Amount { get; set; } 
		public string? Wallet { get; set; }
		public string? Password { get; set; }
		public string? Type { get; set; } 
	}
}
