namespace DentalClinicManagementApp.Lib
{
    public class AppConstants
    {
        public static readonly string ADMIN_ROLE = "admin";
        public static readonly string WORKER_ROLE = "worker";

        public static readonly string ADMIN_USER = "admin";
        public static readonly string ADMIN_EMAIL = "admin@admin.pt";
        public static readonly string ADMIN_PWD = "admin2023";

        public static readonly string WORKER_USER = "worker";
        public static readonly string WORKER_EMAIL = "worker@worker.pt";
        public static readonly string WORKER_PWD = "worker2023";

        //Policy and roles associated
        public const string APP_POLICY = "APP_POLICY";
        public static readonly string[] APP_POLICY_ROLES =
        {
            ADMIN_ROLE,
            WORKER_ROLE
        };

        //Just 'Admin'
        public const string APP_ADMIN_POLICY = "APP_ADMIN_POLICY";
        public static readonly string[] AAPP_ADMIN_POLICY_ROLES =
        {
            ADMIN_ROLE
        };
    }
}
