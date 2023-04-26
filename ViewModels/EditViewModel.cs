
using Lab4.Models;

namespace Lab4.ViewModels
{
    internal class EditViewModel : IUserInputModel
    {
        private DAO _dao;
        private ulong _currId;
        
        internal EditViewModel(DAO dao, ulong id)
        {
            _dao = dao;
            _currId = id;
        }

        public void submit(Person person)
        {
            _dao.updatePerson(_currId, person);
        }

    }
}
