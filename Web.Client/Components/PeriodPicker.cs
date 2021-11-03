using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Components
{
	public class PeriodPicker : HxSelectBase<int?, PeriodDto>
	{
		[Parameter] public string NullText { get; set; }

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

		[Inject] protected IPeriodsDataStore PeriodsDataStore { get; set; }

		public PeriodPicker()
		{
			this.ValueSelectorImpl = (c => c.PeriodId);
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
			this.DataImpl ??= (await PeriodsDataStore.GetAllAsync()).OrderBy(p => p.EndDate);
		}

		protected override async Task OnParametersSetAsync()
		{
			await EnsureDataAsync();
			if (this.Value.HasValue && !this.DataImpl.Any(e => e.PeriodId == this.Value))
			{
				var appendPeriod = await ResolveItemFromId(this.Value);
				if (appendPeriod != null)
				{
					this.DataImpl = this.DataImpl.Append(appendPeriod).OrderBy(u => u.Name);
				}
				else
				{
					throw new InvalidOperationException("Period nenalezen.");
				}
			}
			await base.OnParametersSetAsync();
		}

		private async Task<PeriodDto> ResolveItemFromId(int? id)
		{
			if (id is null)
			{
				return null;
			}
			return (await PeriodsDataStore.GetByKeyAsync(id.Value));
		}
	}
}
