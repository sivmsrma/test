using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Terret_Billing.Presentation.Models
{
    public class TaggingPagination : INotifyPropertyChanged
    {
        private int _currentPage = 1;
        private int _totalPages = 1;
        private int _totalItems = 0;
        private string _pageInfo = string.Empty;
        private bool _canGoNext = false;
        private bool _canGoPrevious = false;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }

        public int TotalItems
        {
            get => _totalItems;
            set
            {
                _totalItems = value;
                OnPropertyChanged(nameof(TotalItems));
            }
        }

        public string PageInfo
        {
            get => _pageInfo;
            set
            {
                _pageInfo = value;
                OnPropertyChanged(nameof(PageInfo));
            }
        }

        public bool CanGoNext
        {
            get => _canGoNext;
            set
            {
                _canGoNext = value;
                OnPropertyChanged(nameof(CanGoNext));
            }
        }

        public bool CanGoPrevious
        {
            get => _canGoPrevious;
            set
            {
                _canGoPrevious = value;
                OnPropertyChanged(nameof(CanGoPrevious));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}