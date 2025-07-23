using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Domain.Entities;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using ClosedXML.Excel;
using System.Windows.Input;

namespace Terret_Billing.Presentation.ViewModels
{

    public class PurchaseReportViewModel : INotifyPropertyChanged
    {

        private IPurchaseReportRepository _purchaseRepository;

        private readonly string _firmid;
           
        public ObservableCollection<string> Metals { get; set; } = new();

        public ObservableCollection<string> PurityTypes { get; set; } = new();

        public ObservableCollection<string> Parties { get; set; } = new();

        public ObservableCollection<PurchaseViewReport> FilteredPurchaseReports { get; set; } = new();

        public ObservableCollection<string> BillNos { get; set; } = new();
        public ICommand CancelCommand { get; }

        private List<PurchaseViewReport> _allPurchaseReports = new();

       

        private string _selectedParty;
        public string SelectedParty
        {
            get => _selectedParty;
            set
            {
                SetProperty(ref _selectedParty, value);
                ApplyFilters();
            }
        }

        private string _selectedBillNo;
        public string selectedBillNo
        {
            get => _selectedBillNo;
            set
            {
                SetProperty(ref _selectedBillNo, value);
                ApplyFilters();
            }
        }

        private string _selectedMetal;
        public string SelectedMetal
        {
            get => _selectedMetal;
            set
            {
                SetProperty(ref _selectedMetal, value);
                ApplyFilters();
            }
        }

        private string _selectedPurity;
        public string selectedPurity
        {
            get => _selectedPurity;
            set
            {
                SetProperty(ref _selectedPurity, value);
                ApplyFilters();
            }
        }


        public PurchaseReportViewModel(IPurchaseReportRepository purchaseReportRepository, string firmId)
        {
            _firmid = firmId;
            _purchaseRepository = purchaseReportRepository;
            var load = LoadAllAsync();
        }

        public async Task LoadAllAsync()
        {
            await Task.WhenAll(
                LoadItemsAsync(Parties, _purchaseRepository.GetPartyNamesWithPurchasesAsync(_firmid)),
                LoadItemsAsync(BillNos, _purchaseRepository.GetBillNoWithPurchasesAsync(_firmid)),
                LoadItemsAsync(Metals, _purchaseRepository.GetMetalsWithPurchasesAsync(_firmid)),
                LoadItemsAsync(PurityTypes, _purchaseRepository.GetPurityTypesWithPurchasesAsync(_firmid)),
                LoadItemsAsync(FilteredPurchaseReports, _purchaseRepository.GetAllPurchaseReportLocalAsync(_firmid))
            );
        }
        private async Task LoadItemsAsync<T>(ObservableCollection<T> collection, Task<IEnumerable<T>> fetchTask)
        {
            collection.Clear();
            var items = await fetchTask;
            foreach (var item in items)
                collection.Add(item);

            // If loading purchase reports, also update the master list
            if (typeof(T) == typeof(PurchaseViewReport))
            {
                _allPurchaseReports = items.Cast<PurchaseViewReport>().ToList();
                ApplyFilters();
            }
        }


        private void ApplyFilters()
        {
            var filtered = _allPurchaseReports.AsEnumerable();

            if (!string.IsNullOrEmpty(SelectedParty))
                filtered = filtered.Where(r => r.PartyName == SelectedParty);

            if (!string.IsNullOrEmpty(SelectedMetal))
                filtered = filtered.Where(r => r.Metal == SelectedMetal);

            // Add more filters as needed

            FilteredPurchaseReports.Clear();
            foreach (var item in filtered)
                FilteredPurchaseReports.Add(item);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
    }

}


