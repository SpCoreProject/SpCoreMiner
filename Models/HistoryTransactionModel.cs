using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpCoreMiner.Models
{
    public class HistoryTransactionModel
    {
		[PrimaryKey, AutoIncrement]
		public int id { get; set; }
		public decimal Amount { get; set; } 
		public decimal Fee { get; set; } 
		public string FromWallet { get; set; }
		public string ToWallet { get; set; }
		public string HashId { get; set; }
		public string BlockId { get; set; }
		public int Confirm { get; set; }
		public string SendModel { get; set; } 
		public DateTime Date { get; set; } 
	}
}
