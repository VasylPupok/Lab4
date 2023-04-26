using Lab4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.ViewModels
{
    internal interface IUserInputModel
    {
        void submit(Person person);
    }
}
