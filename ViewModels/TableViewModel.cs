using Lab4.Models;
using Lab4.Utils;
using Lab4.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Lab4.ViewModels
{
    internal class TableViewModel
    {
        private TableView _viewDataGrid;
        private readonly DAO _dao;
        

        public TableViewModel(TableView viewItemContainer)
        {   
            _viewDataGrid = viewItemContainer;
            _dao = new DAO();
        }

        public void addAllPeople()
        {
            _viewDataGrid.UserGrid.Items.Clear();
            foreach (var p in _dao.getPeople().OrderBy(p => p.First))
            {
                _viewDataGrid.UserGrid.Items.Add(p);
            }
        }

        public void removeSelected()
        {
            ulong id = (_viewDataGrid.UserGrid.SelectedItem as Pair<ulong, Person>).First;
            _dao.deletePerson(id);
            _viewDataGrid.UserGrid.Items.Remove(_viewDataGrid.UserGrid.SelectedItem);
        }

        public void addPerson()
        {
            _viewDataGrid.NavigationService.Navigate(new EditUserPage(new AddViewModel(_dao), _viewDataGrid));
        }

    }
}
