﻿using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class MobileApiResponse
    {
        public int Id { get; set; }

    }

    public class FileDownloadResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public string Data { get; set; }
    }

    public class CustomerResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public CustomerDto Data { get; set; }
    }

    public class NonCSDResponseNameData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<NonCSDCanteenDto> Data { get; set; }
    }

    public class YearListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int TotalCount { get; set; }
        public IList<YearMonthDto> Data { get; set; }
        public IList<YearDto> YearData { get; set; }
    }

    public class YearMonthDto
    {

        public string YearMonth { get; set; }
        public int Month { get; set; }
        public string Year { get; set; }
        public bool IsDefault { get; set; }
    }

    public class ProductListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<ProductDto> Data { get; set; }
    }

    public class ProductDetailsResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public ProductDto Data { get; set; }
        public IList<ProductDto> SimilarProductData { get; set; }
    }

    public class ProductDetailsRequestData
    {
        public string Id { get; set; }
        public Guid? CustomerId { get; set; }

    }

    public class ProductCategoriesResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int productCount { get; set; }
        //public bool StoreOpenClose { get; set; }
        public IList<ProductCategoryDto> Data { get; set; }
    }

    public class IUDResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }

    }

    public class CartListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<CartDto> Data { get; set; }
    }
    public class CustomerAddressListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<CustomerAddressDto> Data { get; set; }
    }
    public class CustomerAddressResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public CustomerAddressDto Data { get; set; }
    }

    public class WishlistResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<WishlistDto> Data { get; set; }
    }



    public class PaymentCardResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public PaymentCardDto Data { get; set; }
    }

    public class PaymentCardListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<PaymentCardDto> Data { get; set; }
    }

    public class CustomerProfileResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public CustomerDto Data { get; set; }
    }

    public class CustomerOrderListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<SalesOrderDto> Data { get; set; }
    }

    public class ReminderListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<ReminderDto> Data { get; set; }
    }

    public class LogoutResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public string Data { get; set; }
    }



    public class CustomerOrderDetailsResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public SalesOrderDto Data { get; set; }
    }
    public class CustomerOrderSummaryResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public OrderSummary Data { get; set; }
    }

    public class OrderSummary
    {
        public string Price { get; set; }
        public string Discount { get; set; }
        public decimal Items { get; set; }
        public string TotalSaveAmount { get; set; }
        public string DeliveryCharges { get; set; }
    }

    public class OrderNumberResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public string OrderNumber { get; set; }
    }

    public class SendEmailResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
    }

    public class BannerListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<BannerDto> Data { get; set; }
    }

    public class OTPBannerListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<OTPBannerDto> Data { get; set; }
    }
    public class HomePageBannerListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<NoticeDto> TextData { get; set; }
        public string AlertMessage { get; set; }
        public bool StoreOpenClose { get; set; }
        public IList<HomePageBannerDto> Data { get; set; }
    }
    public class NoticeResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public NoticeDto Data { get; set; }
    }

    public class LoginPageBannerListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<LoginPageBannerDto> Data { get; set; }
    }

    public class CategoryBannerListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<CategoryBannerDto> Data { get; set; }
    }

    public class SalesOrderResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public Guid? SalesOrderId { get; set; }
        public bool isOrderDateChanged { get; set; }
    }

    public class CounterResponseNameData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<CounterDto> Data { get; set; }
    }
    public class ProductMainCategoriesResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<ProductMainCategoryDto> Data { get; set; }
    }

    public class FAQResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<FAQDto> Data { get; set; }
    }

    public class StoreOpenCloseResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public ShopHolidayDto Data { get; set; }
    }
    public class HelpAndSupportResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<HelpAndSupportDto> Data { get; set; }
    }

    public class AppVersionResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<AppVersionDto> Data { get; set; }
    }
    public class ExlUploadPurchaseOrderResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public List<PurchaseOrderItemDto> Data { get; set; }
    }

    public class ProductCategoryWiseSalesReportResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public string ProductCategoryName { get; set; }
        public string PurAmount { get; set; }
        public string TotalAmount { get; set; }

        public string OtherPurAmount { get; set; }
        public string OtherTotalAmount { get; set; }

    }
    public class MessageRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string DeviceToken { get; set; }
    }

    public class TotalPaymentsSalesReportResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public string PaymentType { get; set; }
        public string TotalAmount { get; set; }
        public List<PaymentsData> Data { get; set; }

    }

    public class PaymentsData
    {
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }

    }

    public class CounterSalesData
    {
        public string CounterName { get; set; }
        public decimal TotalAmount { get; set; }

    }

    public class DayWiseSummaryReportResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }

        public List<CounterSalesData> CounterSalesData { get; set; }
        public List<PaymentsData> PaymentsData { get; set; }

        public string ProductCategoryName { get; set; }
        public string PurAmount { get; set; }
        public string TotalAmount { get; set; }

        public string OtherPurAmount { get; set; }
        public string OtherTotalAmount { get; set; }

    }

    public class BrandListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<BrandDto> Data { get; set; }
    }

    public class UpdateSODateTimeResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
    }

    public class SupplierResponseData
    {
        public int TotalCount { get; set; }
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<SupplierDto> Data { get; set; }
    }

    public class MobileAppLoginResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public MobileAppLogin Data { get; set; }
    }

    public class MobileAppLogin
    {
        public Guid UserId { get; set; }
        public int Otp { get; set; }
        public string Mobile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleType { get; set; }
    }

    public class MobileAppLoginTokenResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public MobileAppLoginToken Data { get; set; }
    }

    public class MobileAppLoginToken
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class POMSTB
    {
        public Guid? Id { get; set; }
        public string OrderNumber { get; set; }
        public Guid SupplierId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class PurchaseOrderMSTBData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public POMSTB Data { get; set; }
    }

    public class NoDataResponse
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public string[] Data { get; set; }
    }

    public class MSTBPurchaseOrderItemResponseData
    {
        public int Done { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public List<MSTBPurchaseOrderItemDto> Data { get; set; }
    }

    public class MstbSettingListResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<MstbSettingDto> Data { get; set; }
    }

    public class MstbSettingResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public MstbSettingDto Data { get; set; }
    }

    public class MainGRNOrdeData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public PurchaseOrderDto Data { get; set; }
    }
}



