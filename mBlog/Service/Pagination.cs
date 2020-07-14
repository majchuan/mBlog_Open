using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace mBlog.Service
{
    public class Pagination
    {
        public int TotalPageNumber {get;set;}
        public int CurrentPageNumber {get;set;}
        public const int FirstPage = 1 ;
        public const int PageSize = 10 ;

        public IList<Object> PageModelList {get;set;}

        /*
        * True : next page, false : prev page
        */
        public Boolean? PageDirection {get;set;}
        public Pagination( IList<Object> aPageModelList, int currentPageNum , Boolean? pageDirection)
        {
            PageModelList = aPageModelList ;
            TotalPageNumber = Convert.ToInt32(Math.Ceiling( (double)PageModelList.Count / (double)Pagination.PageSize));
            PageDirection = pageDirection;

            if(PageDirection == null)
            {
                CurrentPageNumber = Pagination.FirstPage ;
            }else{
                if(TotalPageNumber > currentPageNum)
                {
                    if(pageDirection == true)
                    {
                        CurrentPageNumber = currentPageNum + 1 ;
                    }else{
                        CurrentPageNumber = Pagination.FirstPage >= currentPageNum ? Pagination.FirstPage : currentPageNum - 1 ;
                    }
                }else if( TotalPageNumber <= currentPageNum)
                {
                    CurrentPageNumber = TotalPageNumber;
                }
            }
        }

        public IList<Object> UsePagination(Func<IList<Object>,int, IList<Object>> displayModel = null)
        {
            if(displayModel == null)
            {
                displayModel = (x, pageNumber) => {
                    var count = 0 ;
                    if(x != null && x.Count > 0)
                    {
                        var lastPage = (int)Math.Ceiling((decimal)x.Count / Pagination.PageSize);

                        switch (pageNumber)
                        {
                            case Pagination.FirstPage :
                                if(x.Count < Pagination.PageSize)
                                {
                                    count = x.Count ;
                                }else{
                                    count = Pagination.PageSize;
                                }
                                break;
                            case int currentPage when lastPage == pageNumber :
                                count = x.Count - (currentPage - 1) * Pagination.PageSize;
                                break;
                            default :
                                count = Pagination.PageSize ;
                                break;
                        }
                    }
                    return (x== null || x.Count == 0) ? new List<Object>() : x.ToList().GetRange(((pageNumber-1) * Pagination.PageSize), count);
                };
            }
            return displayModel.Invoke(PageModelList,CurrentPageNumber);
        }


    }
}