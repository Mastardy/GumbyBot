using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GumbyBot.Models
{
	public class Currency
	{
		// Discord ID
		[Required]
		[Key]
		public ulong Id { get; set; }
		// How much money they have
		[Required]
		public ulong Amount { get; set; }

		public ulong TakeMoney(ulong amount)
		{
			if (amount > Amount) Amount = 0;
			else Amount -= amount;
			return Amount;
		}

		public ulong GiveMoney(ulong amount)
		{
			ulong amountTillMax = ulong.MaxValue - Amount;
			if (amount > amountTillMax) Amount = ulong.MaxValue;
			else Amount += amount;
			return Amount;
		}
	}
}
