﻿using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.ADMIN.Services
{
    public class VoucherServices : IVoucherServices
    {
        private readonly HttpClient _client;

        public VoucherServices(HttpClient client)
        {
            _client = client;
        }

        public async Task<ResponseData<string>> ChangeStatusVoucher(int id)
        {
            var respone = await _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/Change-Status-Voucher?id={id}");
            //var result = await respone.Content.ReadFromJsonAsync<ResponseData<string>>();
            return respone;
        }

        public async Task<ResponseData<string>> CreateVoucher(VoucherView voucherView)
        {
            var respone = await _client.PostAsJsonAsync("api/UserLogin/Create-Voucher", voucherView);
            var result = await respone.Content.ReadFromJsonAsync<ResponseData<string>>();
            return result;
        }

        public async Task<ResponseData<List<VoucherView>>> GetAll()
        {
            var repon = await _client.GetFromJsonAsync<ResponseData<List<VoucherView>>>("api/UserLogin/List-Voucher");
            return repon;
        }

        public async Task<ResponseData<string>> UpdateVoucher(VoucherView voucherView)
        {
            var respone = await _client.PutAsJsonAsync<VoucherView>($"api/UserLogin/Update-Voucher", voucherView);
            var result = await respone.Content.ReadFromJsonAsync<ResponseData<string>>();
            return result;
        }
    }
}
