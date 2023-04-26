
using Lab4.Models;

namespace Lab4.ViewModels
{
    internal class AddViewModel : IUserInputModel
    {
        private DAO _dao;

        internal AddViewModel(DAO dao)
        {
            _dao = dao;
        }

        public void submit(Person person)
        {
            _dao.addPerson(person);
        }
    }
}
