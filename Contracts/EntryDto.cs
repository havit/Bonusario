using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Bonusario.Contracts
{
	public class EntryDto
	{
		public int Id { get; set; }

		public int? CreatedById { get; set; }

		public int? RecipientId { get; set; }

		public string Text { get; set; }

		public int Value { get; set; }

		public List<string> Tags { get; set; }

		public DateTime? Submitted { get; set; }
		public DateTime? Created { get; set; }
		public int PeriodId { get; set; }
	}
}
