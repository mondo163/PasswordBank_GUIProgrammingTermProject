using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordBank.ViewModels
{
    //used to close the windows using the close action and the event handler in the xaml.cs for the page
    //tried to use abstraction for most of the pages, but had to do it differently for teh main window and database
    //page
    interface ICloseWindow
    {
        Action Close { get; set; }
    }
}
