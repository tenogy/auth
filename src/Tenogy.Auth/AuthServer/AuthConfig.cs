using System.Collections.Generic;

namespace Tenogy.Auth
{
	public class AuthConfig
	{
		public string Secret { get; set; }
		public string Certificate { get; set; }
		public IEnumerable<IdentityApiResource> Apis { get; set; }
		public IEnumerable<IdentityClient> Clients { get; set; }
	}

	public class IdentityApiResource : NamedResource
	{
		public string Secret { get; set; }
		public IEnumerable<NamedResource> Scopes { get; set; }
	}

	public class NamedResource
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
	}

	public class IdentityClient
	{
		public string ClientId { get; set; }
		public string Secret { get; set; }
		public IEnumerable<string> Scopes { get; set; }
	}
}
