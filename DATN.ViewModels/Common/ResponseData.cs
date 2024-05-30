﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Common
{
	public class ResponseData<T>
	{
		public bool IsSuccess { get; set; }
		public string? Error { get; set; }
		public T? Data { get; set; }
	}

}
