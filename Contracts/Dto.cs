﻿#pragma warning disable SA1402 // File may only contain a single class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Havit.Bonusario.Contracts;

[ProtoContract]
public class Dto<TValue>
{
	[ProtoMember(1)]
	public TValue Value { get; set; }

	public Dto()
	{
		// NOOP				
	}

	public Dto(TValue value)
	{
		this.Value = value;
	}
}

public static class Dto
{
	public static Dto<TValue> FromValue<TValue>(TValue value)
	{
		return new Dto<TValue>(value);
	}
}
