using System;
using System.Collections.Generic;
using System.Linq;

namespace Terret_Billing.Application.Helpers
{
    public static class Paginator
    {
        public const int DefaultPageSize = 17;
        public const int MaxPageSize = 100;

        // Gets paginated items from stored procedure results (all items)
        public static (List<T> items, int currentPage, int totalPages, int totalItems) GetPageFromStoredProc<T>(
            List<T> allItems, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
            if (pageSize < 1) throw new ArgumentException("Page size must be greater than 0", nameof(pageSize));

            var totalItems = allItems?.Count ?? 0;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (totalPages == 0) totalPages = 1;

            var currentPage = Math.Max(1, Math.Min(pageNumber, totalPages));
            var startIndex = (currentPage - 1) * pageSize;

            var pageItems = allItems?.Skip(startIndex).Take(pageSize).ToList() ?? new List<T>();

            return (pageItems, currentPage, totalPages, totalItems);
        }

        // Gets paginated items from stored procedure with total count
        public static (List<T> items, int currentPage, int totalPages, int totalItems) GetPageFromStoredProc<T>(
            List<T> pageItems, int totalItems, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
            if (pageSize < 1) throw new ArgumentException("Page size must be greater than 0", nameof(pageSize));

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (totalPages == 0) totalPages = 1;

            var currentPage = Math.Max(1, Math.Min(pageNumber, totalPages));

            return (pageItems, currentPage, totalPages, totalItems);
        }

        // Gets page info string
        public static string GetPageInfo(int currentPage, int totalPages, int totalItems, int pageSize)
        {
            if (totalItems == 0) return "No items found";
            
            return $"Page {currentPage}/{totalPages}";
        }

        // Gets visible page numbers for UI
        public static int[] GetVisiblePageNumbers(int currentPage, int totalPages, int maxVisiblePages = 5)
        {
            if (totalPages <= maxVisiblePages) return Enumerable.Range(1, totalPages).ToArray();

            var startPage = Math.Max(1, currentPage - maxVisiblePages / 2);
            var endPage = Math.Min(totalPages, startPage + maxVisiblePages - 1);

            if (endPage - startPage + 1 < maxVisiblePages) startPage = Math.Max(1, endPage - maxVisiblePages + 1);

            return Enumerable.Range(startPage, endPage - startPage + 1).ToArray();
        }

        // Validates pagination parameters
        public static (int pageNumber, int pageSize) ValidateParameters(int pageNumber, int pageSize)
        {
            var normalizedPageNumber = Math.Max(1, pageNumber);
            var normalizedPageSize = Math.Max(1, Math.Min(pageSize, MaxPageSize));
            return (normalizedPageNumber, normalizedPageSize);
        }

        // Calculates offset 
        public static int CalculateOffset(int pageNumber, int pageSize)
        {
            return (pageNumber - 1) * pageSize;
        }

        // Gets next page number if available
        public static int? GetNextPage(int currentPage, int totalPages)
        {
            return currentPage < totalPages ? currentPage + 1 : null;
        }

        // Gets previous page number if available
        public static int? GetPreviousPage(int currentPage)
        {
            return currentPage > 1 ? currentPage - 1 : null;
        }
    }
} 