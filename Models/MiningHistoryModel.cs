using SQLite;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SpCoreMiner.Models
{
    public class MiningHistoryModel
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Mining { get; set; }
        public string Type { get; set; }
        public string Block { get; set; }
        public double Amount { get; set; }
        public string Hash { get; set; }
        public double Size { get; set; }
        public int Confirm { get; set; } 
        public double Distance { get; set; }
        public double InputValue { get; set; }
        public double OutputValue { get; set; }
        public double Fees { get; set; }
        public double Depth { get; set; } 
        public string Version { get; set; }
        public string Difficulty { get; set; }
        public int Bits { get; set; }
        public string Minted { get; set; }
        public long Height { get; set; }
        public string Confirmations { get; set; }
        public string Miner { get; set; }
        public DateTime Date { get; set; }
        public int BlockID { get; set; }
        public int Transactions { get; set; }
        public string Pool { get; set; } 
    }
}
