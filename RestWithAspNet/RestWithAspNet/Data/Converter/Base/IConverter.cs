using System.Collections.Generic;

namespace RestWithAspNet.Data.Converter.Base
{
    public interface IConverter<O, D>
    {
        public D Parse(O origin);
        public List<D> Parse(List<O> origin);
    }


}
