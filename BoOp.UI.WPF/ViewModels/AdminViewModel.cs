using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoOp.UI.WPF.ViewModels
{
    public class AdminViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;

        public Rechtelevel UserRights { get; }
        public DelegateCommand BackCommand { get; set; }
        public DelegateCommand AddPersonCommand { get; set; }
        public DelegateCommand RemoveBookCommand { get; set; }
        public DelegateCommand EditBookCommand { get; set; }
        public DelegateCommand AddUserCommand { get; set; }
        public DelegateCommand RemoveUserCommand { get; set; }
        public DelegateCommand EditUserCommand { get; set; }
        public DelegateCommand AddBookCommand { get; set; }

        public AdminViewModel(INavigationService navigationService, ILibrary library, PersonModel user)
        {
            _navigationService = navigationService;
            _library = library;
            UserRights = user.Rechte;

            BackCommand = new DelegateCommand( 
                x =>
                {
                    _navigationService.ShowLibraryView(user);
                });

            AddPersonCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAddPersonView(user);
                }, 
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });
            
            AddBookCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAddBookView(user);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });

            EditUserCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowEditUserView(user);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });

            RemoveBookCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowRemoveBookView(user);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.ADMIN;
                });
        }
    }
}
