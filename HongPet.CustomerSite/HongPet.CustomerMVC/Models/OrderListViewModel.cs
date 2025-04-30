using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.CustomerMVC.Models;

public class OrderListViewModel
{
    public PagedList<OrderVM> OrderPagedList { get; set; }
        = new PagedList<OrderVM>();
}
