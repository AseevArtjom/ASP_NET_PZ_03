using ASP_NET_PZ_03.Models;
using System.Text.Json;

namespace ASP_NET_PZ_03.Services
{
	public class FileStorage : IObjectCollectionStorage<List<Info>>
	{
		private string _filename;

		public FileStorage(string filename)
		{
			_filename = filename;
			LoadAsync().Wait();
		}

		public async Task LoadAsync()
		{
			if(File.Exists(_filename))
			{
				using var file = File.OpenRead(_filename);
                items = await JsonSerializer.DeserializeAsync<List<Info>>(file);
			}
			else
			{
                items = new List<Info>();
			}
		}

        public async Task SaveAsync()
        {
            using var file = File.OpenWrite(_filename);
            await JsonSerializer.SerializeAsync(file, items);
        }


        public List<Info> items { get; private set; }
    }
}
