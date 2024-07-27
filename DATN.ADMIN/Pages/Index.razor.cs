using DATN.ADMIN.IServices;
using DATN.Aplication.CustomProvider;
using DATN.ViewModels.DTOs.Statistical;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using MudBlazor;

namespace DATN.ADMIN.Pages
{
	public partial class Index
	{
		[Inject]
		protected IHttpContextAccessor _httpContextAccessor { get; set; }
		[Inject]
		protected CustomAuthenticationStateProvider _customAuthenticationStateProvider { get; set; }
		[Inject]
		protected IStatiscalClient statiscalClient { get; set; }
		[Inject]
		protected ISnackbar Snackbar { get; set; }

		int type = 1;
		List<Top3Statistical> top3ProductReven;
		List<Top3Statistical> top3ServiceReven;
		List<Top3Statistical> top3ProductQuantity;
		List<Top3Statistical> top3ServiceQuantity;
		public double[] dataPie = { 0, 0 };
		public string[] labelsPie = { "Dịch vụ", "Sản phẩm" };
		public List<ChartSeries> Series = new List<ChartSeries>();
		public List<string> Labelsbar = new List<string>();

		protected override async Task OnInitializedAsync()
		{
			type = 3;
			await LoadData(type);
			StateHasChanged();

		}
		async Task LoadData(int? value = 1)
		{
			if (value == null) return;
			var oldType = type;
			type = value.Value;
			var response = await statiscalClient.StatisticalIndex(type);
			if (response.IsSuccess&& response.Data.ProductRevenueStatistical.Count()>0)
			{
				dataPie = response.Data.DataPiceRevenue;
				Labelsbar = response.Data.CustomerStatistical.Label;
				Series.Clear();
				Series.Add(
					new ChartSeries() { Name = response.Data.CustomerStatistical.Name, Data = response.Data.CustomerStatistical.Amount.ToArray() }
					);
				top3ProductReven = response.Data.ProductRevenueStatistical;
				top3ServiceReven = response.Data.ServiceRevenueStatistical;
				top3ProductQuantity = response.Data.ProductQuantityStatistical;
				top3ServiceQuantity = response.Data.ServiceQuantityStatistical;
			}
            else
            {
				Snackbar.Add("Chưa có data nhé b!");
				type=oldType;
            }
            StateHasChanged();

		}
	}
}
