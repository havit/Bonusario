﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Havit.Bonusario.Model.Common;

public class ApplicationSettings
{
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int Id { get; set; }

	public enum Entry
	{
		Current = -1
	}
}
