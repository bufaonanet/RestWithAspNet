using System.Collections.Generic;

namespace RestWithAspNet.Hypermedia.Abstract
{
    public interface ISupportsHyperMedia
    {
        List<HyperMediaLink> Links { get; set; }
       
    }
}
