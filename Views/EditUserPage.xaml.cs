using Lab4.Models;
using Lab4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab4.Views
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class EditUserPage : Page
    {
        private IUserInputModel _userInputModel;
        private TableView _table;

        private EditUserPage(IUserInputModel userInputModel, TableView table)
        {
            InitializeComponent();
            _userInputModel = userInputModel;
            _table = table;
        }

        internal static EditUserPage AddUserView(AddViewModel userInputModel, TableView table)
        {
            return new EditUserPage(userInputModel, table);
        }

        internal static EditUserPage EditUserView(EditViewModel userInputModel, TableView table, Person userToBeEdited)
        {
            EditUserPage page = new EditUserPage(userInputModel, table);
            page.nameInput.Text = userToBeEdited.Name;
            page.surnameInput.Text = userToBeEdited.Surname;
            page.emailInput.Text = userToBeEdited.Email;
            page.dateInput.SelectedDate = userToBeEdited.Birthday;
            return page;
        }


        private void submitButtonClicked(object sender, EventArgs e)
        {
            lock (this)
            {

                this.IsEnabled = false;

                string name = this.nameInput.Text;
                string surname = this.surnameInput.Text;
                string email = this.emailInput.Text;
                DateTime? birthday = this.dateInput.SelectedDate;
                Person p = new Person(name, surname, email, birthday.Value);

                Thread worker = new Thread(
                async () =>
                {
                    if (await Task.Run(() => allInputsValid(name, surname, email, birthday)))
                    {
                        //Person p = new Person(name, surname, email, birthday.Value);
                        this._userInputModel.submit(p);
                        this.Dispatcher.Invoke(() => {
                            _table.refresh();
                            this.NavigationService.Navigate(_table); 
                        });
                    }
                    this.Dispatcher.Invoke(() => this.IsEnabled = true);
                }

                    );

                worker.IsBackground = true;
                worker.SetApartmentState(ApartmentState.STA);
                worker.Start();
            }
        }

        private bool allInputsValid(
                string name,
                string surname,
                string email,
                DateTime? birthday
            )
        {

            bool[] results = Task.WhenAll(new List<Task<bool>> {
                validateName(name),
                validateSurname(surname),
                validateEmail(email),
                validateDate(birthday),
            }).Result;

            return results.All(x => x);
        }

        private Task<bool> validateName(string name)
        {
            Thread.Sleep(1000);
            return
                Task.Run(() =>
                !string.IsNullOrEmpty(name) && Regex.IsMatch(name, "[\\w]+", RegexOptions.IgnoreCase));
        }

        private Task<bool> validateSurname(string surname)
        {
            return
                Task.Run(() =>
                !string.IsNullOrEmpty(surname) && Regex.IsMatch(surname, "[\\w]+", RegexOptions.IgnoreCase));
        }

        private Task<bool> validateEmail(string email)
        {
            return
                Task.Run(
                    () => !string.IsNullOrEmpty(email) &&
                    Regex.IsMatch(email, "[\\w_.]+@[\\w_.]+", RegexOptions.IgnoreCase)
                        );
        }

        private Task<bool> validateDate(DateTime? date)
        {
            return Task.Run(() =>
                    date.HasValue &&
                    date.Value <= DateTime.Now &&
                    (DateTime.Now.Year - date.Value.Year) <= Person.MAX_AGE
                    );
        }

    }
}
