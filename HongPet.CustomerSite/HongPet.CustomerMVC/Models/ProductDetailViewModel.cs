using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.CustomerMVC.Models;

public class ProductDetailViewModel
{
    public ProductDetailVM ProductDetail { get; set; } = new ProductDetailVM();
    public PagedList<ReviewVM> Reviews { get; set; } = new PagedList<ReviewVM>();

}
