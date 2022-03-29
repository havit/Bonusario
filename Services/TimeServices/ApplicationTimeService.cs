﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Havit.Services.TimeServices;

namespace Havit.Bonusario.Services.TimeServices;

/// <summary>
/// Provides current time in local time-zone ("Central Europe Standard Time", "Europe/Prague" for non-Windows platforms).
/// </summary>
public class ApplicationTimeService : TimeZoneTimeServiceBase, ITimeService
{
	/// <summary>
	/// Returns time-zone you want to treat as local ("Central Europe Standard Time", "Europe/Prague" for non-Windows platforms).
	/// </summary>
	protected override TimeZoneInfo CurrentTimeZone
	{
		get
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
			}
			return TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague"); // MacOS
		}
	}
}
