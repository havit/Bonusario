using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Components;

public class EntryTagsPicker : HxInputTags
{
	public override async Task SetParametersAsync(ParameterView parameters)
	{
		await base.SetParametersAsync(parameters);

		DataProvider = GetTagSuggestions;
		SuggestDelay = 0;
		SuggestMinimumLength = 0;
		Naked = parameters.GetValueOrDefault(nameof(Naked), true);
		ShowAddButton = true;
		AddButtonText = "tag";
	}

	private List<string> preconfiguredTags = new List<string>()
	{
		"důvěra",
		"férovost",
		"nezávislost",
		"potěšení",
		"přátelskost",
		"radost",
		"stabilita",
		"team",
		"vášeň",
		"vzdělávání",
		"znalosti",
	};
	private Task<InputTagsDataProviderResult> GetTagSuggestions(InputTagsDataProviderRequest request)
	{
		return Task.FromResult(new InputTagsDataProviderResult()
		{
			Data = preconfiguredTags.Select(v => v.ToString()).Where(v => !this.Value.Contains(v))
		});
	}

}
