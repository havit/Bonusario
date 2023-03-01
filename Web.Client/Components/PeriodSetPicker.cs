using Havit.Bonusario.Web.Client.DataStores;

namespace Havit.Bonusario.Web.Client.Components;

public class PeriodSetPicker : HxSelectBase<int?, PeriodSetDto>
{
	[Parameter] public string NullText { get; set; }

#pragma warning disable BL0007 // Component parameter should be auto-property
	[Parameter]
	public bool? Nullable
	{
		get
		{
			return NullableImpl;
		}
		set
		{
			this.NullableImpl = value;
		}
	}
#pragma warning restore BL0007 // Component parameter should be auto-property

	[Inject] protected IPeriodSetsDataStore PeriodSetsDataStore { get; set; }

	public PeriodSetPicker()
	{
		this.ValueSelectorImpl = (c => c.Id);
		this.TextSelectorImpl = (c => c.Name);
		this.AutoSortImpl = false;
	}

	protected override async Task OnInitializedAsync()
	{
		this.NullTextImpl ??= NullText ?? "-vyberte-";
		this.NullDataTextImpl ??= "...načítám...";

		await base.OnInitializedAsync();
		await EnsureDataAsync();
	}

	private async Task EnsureDataAsync()
	{
		if (this.DataImpl is null)
		{
			this.DataImpl ??= (await PeriodSetsDataStore.GetAllAsync()).OrderByDescending(p => p.Name);
		}
	}

	protected override async Task OnParametersSetAsync()
	{
		await EnsureDataAsync();
		if (this.Value.HasValue && !this.DataImpl.Any(e => e.Id == this.Value))
		{
			var appendPeriodSet = await ResolveItemFromId(this.Value);
			if (appendPeriodSet != null)
			{
				this.DataImpl = this.DataImpl.Append(appendPeriodSet).OrderByDescending(u => u.Name);
			}
			else
			{
				throw new InvalidOperationException("PeriodSet nenalezen.");
			}
		}
		await base.OnParametersSetAsync();
	}

	private async Task<PeriodSetDto> ResolveItemFromId(int? id)
	{
		if (id is null)
		{
			return null;
		}
		return (await PeriodSetsDataStore.GetByKeyAsync(id.Value));
	}
}
