namespace GtMotive.Estimate.Microservice.Host.DependencyInjection
{
    internal sealed class PathBase
    {
        public const string DefaultPathBase = "/";

        public PathBase(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || DefaultPathBase.Equals(value, System.StringComparison.Ordinal))
            {
                IsDefault = true;
                CurrentWithoutTrailingSlash = DefaultPathBase;
            }
            else
            {
                IsDefault = false;
                CurrentWithoutTrailingSlash = value.TrimEnd('*').TrimEnd('/');
            }
        }

        public bool IsDefault { get; }

        public string CurrentWithoutTrailingSlash { get; }
    }
}
