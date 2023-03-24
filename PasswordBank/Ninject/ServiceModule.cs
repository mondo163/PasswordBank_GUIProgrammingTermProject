using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserPasswordDatabaseLib;

namespace PasswordBank.Ninject
{
    //Help from: https://www.youtube.com/watch?v=yN4SgWHwhgk
    //On how to set up a ninject dependency injection design for all of the page
    //view models and the single instance database for the program. 
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<UsersDatabase>().To<UsersDatabase>().InSingletonScope();
        }
    }
}
