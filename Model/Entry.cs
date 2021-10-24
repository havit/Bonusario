using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Bonusario.Model
{
	public class Entry
	{
		public int Id { get; set; }

		public Employee CreatedBy { get; set; }
		public int CreatedById { get; set; }

		public Employee Recipient { get; set; }
		public int RecipientId { get; set; }

		[MaxLength]
		public string Text { get; set; }

		public int Value { get; set; }

		public List<EntryTag> Tags { get; set; }
	}
}
