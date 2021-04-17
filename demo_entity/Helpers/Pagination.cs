using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo_entity.Helpers
{
    public class Pagination<T> where T : class
    {
        public int CurrPage { get; set; }
        public bool NextPage { get; set; }
        public bool PrevPage { get; set; }
        public int TotalPage { get; set; }
        public List<T> Data { get; set; }

        public Pagination(int currPage, bool nextPage, bool prevPage,int totalPage, List<T> data)
        {
            CurrPage = currPage;
            NextPage = nextPage;
            PrevPage = prevPage;
            TotalPage = totalPage;
            Data = data;
        }
    }
}
