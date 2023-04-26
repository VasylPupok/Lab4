
using Lab4.ViewModels;
using System.Windows;
using System.Windows.Controls;


namespace Lab4.Views
{
    /// <summary>
    /// Interaction logic for TablePage.xaml
    /// </summary>
    public partial class TableView : Page
    {

        private TableViewModel _tableViewModel;

        public TableView() {
            InitializeComponent();
            _tableViewModel = new TableViewModel(this);
            refresh();
        }

        public void refresh()
        {
            _tableViewModel.addAllPeople();
        }

        private void SelectedPerson(object sender, RoutedEventArgs e)
        {
            RemovePersonButton.IsEnabled = true;
            EditPersonButton.IsEnabled = true;
        }

        private void RemovePersonButton_Click(object sender, RoutedEventArgs e)
        {
            _tableViewModel.removeSelected();
        }

        private void FiltersButton_Click(object sender, RoutedEventArgs e)
        {

        }
  
        private void RemoveFiltersButton_Click(object sender, object e)
        {
            _tableViewModel.addAllPeople();
        }

        private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        {
            _tableViewModel.addPerson();
        }
    }
}
