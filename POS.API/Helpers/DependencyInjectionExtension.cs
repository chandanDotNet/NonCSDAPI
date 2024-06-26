﻿using POS.Common.UnitOfWork;
using POS.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace POS.API.Helpers
{
    public static class DependencyInjectionExtension
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<IPropertyMappingService, PropertyMappingService>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserClaimRepository, UserClaimRepository>();
            services.AddScoped<IRoleClaimRepository, RoleClaimRepository>();
            services.AddScoped<ILoginAuditRepository, LoginAuditRepository>();
            services.AddScoped<INLogRepository, NLogRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IEmailSMTPSettingRepository, EmailSMTPSettingRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<INewsletterSubscriberRepository, NewsletterSubscriberRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ITestimonialsRepository, TestimonialsRepository>();
            services.AddScoped<IReminderNotificationRepository, ReminderNotificationRepository>();
            services.AddScoped<IReminderRepository, ReminderRepository>();
            services.AddScoped<IReminderUserRepository, ReminderUserRepository>();
            services.AddScoped<IReminderSchedulerRepository, ReminderSchedulerRepository>();
            services.AddScoped<IDailyReminderRepository, DailyReminderRepository>();
            services.AddScoped<IQuarterlyReminderRepository, QuarterlyReminderRepository>();
            services.AddScoped<IHalfYearlyReminderRepository, HalfYearlyReminderRepository>();
            services.AddScoped<ISendEmailRepository, SendEmailRepository>();
            // PO
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IPurchaseOrderItemRepository, PurchaseOrderItemRepository>();
            services.AddScoped<IPurchaseOrderPaymentRepository, PurchaseOrderPaymentRepository>();
            //SO
            services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
            services.AddScoped<ISalesOrderItemRepository, SalesOrderItemRepository>();
            services.AddScoped<ISalesOrderItemTaxRepository, SalesOrderItemTaxRepository>();
            services.AddScoped<ISalesOrderPaymentRepository, SalesOrderPaymentRepository>();

            services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();
            services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductTaxRepository, ProductTaxRepository>();
            services.AddScoped<ITaxRepository, TaxRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IInquiryRepository, InquiryRepository>();
            services.AddScoped<IInquiryProductRepository, InquiryProductRepository>();
            services.AddScoped<IInquiryStatusRepository, InquiryStatusRepository>();
            services.AddScoped<IInquiryNoteRepository, InquiryNoteRepository>();
            services.AddScoped<IInquiryAttachmentRepository, InquiryAttachmentRepository>();
            services.AddScoped<IInquiryActivityRepository, InquiryActivityRepository>();
            services.AddScoped<IInquirySourceRepository, InquirySourceRepository>();

            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICounterRepository, CounterRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IInventoryHistoryRepository, InventoryHistoryRepository>();

            services.AddScoped<IInventoryHistoryRepository, InventoryHistoryRepository>();
            services.AddScoped<IUnitConversationRepository, UnitConversationRepository>();
            services.AddScoped<IWarehouseInventoryRepository, WarehouseInventoryRepository>();
            services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IPaymentCardRepository, PaymentCardRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<ILoginPageBannerRepository, LoginPageBannerRepository>();
            services.AddScoped<INonCSDCanteenRepository, NonCSDCanteenRepository>();
            services.AddScoped<IProductMainCategoryRepository, ProductMainCategoryRepository>();
            services.AddScoped<IFAQRepository, FAQRepository>();
            services.AddScoped<IHelpAndSupportRepository, HelpAndSupportRepository>();
            services.AddScoped<IGRNRepository, GRNRepository>();
            // services.AddScoped<IGRNItemRepository, GRNItemRepository>();
            services.AddScoped<IAppVersionRepository, AppVersionRepository>();
            services.AddScoped<ISupplierDocumentRepository, SupplierDocumentRepository>();
            services.AddScoped<IBatchRepository, BatchRepository>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<ICategoryBannerRepository, CategoryBannerRepository>();
            services.AddScoped<IHomePageBannerRepository, HomePageBannerRepository>();
            services.AddScoped<INoticeRepository, NoticeRepository>();
            services.AddScoped<IPackagingRepository, PackagingRepository>();
            services.AddScoped<IOTPBannerRepository, OTPBannerRepository>();
            services.AddScoped<IShopHolidayRepository, ShopHolidayRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IYearRepository, YearRepository>();
            services.AddScoped<IMSTBPurchaseOrderRepository, MSTBPurchaseOrderRepository>();
            services.AddScoped<IMSTBPurchaseOrderItemRepository, MSTBPurchaseOrderItemRepository>();
            services.AddScoped<IMSTBPurchaseOrderItemTaxRepository, MSTBPurchaseOrderItemTaxRepository>();
            services.AddScoped<IMstbSettingRepository, MstbSettingRepository>();
            services.AddScoped<IUserSupplierRepository, UserSupplierRepository>();
        }
    }
}
