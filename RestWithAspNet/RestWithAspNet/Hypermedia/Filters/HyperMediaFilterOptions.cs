using RestWithAspNet.Hypermedia.Abstract;
using System.Collections.Generic;

namespace RestWithAspNet.Hypermedia.Filters
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}
