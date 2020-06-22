using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrShopping.Infrastructure.Common
{
    public class ElasticsearchService
    {
        private ElasticClient _client;
        public ElasticsearchService(string es_url, string table)
        {
            var node = new Uri(es_url);
            _client = new ElasticClient(new ConnectionSettings(node).DefaultIndex(table));
        }


        public async Task Insert<T>(T t) where T : class
        {
            var resp = await _client.IndexDocumentAsync(t);
            Console.WriteLine(resp.Index);
        }
        public async Task<List<T>> SearchAsync<T>(ISearchRequest<T> query) where T : class
        {
            var type = typeof(T);
            var resp = await _client.SearchAsync<T>(query);

            return resp.Documents.ToList();
        }
        public async Task<List<T>> SearchAsync<T>(Func<SearchDescriptor<T>, ISearchRequest> selector = null) where T : class
        {
            var resp = await _client.SearchAsync<T>(selector);
            return resp.Documents.ToList();
        }
    }
}
