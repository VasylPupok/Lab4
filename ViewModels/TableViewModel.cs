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


        internal TableViewModel(TableView viewItemContainer)
        {   
            _viewDataGrid = viewItemContainer;
            _dao = new DAO();
        }

        internal void addAllPeople()
        {
            _viewDataGrid.UserGrid.Items.Clear();
            foreach (var p in _dao.getPeople().OrderBy(p => p.First))
            {
                _viewDataGrid.UserGrid.Items.Add(p);
            }
        }

        internal void removeSelected()
        {
            ulong id = (_viewDataGrid.UserGrid.SelectedItem as Pair<ulong, Person>).First;
            _dao.deletePerson(id);
            _viewDataGrid.UserGrid.Items.Remove(_viewDataGrid.UserGrid.SelectedItem);
        }

        internal void addPerson()
        {
            _viewDataGrid.NavigationService.Navigate(EditUserPage.AddUserView(new AddViewModel(_dao), _viewDataGrid));
        }

        internal void editPerson()
        {
            var selected = _viewDataGrid.UserGrid.SelectedItem as Pair<ulong, Person>;
            if(selected != null)
            {
                _viewDataGrid.NavigationService.Navigate(
                    EditUserPage.EditUserView(
                        new EditViewModel(_dao, selected.First), 
                        _viewDataGrid,
                        selected.Second
                        ));
            }
            
        }

    }
}
