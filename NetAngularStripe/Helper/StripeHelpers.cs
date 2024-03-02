namespace NetAngularStripe.Helper;

public static class StripeHelpers
{
    public static class SubscriptionUpdate
    {
        /// <summary>
        /// The types of subscription updates that are supported for items listed in the products attribute. When empty, subscriptions are not updateable.
        /// </summary>
        public class DefaultAllowedUpdates
        {
            /// <summary>
            /// Allow switching to a different price.
            /// </summary>
            public const string Price = "price";

            /// <summary>
            /// Allow applying promotion codes to subscriptions.
            /// </summary>
            public const string PromotionCode = "promotion_code";

            /// <summary>
            /// Allow updating subscription quantities.
            /// </summary>
            public const string Quantity = "quantity";
        }

    }

    /// <summary>
    /// The types of customer updates that are supported.
    /// </summary>
    public static class CustomerUpdate
    {
        public static class AllowedUpdates
        {
            /// <summary>
            /// Allow updating billing addresses.
            /// </summary>
            public const string Address = "address";

            /// <summary>
            /// Allow updating email addresses.
            /// </summary>
            public const string Email = "email";

            /// <summary>
            /// Allow updating names.
            /// </summary>
            public const string Name = "name";

            /// <summary>
            /// Allow updating phone numbers.
            /// </summary>
            public const string Phone = "phone";

            /// <summary>
            /// Allow updating shipping addresses.
            /// </summary>
            public const string Shipping = "shipping";

            /// <summary>
            /// Allow updating tax IDs.
            /// </summary>
            public const string TaxId = "tax_id";
        }

        public static List<string> GetAllAllowedUpdates()
        {
            return 
            [
                AllowedUpdates.Address,
                AllowedUpdates.Email,
                AllowedUpdates.Name,
                AllowedUpdates.Phone,
                AllowedUpdates.Shipping,
                AllowedUpdates.TaxId,
                AllowedUpdates.TaxId
            ];
        }

    }

    public static class SubscriptionCancel
    {
        /// <summary>
        /// Whether to cancel subscriptions immediately or at the end of the billing period.
        /// </summary>
        public static class Mode
        {
            /// <summary>
            /// After canceling, customers can still renew subscriptions until the billing period ends.
            /// </summary>
            public const string AtPeriodEnd = "at_period_end";

            /// <summary>
            /// Cancel subscriptions immediately.
            /// </summary>
            public const string Immediately = "immediately";
        }

        /// <summary>
        /// Which cancellation reasons will be given as options to the customer.
        /// </summary>
        public static class CancellationReason
        {
            public static class Options
            {
                /// <summary>
                /// Customer service was less than expected
                /// </summary>
                public const string CustomerService = "customer_service";

                /// <summary>
                /// Quality was less than expected
                /// </summary>
                public const string LowQuality = "low_quality";

                /// <summary>
                /// Some features are missing
                /// </summary>
                public const string MissingFeatures = "missing_features";

                /// <summary>
                /// Other reason
                /// </summary>
                public const string Other = "other";

                /// <summary>
                /// I’m switching to a different service
                /// </summary>
                public const string SwitchedService = "switched_service";

                /// <summary>
                /// Ease of use was less than expected
                /// </summary>
                public const string TooComplex = "too_complex";

                /// <summary>
                /// It’s too expensive
                /// </summary>
                public const string TooExpensive = "too_expensive";

                /// <summary>
                /// I don’t use the service enough
                /// </summary>
                public const string Unused = "unused";
            }

            public static List<string> GetAllCancellationReasonOptions()
            {
                return
                [
                    Options.CustomerService,
                    Options.LowQuality,
                    Options.MissingFeatures,
                    Options.Other,
                    Options.SwitchedService,
                    Options.TooComplex,
                    Options.TooExpensive,
                    Options.Unused
                ];
            }
        }
    }

}