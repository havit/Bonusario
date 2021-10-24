using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Data.EntityFrameworkCore.Attributes;

namespace Havit.Bonusario.Model
{
	[Cache]
	public class Period
	{
		public int Id { get; set; }

		[MaxLength(200)]
		public string Name { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public DateTime Created { get; set; }
	}
}
