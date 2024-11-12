using System.ComponentModel.DataAnnotations.Schema;

namespace FileSystem.ControllerModels
{
    [NotMapped]
    public class ShoppingCartDetails
    {
        public string MobileNo { get; set; }
        public int ShoppingAddressIndex { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public int ClearShoppingCart { get; set; }
        public List<ShoppingProductDetail> ProductDetail { get; set; } = new List<ShoppingProductDetail>();
    }

    [NotMapped]
    public class ShoppingProductDetail
    {
        public string SupplierMobileNo { get; set; }
        public string UserProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SizeLinkId { get; set; }
        public string SizeCode { get; set; }
        public int ColorLinkId { get; set; }
        public string ColorName { get; set; }
        public int Index { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
    }
}
