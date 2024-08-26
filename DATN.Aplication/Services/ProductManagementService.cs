using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.Enum;

namespace DATN.Aplication.Services
{
	public class ProductManagementService : IProductManagementService
	{
		IUnitOfWork _unitOfWork;
		public ProductManagementService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<ResponseData<string>> CreateProduct(CreateProductView productView)
		{
			var check = (from product in await _unitOfWork.ProductRepository.GetAllAsync()
						 where product.Name == productView.Name.Trim().TrimStart().TrimEnd()
						 select product).FirstOrDefault();
			try
			{
				if (check == null)
				{
					var product = new Product()
					{
						IdBrand = productView.IdBrand,
						Name = productView.Name,
						Description = productView.Description,
						IdCategoryDeatail = productView.IdCategoryProduct,
						Status = true,
					};
					await _unitOfWork.ProductRepository.AddAsync(product);
					await _unitOfWork.SaveChangeAsync();
					var img = new ImageProduct()
					{
						ProductID = product.Id,
						UrlImage = productView.ImgUrl,
						ImgKey = productView.ImgID

					};
					await _unitOfWork.ImageProductRepository.AddAsync(img);
					if (productView.lstPD.Count() > 0)
					{
						foreach (var item in productView.lstPD)
						{
							var productDT = new ProductDetail()
							{
								IdProduct = product.Id,
								Name = item.Name,
								Amount = item.Amount,
								Price = item.Price,
								Status = ProductDetailStatus.Stocking,
							};
							await _unitOfWork.ProductDetailRepository.AddAsync(productDT);
						}
						await _unitOfWork.SaveChangeAsync();
						return new ResponseData<string> { IsSuccess = true, Data = "Thêm sản phẩm thành công" };
					}
					return new ResponseData<string> { IsSuccess = false, Error = "Phải có ít nhất 1 biến thể" };
				}
				else
					return new ResponseData<string> { IsSuccess = false, Error = "Tên sản phẩm bị trùng" };
			}
			catch (Exception)
			{
				return new ResponseData<string> { IsSuccess = false, Error = "Lỗi hệ thống! Vui lòng liên hệ nhà phát triển" };
			}
		}
		public async Task<ResponseData<string>> UpdateProduct(CreateProductView productView)
		{
			try
			{
				var product = (from pro in await _unitOfWork.ProductRepository.GetAllAsync()
							   where pro.Id == productView.Id
							   select pro).FirstOrDefault();
				var checkdup = from pro in await _unitOfWork.ProductRepository.GetAllAsync()
							   where pro.Name == productView.Name.Trim().TrimStart().TrimEnd()
							   select pro;
				if (checkdup.Count() > 0)
				{
					if (product.Id == checkdup.FirstOrDefault().Id)
					{
						product.Name = productView.Name;
						product.Description = productView.Description;
						product.IdBrand = productView.IdBrand;
						product.IdCategoryDeatail = productView.IdCategoryProduct;
						await _unitOfWork.ProductRepository.UpdateAsync(product);

						var imgPro = await _unitOfWork.ImageProductRepository.FindAsync(p => p.ProductID == product.Id);
						if (imgPro == null || !imgPro.Any())
						{
							var newImg = new ImageProduct
							{
								ProductID = product.Id,
								ImgKey = productView.ImgID,
								UrlImage = productView.ImgUrl,

							};
							await _unitOfWork.ImageProductRepository.AddAsync(newImg);
						}
						else
						{
							var imgU = imgPro.First();
							imgU.ImgKey = productView.ImgID;
							imgU.UrlImage = productView.ImgUrl;
							await _unitOfWork.ImageProductRepository.UpdateAsync(imgU);
						}
						await _unitOfWork.SaveChangeAsync();
						return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
					}
					else
						return new ResponseData<string> { IsSuccess = false, Error = "Tên loại sản phẩm trùng với loại sản phẩm đã có" };
				}
				else
				{
					product.Name = productView.Name;
					product.Description = productView.Description;
					product.IdBrand = productView.IdBrand;
					product.IdCategoryDeatail = productView.IdCategoryProduct;
					await _unitOfWork.ProductRepository.UpdateAsync(product);
					await _unitOfWork.SaveChangeAsync();
					return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
				}

			}
			catch (Exception)
			{
				return new ResponseData<string> { IsSuccess = false, Error = "Lỗi hệ thống! Vui lòng liên hệ nhà phát triển" };
			}
		}
		public async Task<ResponseData<string>> RemoveProduct(int id)
		{
			try
			{
				var product = (from cate in await _unitOfWork.ProductRepository.GetAllAsync()
							   where cate.Id == id
							   select cate).FirstOrDefault();
				if (product != null)
				{
					product.Status = false;
					await _unitOfWork.ProductRepository.UpdateAsync(product);
					await _unitOfWork.SaveChangeAsync();
					return new ResponseData<string> { IsSuccess = true, Data = "Cập nhật trạng thái thành công" };
				}
				else
					return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy sản phẩm cần cập nhật" };

			}
			catch (Exception)
			{
				return new ResponseData<string> { IsSuccess = false, Error = "Lỗi hệ thống! Vui lòng liên hệ nhà phát triển" };
			}
		}
		public async Task<ResponseData<string>> ActiveProduct(int id)
		{
			try
			{
				var product = (from cate in await _unitOfWork.ProductRepository.GetAllAsync()
							   where cate.Id == id
							   select cate).FirstOrDefault();
				if (product != null)
				{
					product.Status = true;
					await _unitOfWork.ProductRepository.UpdateAsync(product);
					await _unitOfWork.SaveChangeAsync();
					return new ResponseData<string> { IsSuccess = true, Data = "Cập nhật trạng thái thành công" };
				}
				else
					return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy sản phẩm cần cập nhật" };

			}
			catch (Exception)
			{
				return new ResponseData<string> { IsSuccess = false, Error = "Lỗi hệ thống! Vui lòng liên hệ nhà phát triển" };
			}
		}
		public async Task<ResponseData<ImageProduct>> GetImgByProduct(int idProduct)
		{
			try
			{
				var img = await _unitOfWork.ImageProductRepository.FindAsync(p => p.ProductID == idProduct);
				if (img == null | !img.Any())
				{
					return new ResponseData<ImageProduct>(false, "Không có ảnh");
				}
				else
				{
					return new ResponseData<ImageProduct>(img.First());
				}
			}
			catch (Exception)
			{

				return new ResponseData<ImageProduct>(false, "Lỗi lấy ảnh");
			}
		}
		public async Task<ResponseData<List<ProductView>>> ListProduct()
		{
			var productDT = await _unitOfWork.ProductDetailRepository.FindAsync(p => p.Status == ProductDetailStatus.Stocking);

			var query = from product in await _unitOfWork.ProductRepository.GetAllAsync()
						join brand in await _unitOfWork.BrandRepository.GetAllAsync()
						on product.IdBrand equals brand.Id
						join img in await _unitOfWork.ImageProductRepository.GetAllAsync()
						on product.Id equals img.ProductID
						join cd in await _unitOfWork.CategoryDetailRepository.GetAllAsync()
						on product.IdCategoryDeatail equals cd.Id
						join c in await _unitOfWork.CategoryRepository.GetAllAsync()
						on cd.IdCategory equals c.Id
						select new ProductView()
						{
							Id = product.Id,
							Brand = brand.Name,
							Name = product.Name,
							CategoryProduct = cd.Name,
							Description = product.Description,
							Status = product.Status,
							CategoryProductId = cd.Id,
							IdBrand = brand.Id,
							Url = img.UrlImage,
							IdImg = img.ImgKey,
							Price = product.ProductDetails != null && product.ProductDetails.Count() >= 2 ? product.ProductDetails.Min(x => x.Price).ToString("N0") + "- " + product.ProductDetails.Max(x => x.Price).ToString("N0") : product.ProductDetails != null && product.ProductDetails!.Count() > 0 ? product.ProductDetails!.First().Price.ToString("N0") : "Không có biến thể nào sẵn sàng"

						};
			if (query.Count() > 0)
				return new ResponseData<List<ProductView>> { IsSuccess = true, Data = query.OrderByDescending(p => p.Id).ToList() };
			else
				return new ResponseData<List<ProductView>> { IsSuccess = false, Error = "Chưa có dữ liệu", Data = new List<ProductView>() };
		}
		public async Task<ResponseData<ProductClient>> GetProductByID(int id)
		{
			try
			{
				var lstPD = await _unitOfWork.ProductDetailRepository.GetAllAsync();
				var lstP = await _unitOfWork.ProductRepository.GetAllAsync();
				var lstIMG = await _unitOfWork.ImageProductRepository.GetAllAsync();
				var lstBrand = await _unitOfWork.BrandRepository.GetAllAsync();
				var lstCateDetail = await _unitOfWork.CategoryDetailRepository.GetAllAsync();

				var query = from product in lstP
							join brand in lstBrand
							on product.IdBrand equals brand.Id
							join img in  lstIMG
							 on product.Id equals img.ProductID
							join cd in  lstCateDetail 
							on product.IdCategoryDeatail equals cd.Id
							where product.Id == id && product.Status
							select new ProductClient()
							{
								Id = product.Id,
								BrandDescription = brand.Description,
								BrandName = brand.Name,
								CateDetailName = cd.Name,
								ImgUrl = img.UrlImage,
								NameProduct = product.Name,
								ProductDescription = product.Description,
								ListProductDetail = lstPD.Where(p => p.Id == id && p.Status != ProductDetailStatus.Deleted).Select(p => new ProductDetailClient() { Name = p.Name, ID = p.Id, Price = p.Price.ToString("N0") }).ToList(),
							};
				if (!query.Any())
				{
					return new ResponseData<ProductClient>(false,"Không có thông tin sản phẩm");

				}
				return new ResponseData<ProductClient>(query.ToList().First());

			}
			catch (Exception ex)
			{

				return new ResponseData<ProductClient>(false, "Lỗi trong quá trinh xử lý: " + ex);
			}
		}

	}
}

