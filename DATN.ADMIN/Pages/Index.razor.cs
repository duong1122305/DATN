﻿using DATN.ADMIN.Components;
using DATN.ADMIN.IServices;
using DATN.Aplication.CustomProvider;
using DATN.ViewModels.DTOs.Statistical;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MudBlazor;
using Syncfusion.Blazor.PdfViewer;

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
		LstDataChart customerData = new LstDataChart();
		LstDataChart revenueDatas = new LstDataChart();
		LstDataChart revenuePieDatas = new LstDataChart();
		List<ProductOutStock> lstProductOutStock = new List<ProductOutStock>();
		List<ProductOutStock> lstProductDataRaw = new List<ProductOutStock>();
		//List<Top3Statistical> top3ProductReven;
		//List<Top3Statistical> top3ServiceReven;
		List<Top3Statistical> top3ProductQuantity;
		List<Top3Statistical> top3ServiceQuantity;
		public double[] dataPie = { 0, 0 };
		public string[] labelsPie = { "Dịch vụ", "Sản phẩm" };
		string width = "99%";
		// bộ lọc
		private MudDateRangePicker _picker;
		private DateRange _dateRange;
		private bool isDisplayDate = false;
		protected override async Task OnInitializedAsync()
		{
			type = 3;
			await LoadData(type);
			StateHasChanged();
			lstProductOutStock = lstProductDataRaw;
        }
        async Task LoadData(int? value = 1)
		{
			DateTime? startDate= DateTime.Now;
			DateTime? endDate= DateTime.Now; 
			if (value==4|| value==5)
			{
				isDisplayDate = true;
				
                if (value==4)
                {
					type = 4;
					return;
                }
				else{
					if (_picker != null && _picker.DateRange != null && _picker.DateRange.Start != null && _picker.DateRange.End != null)
					{
						startDate = _picker.DateRange.Start;
						endDate = _picker.DateRange.End;
						var days = _picker.DateRange.Start.Value.Day;
					}
				}
            }
            else
            {
                isDisplayDate = false;
            }
       //     StateHasChanged();
            if (value == null) return;
			type = value.Value;
			var response = await statiscalClient.StatisticalIndex( startDate.Value.ToString("MM/dd/yyy"),endDate.Value.ToString("MM/dd/yyy"), type);
			if(type==5)type = 4;
            if (response.IsSuccess)
			{
				revenuePieDatas = response.Data.DataPiceRevenue;
				customerData = response.Data.CustomerStatistical;
				revenueDatas = response.Data.RevenueStatistical;
				lstProductDataRaw = response.Data.LstProductOutStock;
				//	top3ProductReven = response.Data.ProductRevenueStatistical;
				// top3ServiceReven = response.Data.ServiceRevenueStatistical;
				top3ProductQuantity = response.Data.ProductQuantityStatistical;
				top3ServiceQuantity = response.Data.ServiceQuantityStatistical;
				width = "99%";
			}
			else
			{
				Snackbar.Add("Chưa có data nhé b!");
			}
			StateHasChanged();
			width = "100%";
           
        }
		async void ChangStatus(int value)
		{

			if (value == -1)
			{
				lstProductOutStock = lstProductDataRaw;
			}
			else if(value == 1)
			{
				lstProductOutStock = lstProductDataRaw.Where(p=>p.Status).ToList();
			}
			else if(value == 0)
			{
				lstProductOutStock = lstProductDataRaw.Where(p=>!p.Status).ToList();
			}
			StateHasChanged();
        }
        // lọc ngày
        private async Task SearchByDate()
        {
            
        }

    }



}
