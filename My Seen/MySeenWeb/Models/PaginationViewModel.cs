using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class PaginationViewModel
    {
        public PaginationViewModel(int pageNum, int totalRecords, int countInPage,string controller, string page)
        {
            Controller = controller;
            Page = page;

            CurentPage = pageNum;
            isFirstPage = CurentPage == 1;
            if (totalRecords != 0) LastPage = totalRecords / countInPage;
            else LastPage = 1;
            if (totalRecords % countInPage != 0) LastPage++;
            isLastPage = LastPage == CurentPage;
            MiddlePage = LastPage / 2;
            if (LastPage % 2 != 0) MiddlePage++;
            isMiddlePage = MiddlePage == CurentPage;

            //Хочу считать сам странички которые надо отобразить
            List = new List<int>();
            List.Add(1);
            if (isFirstPage && totalRecords > 1 && !List.Contains(2)) List.Add(2);

            if (!List.Contains(CurentPage)) List.Add(CurentPage);
            if ((CurentPage - 1) > 0 && !List.Contains(CurentPage - 1)) List.Add(CurentPage - 1);
            if ((CurentPage + 1) < LastPage && !List.Contains(CurentPage + 1)) List.Add(CurentPage + 1);

            //Середину не показывать если пересекаемся
            if (!(CurentPage == MiddlePage || CurentPage == (MiddlePage - 1) || CurentPage == (MiddlePage + 1))
                && !(CurentPage - 1 == MiddlePage || CurentPage - 1 == (MiddlePage - 1) || CurentPage - 1 == (MiddlePage + 1))
                && !(CurentPage + 1 == MiddlePage || CurentPage + 1 == (MiddlePage - 1) || CurentPage + 1 == (MiddlePage + 1))
                && !(CurentPage + 2 == MiddlePage - 1)
                && !(CurentPage - 2 == MiddlePage + 1)
                )
            {
                if (!List.Contains(MiddlePage)) List.Add(MiddlePage);
                if (!List.Contains(MiddlePage - 1)) List.Add(MiddlePage - 1);
                if (!List.Contains(MiddlePage + 1)) List.Add(MiddlePage + 1);
            }
            if (!List.Contains(LastPage)) List.Add(LastPage);
            if (isLastPage && !List.Contains(LastPage - 1)) List.Add(LastPage - 1);


            //если пропуск между страницами всего 1 которую заменили бы на "..." лучше покажу страничку...
            int i_prev=-5;
            List<int> add_list=new List<int>();

            List.Sort();
            foreach (int i in List)
            {
                if ((i_prev + 2) == i)
                {
                    add_list.Add(i_prev + 1);
                }
                i_prev = i;
            }

            foreach(int i in add_list)
            {
                List.Add(i);
            }

            List.Sort();
        }
        public bool isFirstPage { get; set; }
        public bool isMiddlePage { get; set; }
        public bool isLastPage { get; set; }
        public int CurentPage { get; set; }
        public int LastPage { get; set; }
        public int MiddlePage { get; set; }

        public string Controller { get; set; }
        public string Page { get; set; }

        public List<int> List { get; set; }
    }
}
