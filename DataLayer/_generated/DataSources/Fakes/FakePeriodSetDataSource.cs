﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Havit.Data.EntityFrameworkCore.Patterns.DataSources.Fakes;
using Havit.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using Havit.Data.Patterns.Attributes;

namespace Havit.Bonusario.DataLayer.DataSources.Fakes;

[Fake]
[System.CodeDom.Compiler.GeneratedCode("Havit.Data.EntityFrameworkCore.CodeGenerator", "1.0")]
public class FakePeriodSetDataSource : FakeDataSource<Havit.Bonusario.Model.PeriodSet>, Havit.Bonusario.DataLayer.DataSources.IPeriodSetDataSource
{
	public FakePeriodSetDataSource(params Havit.Bonusario.Model.PeriodSet[] data)
		: this((IEnumerable<Havit.Bonusario.Model.PeriodSet>)data)
	{			
	}

	public FakePeriodSetDataSource(IEnumerable<Havit.Bonusario.Model.PeriodSet> data, ISoftDeleteManager softDeleteManager = null)
		: base(data, softDeleteManager)
	{
	}
}
