using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.CustomerMVC.Models;

public class ProductListViewModel
{
    public PagedList<ProductGeneralVM> ProductPagedList { get; set; } 
        = new PagedList<ProductGeneralVM>();
    public string? SearchString { get; set; }
}
