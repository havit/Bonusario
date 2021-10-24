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
	public class EmployeePicker : HxSelectBase<int?, EmployeeReferenceDto>
	{
		[Parameter] public string NullText { get; set; }

		[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }

		public EmployeePicker()
		{
			this.NullableImpl = true;
			this.ValueSelectorImpl = (c => c.EmployeeId);
			this.TextSelectorImpl = (c => c.Name);
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
			var data = await EmployeesDataStore.GetAllAsync();
			this.DataImpl = data.Where(e => !e.IsDeleted);
		}

		protected override async Task OnParametersSetAsync()
		{
			await EnsureDataAsync();

			if (this.Value.HasValue && !this.DataImpl.Any(e => e.EmployeeId == this.Value))
			{
				var appendEmployee = await ResolveItemFromId(this.Value);
				if (appendEmployee != null)
				{
					this.DataImpl = this.DataImpl.Append(appendEmployee).OrderBy(u => u.Name);
				}
				else
				{
					throw new InvalidOperationException("Employee nenalezen.");
				}
			}
			await base.OnParametersSetAsync();
		}

		private async Task<EmployeeReferenceDto> ResolveItemFromId(int? id)
		{
			if (id is null)
			{
				return null;
			}
			return (await EmployeesDataStore.GetByKeyAsync(id.Value));
		}
	}
}
