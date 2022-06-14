using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreBBS.Entities
{
    public class Page<T>
        where T : class
    {
        public long Total { get; private set; }
        public long PageSize { get; private set; }
        public List<T> List { get; private set; }
        public Page(List<T> list, long pageSize, long total)
        {
            this.List = list;
            this.PageSize = pageSize;
            this.Total = total;
        }

        public long GetPageCount()
        {
            return (Total + PageSize - 1) / PageSize;
        }
    }
}
