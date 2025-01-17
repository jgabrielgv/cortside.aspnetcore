using System;

namespace Cortside.AspNetCore.AccessControl {
    public class IdentityServerConfiguration {
        public string Authority { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public string ApiName { get; set; }
        public string ApiSecret { get; set; }
        public bool EnableCaching { get; set; }
        public TimeSpan CacheDuration { get; set; }
        public TokenRequest Authentication { get; set; }
    }
}
