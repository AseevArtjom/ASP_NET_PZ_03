using ASP_NET_PZ_03.Models;
using System.Text.Json;

namespace ASP_NET_PZ_03.Services
{
	public interface IObjectCollectionStorage<T> where T : class
	{
		public Task SaveAsync();

		public T items { get; }
	}
}
