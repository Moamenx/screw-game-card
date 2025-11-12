using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewGameCard.Application.DTO.Common
{
    public class PaginatedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public PaginatedResult() { }

        public PaginatedResult(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        }

        public static PaginatedResult<T> Create(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            return new PaginatedResult<T>(data, pageNumber, pageSize, totalRecords);
        }

        public static PaginatedResult<T> Empty(int pageNumber = 1, int pageSize = 10)
        {
            return new PaginatedResult<T>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = 0,
                TotalRecords = 0,
                Data = Enumerable.Empty<T>()
            };
        }
    }
}
