using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadInventoryFunc
{
    public class _35
    {
        [JsonProperty("136")]
        public string _136 { get; set; }

        [JsonProperty("137")]
        public string _137 { get; set; }

        [JsonProperty("138")]
        public string _138 { get; set; }

        [JsonProperty("139")]
        public string _139 { get; set; }

        [JsonProperty("140")]
        public string _140 { get; set; }
    }

    public class AdsConfig
    {
        public DynamicPricing dynamicPricing { get; set; }
    }

    public class CategoriesConfig
    {
        public List<string> restrictedCategories { get; set; }
        public OnDemandBlockConfig onDemandBlockConfig { get; set; }
    }

    public class Contact
    {
        public string name { get; set; }
        public string mobileNumber { get; set; }
        public string contactType { get; set; }
        public string email { get; set; }
        public string sellerId { get; set; }
    }

    public class ContainerOrdering
    {
        [JsonProperty("column-1")]
        public List<string> column1 { get; set; }

        [JsonProperty("column-2")]
        public List<string> column2 { get; set; }

        [JsonProperty("column-3")]
        public List<string> column3 { get; set; }
    }

    public class ContextualHelpConfig
    {
        public List<Route> routes { get; set; }
    }

    public class DynamicPricing
    {
        public int prefillRoiPercent { get; set; }
        public int warnRoiPercent { get; set; }
        public int warnRoiMinValue { get; set; }
    }

    public class Events
    {
        public SellerDashboardPerformance seller_dashboard_performance { get; set; }
    }

    public class Fdp
    {
        public string @namespace { get; set; }
        public Events events { get; set; }
    }

    public class Feature
    {
        public bool reports { get; set; }
        public bool singleProductListing { get; set; }
        public bool orderManagement { get; set; }
    }

    public class FlipkartContact
    {
        public string address { get; set; }
    }

    public class GaRevampConfig
    {
        public List<string> spfNodeId { get; set; }
        public string tdsNodeId { get; set; }
        public List<string> multiMediaNodeId { get; set; }
        public bool isSelfCloseEnabledByUI { get; set; }
        public bool isSPFEnabledForHyperlocal { get; set; }
        public int RCBbeforeHour { get; set; }
        public int RCBbeforeMinute { get; set; }
        public int RCBafterHour { get; set; }
        public int RCBafterMinute { get; set; }
        public List<string> spfRedirectionEnabledNodes { get; set; }
        public List<string> createTicketDisabledNodes { get; set; }
    }

    public class GlobalConfigStr
    {
        public string googleAnalyticsId { get; set; }
        public string kissmetricsId { get; set; }
        public string gaUid { get; set; }
    }

    public class GstMetadata
    {
        public string govtUrl { get; set; }
        public string clearTaxIcon { get; set; }
        public string partnerService { get; set; }
        public string clearTaxUrl1 { get; set; }
        public string clearTaxUrl2 { get; set; }
        public string clearTaxUrl3 { get; set; }
        public string taxClient { get; set; }
        public string taxBuddyIcon { get; set; }
        public string taxBuddyText { get; set; }
        public string vakilSearchIcon { get; set; }
        public string vakilSearchText { get; set; }
    }

    public class GuidedAssistance
    {
        public FlipkartContact flipkartContact { get; set; }
    }

    public class HomePage
    {
        public ContainerOrdering containerOrdering { get; set; }
        public MarketingConfig marketingConfig { get; set; }
    }

    public class ImportantNoticeMessage
    {
        public string message { get; set; }
        public string link { get; set; }
    }

    public class LoginContext
    {
        public string seller_id { get; set; }
        public string display_name { get; set; }
        public string business_name { get; set; }
        public bool on_behalf { get; set; }
        public List<object> roles { get; set; }
    }

    public class MarketingConfig
    {
        public bool showCard { get; set; }
        public string imageSrc { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string imageUrl { get; set; }
    }

    public class MultiTenancyVersions
    {
        [JsonProperty("common-multi-tenancy")]
        public string commonmultitenancy { get; set; }
    }

    public class Onboarding
    {
        public int categorySizeLimit { get; set; }
    }

    public class OnDemandBlockConfig
    {
        public bool block { get; set; }
        public string errorMessage { get; set; }
        public List<string> categories { get; set; }
    }

    public class QuestionOrderMapping
    {
        [JsonProperty("35")]
        public _35 _35 { get; set; }

        [JsonProperty("36")]
        public string _36 { get; set; }

        [JsonProperty("37")]
        public string _37 { get; set; }

        [JsonProperty("38")]
        public string _38 { get; set; }

        [JsonProperty("39")]
        public string _39 { get; set; }

        [JsonProperty("40")]
        public string _40 { get; set; }

        [JsonProperty("41")]
        public string _41 { get; set; }
    }

    public class RateCard
    {
        public bool shouldShowDatePickerCheckBox { get; set; }
        public string effectiveDate { get; set; }
        public string effectiveDateText { get; set; }
    }

    public class ReqCallBackOnboarding
    {
        public int endTime { get; set; }
        public int startTime { get; set; }
        public int breakTime { get; set; }
    }

    public class RolesToDescription
    {
        public List<string> PLAdsManager { get; set; }
        public List<string> CatalogManager { get; set; }
    }

    public class Root
    {
        public bool isWebView { get; set; }
        public string sellerType { get; set; }
        public string chatStatus { get; set; }
        public bool enableAlerts { get; set; }
        public string sellerId { get; set; }
        public SellerDetails sellerDetails { get; set; }
        public string csrfToken { get; set; }
        public bool allLinksEnabled { get; set; }
        public string featureConfig { get; set; }
        public int redirectState { get; set; }
        public string sellerIdQuery { get; set; }
        //public Feature feature { get; set; }
        public bool isDcsCallFailed { get; set; }
        public bool isOnBehalf { get; set; }
        public string onBehalfEmail { get; set; }
        public string onBehalfHomePage { get; set; }
        public bool enableDropShipment { get; set; }
        public bool isDropShipmentEnabled { get; set; }
        public bool isMPSSeller { get; set; }
        public bool autoInvoicing { get; set; }
        public string googleAnalyticsId { get; set; }
        public bool fbfEnabled { get; set; }
        public bool enablePayments { get; set; }
        public bool enableOldReturns { get; set; }
        public bool enableRTM { get; set; }
        public bool enableStaticHomepage { get; set; }
        //public GlobalConfigStr globalConfigStr { get; set; }
        public int chatCategory { get; set; }
        public string chatUrl { get; set; }
        public string ssChannelName { get; set; }
        public bool onboardingDownTime { get; set; }
        public string gaLabelPrefix { get; set; }
        public string ssContactNumber { get; set; }
        //public List<string> shortCircuitedIssues { get; set; }
        //public List<string> hideUploader { get; set; }
        //public RolesToDescription rolesToDescription { get; set; }
        //public List<LoginContext> loginContexts { get; set; }
        public int numberOfUsersAllowed { get; set; }
        //public List<object> whiteListedGACategory { get; set; }
        //public List<string> whiteListedKMCategory { get; set; }
        //public ContextualHelpConfig contextualHelpConfig { get; set; }
        //public UiConfig uiConfig { get; set; }
        public string commissionCalculatorFutureDate { get; set; }
        public string gstCommissionCalculatorFutureDate { get; set; }
        //public CategoriesConfig categoriesConfig { get; set; }
        public bool enableSIR { get; set; }
        public string sbcUrl { get; set; }
        //public GstMetadata gstMetadata { get; set; }
        //public ReqCallBackOnboarding reqCallBackOnboarding { get; set; }
        //public MultiTenancyVersions multiTenancyVersions { get; set; }
        public bool enableFBFLite { get; set; }
        public bool isLockdownException { get; set; }
        public bool enableFBFSeller { get; set; }
        public string maskedSellerId { get; set; }
        public bool enableMixpanel { get; set; }
        public string bssAdvertisementAppUrl { get; set; }
        public string userId { get; set; }
        public bool enableFbfLiteOrderProcessing { get; set; }
        public bool enableFbfLiteInventoryManagement { get; set; }
        public bool shouldMoveToFbflite { get; set; }
        public bool enableLogisticsAutomation { get; set; }
        public bool enableAdsOutage { get; set; }
        public bool enableSchedulingV2 { get; set; }
        public bool enableManageListings { get; set; }
        public bool enableLiveChat { get; set; }
        public bool enableOptinFeature { get; set; }
        public bool enableSubmitChanges { get; set; }
        public bool enablePromoOfferDetailsV2 { get; set; }
        public bool enableWalletManager { get; set; }
        public bool enableRestwaySelfshipReturns { get; set; }
        public bool enableMultilocationV2 { get; set; }
        public bool enableMPSinUIOrders { get; set; }
        public bool enableUIVStockUpdate { get; set; }
        public bool enableBeta { get; set; }
        public bool enableSessionManagementUI { get; set; }
        public bool enableStockSuggestion { get; set; }
        public bool enablePriceAutomation { get; set; }
        public bool enableGCRevamp { get; set; }
        public bool enablePaidAccountManagement { get; set; }
        public bool enableListingsDowntime { get; set; }
        public bool enableNextPaymentUI { get; set; }
        public bool enablePartnerServices { get; set; }
        public bool enablePEHomePage { get; set; }
        public bool enableMultiPutlist { get; set; }
        public bool enablePictorUI { get; set; }
        public bool enableUIVStockUpdateV2 { get; set; }
        public bool enableBulkEditUI { get; set; }
        public bool enablePrebook { get; set; }
        public bool enableVCUI { get; set; }
        public bool disableExpiredNavigationTab { get; set; }
        public bool enableOmsMigration { get; set; }
        public bool enableAutoQCUI { get; set; }
        public bool enablePaymentV2 { get; set; }
        public bool enableEditListingsV2 { get; set; }
        public bool enableRcbPilot { get; set; }
        public bool enableSmartSellerWeekend { get; set; }
        public bool enableSmartInventoryRecallV2 { get; set; }
        public bool enableProductTitlePreview { get; set; }
        public bool enableTRI3D { get; set; }
        public bool enableBulkVariantGrouping { get; set; }
        public bool enableSPFV2 { get; set; }
        public bool enableReportCentreV2 { get; set; }
        public bool enableTransactionsV2 { get; set; }
        public bool enableShopsyWidget { get; set; }
        public bool enableManageVariants { get; set; }
        public bool enableMultiPickUpOrders { get; set; }
        public bool enableConsignmentSlotAdjustment { get; set; }
        public bool enableNonFBFThermalPrinter { get; set; }
        public bool enableNonFBFThermalPrinterBulkOption { get; set; }
        public bool enableNewFbfRecall { get; set; }
        public bool enablePriceRecoHighImpactTag { get; set; }
        public bool enableUnifiedAdvertising { get; set; }
        public bool enablePlatformSwitch { get; set; }
        public bool autoEInvoiceOnboarding { get; set; }
        public bool cartManValidation { get; set; }
        public bool enableDedicatedSpaceWidget { get; set; }
        public bool disableCreateConsignmentFlow { get; set; }
        public bool enableTrackMyApprovalV2 { get; set; }
        public bool enableTrendingDesignRecommendation { get; set; }
        public bool enableDashboardV2 { get; set; }
        public bool multiFBFLocation { get; set; }
        public bool enableNewLabelTemplateForShipping { get; set; }
        public bool enableFBFUnifiedInventory { get; set; }
        public bool enableSellerRecap { get; set; }
        public bool enableHelpAndSupport { get; set; }
        public bool enableSelectionInsightsJunoApproval { get; set; }
        public bool enebleFBFBenefitsModal { get; set; }
        public bool enableGrowthPriceRecoOnListings { get; set; }
        public bool enableSelectionInsightsV3MSKU { get; set; }
        public bool enableIHPriceAdsReco { get; set; }
        public bool enableFBFOnboarding { get; set; }
        public bool enableVideoCommListings { get; set; }
        public bool prevTransactionRevamp { get; set; }
        public bool enablePaymentContextualForm { get; set; }
        public bool enableDispatchBreach { get; set; }
        public bool enableReverifyBank { get; set; }
        public bool enableLQSDetails { get; set; }
        public bool enablePriceRecoFileUpload { get; set; }
        public bool enableHelpSection { get; set; }
        public bool enableReturnsReco { get; set; }
        public bool enableRevampedRecosOnGrowthDetails { get; set; }
        public bool enableWelcomeTab { get; set; }
        public bool enableListingTab { get; set; }
        public bool enableOrders { get; set; }
        public bool enableReturns { get; set; }
        public bool enableMetrics { get; set; }
        public bool enableManageProfile { get; set; }
        public bool enableFlipkartPromotions { get; set; }
        public bool enableAddListingInBulk { get; set; }
        public bool enableAddAListing { get; set; }
        public bool enableMyListings { get; set; }
        public bool enableTrackApprovalRequests { get; set; }
        public bool enableBrandApprovalRequests { get; set; }
        public bool enableImageUpload { get; set; }
        public bool enableAddExistingProductListing { get; set; }
        public bool enableAddNewProductListing { get; set; }
        public bool enableViewNonLiveQCVerified { get; set; }
        public bool enableEditNonLiveQCFailed { get; set; }
        public bool enableEditNonLiveQCPending { get; set; }
        public bool enableEditNonLiveDraft { get; set; }
        public bool enableViewLiveListings { get; set; }
        public bool enableSellerLearningCenter { get; set; }
        public bool enableCancelOrders { get; set; }
        public bool enablePriceApprovals { get; set; }
        public bool enableAlphaSellerApprovals { get; set; }
        public bool enableExclusivePromotions { get; set; }
    }

    public class Route
    {
        public string key { get; set; }
        public Value value { get; set; }
        public bool state { get; set; }
    }

    public class SellerDashboardPerformance
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class SellerDetails
    {
        public string age { get; set; }
        public string sellerId { get; set; }
        public string status { get; set; }
        public string emailId { get; set; }
        public List<Contact> contacts { get; set; }
        public string displayName { get; set; }
        public string city { get; set; }
        public int pincode { get; set; }
        public string phoneNumber { get; set; }
        public string sellerName { get; set; }
        public string newStatus { get; set; }
        public bool tnsBlacklisted { get; set; }
        public Tier tier { get; set; }
    }

    public class Tier
    {
        public int support { get; set; }
    }

    public class UiConfig
    {
        public bool sizeChartDisabled { get; set; }
        public string sizeChartDisableMessage { get; set; }
        public bool enableWebEngage { get; set; }
        public bool enableResetPasswordMessageFlow { get; set; }
        public bool enableLoginRateLimitFlow { get; set; }
        public string flipkartMoengageAppId { get; set; }
        public string emeraldMoengageAppId { get; set; }
        public RateCard rateCard { get; set; }
        public GaRevampConfig gaRevampConfig { get; set; }
        public bool isRcbEnabled { get; set; }
        public bool importantNoticeEnabled { get; set; }
        public ImportantNoticeMessage importantNoticeMessage { get; set; }
        public HomePage homePage { get; set; }
        public AdsConfig adsConfig { get; set; }
        public GuidedAssistance guidedAssistance { get; set; }
        public Onboarding onboarding { get; set; }
        public Fdp fdp { get; set; }
        public QuestionOrderMapping questionOrderMapping { get; set; }
    }

    public class Value
    {
        public List<string> tags { get; set; }
        public string title { get; set; }
    }


}
