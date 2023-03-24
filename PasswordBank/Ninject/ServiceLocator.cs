using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordBank.Ninject
{
    //Help from: https://www.youtube.com/watch?v=yN4SgWHwhgk
    //On how to set up a ninject dependency injection design for all of the page
    //view models and the single instance database for the program. 
    public class ServiceLocator
    {
        private readonly IKernel kernel;

        public ServiceLocator()
        {
            kernel = new StandardKernel(new ServiceModule());
        }

        public ViewModels.PasswordBankMainPageVM MainPageViewModel
        {
            get { return kernel.Get<ViewModels.PasswordBankMainPageVM>(); }
        }

        public ViewModels.CreateAccountVM CreateAccountViewModel 
        {
            get { return kernel.Get<ViewModels.CreateAccountVM>(); } 
        }
        public ViewModels.RecoverPasskeyVM RecoverPasskeyViewModel 
        { 
            get { return kernel.Get<ViewModels.RecoverPasskeyVM>(); } 
        }
        public ViewModels.RecoverPasswordVM RecoverPasswordViewModel
        {
            get { return kernel.Get<ViewModels.RecoverPasswordVM>(); }
        }
        public ViewModels.RecoverUsernameVM RecoverUsernameViewModel
        {
            get { return kernel.Get<ViewModels.RecoverUsernameVM>(); }
        }
        public ViewModels.PasswordDatabaseVM PasswordDatabaseViewModel
        {
            get { return kernel.Get<ViewModels.PasswordDatabaseVM>(); }
        }
        public ViewModels.AddAccountVM AddAccountViewModel 
        {
            get { return kernel.Get<ViewModels.AddAccountVM>(); } 
        }
        public ViewModels.EditAccountVM EditAccountViewModel
        {
            get { return kernel.Get<ViewModels.EditAccountVM>(); }
        }
        public ViewModels.EditUserAccountVM EditUserAccountViewModel
        {
            get { return kernel.Get<ViewModels.EditUserAccountVM>(); }
        }
    }
}
