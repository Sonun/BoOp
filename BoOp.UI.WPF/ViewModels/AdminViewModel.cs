using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace BoOp.UI.WPF.ViewModels
{
    public class AdminViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;
        private Dispatcher _dispatcher;
        private ObservableCollection<BookViewModel> _bookList;
        private ObservableCollection<BuchModel> _currentList, _originalList;
        private ObservableCollection<PersonViewModel> _userList;
        private ObservableCollection<PersonModel> _currentUserList, _originalUserList;
        private ObservableCollection<ExemplarViewModel> _lendedBookList;
        private PersonModel _user;
        private bool _titleFlag, _authorFlag, _isbnFlag, _vornameFlag, _rechteFlag, _nachnameFlag, _lendeddateflag, _lendednameflag;
        private string _allBookSearchWord, _userSearchWord, _lendedBooksSearchWord;

        public PersonModel LoggedinUser { get { return _user; } }

        public static ObservableCollection<ExemplarViewModel> StaticBookPrintList;
        public static ObservableCollection<PersonViewModel> StaticUserIDPrintList;

        public ObservableCollection<ExemplarViewModel> BookPrintList { get {  return StaticBookPrintList;  }set { OnPropertyChanged(); } }
        public ObservableCollection<PersonViewModel> UserIDPrintList { get { return StaticUserIDPrintList; } set { OnPropertyChanged(); } }


        //button commands

        public DelegateCommand BackUpDatabaseCommand { get; set; }
        public DelegateCommand LoadBackupCommand { get; set; }
        
        public DelegateCommand BackCommand { get; set; }
        public DelegateCommand AddPersonCommand { get; set; }
        public DelegateCommand RemoveBookCommand { get; set; }
        public DelegateCommand AddBookCommand { get; set; }
        public DelegateCommand CloseApplicationCommand { get; set; }

        //print commands 
        public DelegateCommand PrintBookBarcodesCommand { get; set; }
        public DelegateCommand PrintUserCardsCommand { get; set; }

        //context menu commands
        public DelegateCommand EditBookCommand { get; set; }
        public DelegateCommand AddUserCommand { get; set; }
        public DelegateCommand RemoveUserCommand { get; set; }
        public DelegateCommand EditUserCommand { get; set; }

        //SearchCommands
        public DelegateCommand SearchTitleInAllBooks { get; set; }
        public DelegateCommand ClearSearchInAllBooks { get; set; }
        public DelegateCommand SearchVornameInUsers { get; set; }
        public DelegateCommand ClearSearchInUsers { get; set; }
        public DelegateCommand SearchTitleInLendedBooks { get; set; }
        public DelegateCommand ClearSearchInLendedBooks { get; set; }

        //book sort commands
        public DelegateCommand SortTitleCommand { get; set; }
        public DelegateCommand SortAuthorCommand { get; set; }
        public DelegateCommand SortISBNCommand { get; set; }
        public DelegateCommand SortRatingCommand { get; set; }

        //book search commands
        public DelegateCommand SortVornameCommand { get; set; }
        public DelegateCommand SortNachnameCommand { get; set; }
        public DelegateCommand SortRechteCommand { get; set; }

        //lended book commands
        public DelegateCommand SortLendedName { get; set; }
        public DelegateCommand SortLendedDate { get; set; }


        public Rechtelevel UserRights { get; }

        //searchword propertys
        public string AllBookSearchWord { get { return _allBookSearchWord; } set { _allBookSearchWord = value; OnPropertyChanged(); } }
        public string UserSearchWord { get { return _userSearchWord; } set { _userSearchWord = value; OnPropertyChanged(); } }
        public string LendedBooksSearchWord { get { return _lendedBooksSearchWord; } set { _lendedBooksSearchWord = value; OnPropertyChanged(); } }

        public ObservableCollection<PersonViewModel> UserList
        {
            get
            {
                return _userList;
            }
            set
            {
                _userList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BookViewModel> BookList
        {
            get
            {
                return _bookList;
            }
            set { SetValue(ref _bookList, value); }
        }
        public ObservableCollection<ExemplarViewModel> LendedBookList 
        { 
            get 
                { 
                return _lendedBookList; 
                } 
            set 
                {
                _lendedBookList = value; 
                OnPropertyChanged(); 
                } 
        }


        public AdminViewModel(INavigationService navigationService, ILibrary library, PersonModel user, Dispatcher dispatcher)
        {
            _navigationService = navigationService;
            _library = library;
            _user = user;
            _dispatcher = dispatcher;
            ScanUserViewModel.LogoutTimer.Enabled = false;

            _originalList = _library.GetAllBooks();
            _currentList = _originalList;
            UpdateBooklist(_currentList);
            LendedBookList = new ObservableCollection<ExemplarViewModel>();
            UpdateLendedBooklist(_originalList);

            _originalUserList = _library.GetAllUsers();
            _currentUserList = _originalUserList;
            UpdateUserlist(_currentUserList);

            UserRights = user.Rechte;

            BackUpDatabaseCommand = new DelegateCommand(
                x =>
                {
                    CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                    dialog.InitialDirectory = Directory.GetCurrentDirectory()+ @"\SQLiteBoOpDB.db";
                    dialog.IsFolderPicker = true;
                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        string sourceFile = Directory.GetCurrentDirectory() + @"\SQLiteDB\SQLiteBoOpDB.db";
                        string destinationFile = dialog.FileName;
                        if (File.Exists(sourceFile))
                        {
                            try
                            {
                                FileInfo i = new FileInfo(sourceFile);
                                File.Copy(sourceFile, destinationFile+@"\Datenbankb_Backup_" + DateTime.Now.ToString("g").Replace(":", "-") + ".db", true);
                                MessageBox.Show("Backup liegt in: " + dialog.FileName, "Erfolg!");
                            }
                            catch (IOException iox)
                            {
                                MessageBox.Show("Fehler beim Erstellen des Backups :( \n\n" + iox.Message, "Fehler!");
                            }
                        }
                    }
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });

            LoadBackupCommand = new DelegateCommand(
                x =>
                {
                    if (MessageBox.Show("Der jetzige Stand wird beim Laden überschrieben. \n\nSind Sie sicher, dass Sie die Datenbank überschreiben möchten?", "Laden?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                        dialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\SQLiteBoOpDB.db";
                        dialog.IsFolderPicker = false;
                        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                        {
                            string destinationFile = Directory.GetCurrentDirectory() + @"\SQLiteDB\SQLiteBoOpDB.db";
                            string sourceFile = dialog.FileName;
                            if (File.Exists(sourceFile))
                            {
                                try
                                {
                                    var fileExtension = sourceFile.Substring(sourceFile.Length - 3, 3).ToLower();
                                    if (!fileExtension.Equals(".db"))
                                    {
                                        MessageBox.Show("Sie haben keine Datenbankdatei ausgewählt!","Fehler!");
                                        return;
                                    }

                                    FileInfo i = new FileInfo(sourceFile);
                                    File.Copy(sourceFile, destinationFile, true);
                                    MessageBox.Show("Das Backup wurde erfolgreich geladen! \n\nAnsicht wird aktualisiert!", "Erfolg!");
                                    _navigationService.ShowAdminView(user);
                                }
                                catch (IOException iox)
                                {
                                    MessageBox.Show("Fehler beim Laden des Backups :( \n\n" + iox.Message, "Fehler!");
                                }
                            }
                        }
                    }
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });

            BackCommand = new DelegateCommand( 
                x =>
                {
                    ScanUserViewModel.LogoutTimer.Enabled = true;
                    ScanUserViewModel.LogoutTimer.Start();
                    _navigationService.ShowLibraryView(user);
                });

            AddPersonCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAddPersonView(user);
                }, 
                y =>
                {
                    return user.Rechte >= Rechtelevel.HELFER;
                });
            
            AddBookCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAddBookView(user);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.HELFER;
                });

            PrintBookBarcodesCommand = new DelegateCommand(
                x =>
                {
                    //check if there is something to print
                    if (BookPrintList == null || BookPrintList.Count == 0)
                    {
                        MessageBox.Show("Die Liste, die Sie Drucken wollten, war Leer. \n\nFügen Sie zuerst Daten zum Drucken hinzu", "Fehler!");
                        return;
                    }

                    var triplelist = new List<(string barcode, string name, string klase)>();

                    foreach (var book in BookPrintList)
                    {
                        triplelist.Add((book.Model.BasicInfos.Barcode, book.BuchModel.BasicInfos.Titel + " - " + book.BuchModel.BasicInfos.Author, ""));
                    }

                    try
                    {
                        Utils.GenerateMultipleBarcodePDF(triplelist, false, user.VorUndNachname, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

                        MessageBox.Show("Die PDF Datei wurde unter: " + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\BoOp_PDF_Dateien\\{user.VorUndNachname}\\Bücher gespeichert.", "Erfolg!");
                        StaticBookPrintList.Clear();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("PDF konnte nicht erstellt werden." + e.Message, "Fehler!");
                    }
                    
                });

            PrintUserCardsCommand = new DelegateCommand(
                x =>
                {
                    //check if there is something to print
                    if(UserIDPrintList == null || UserIDPrintList.Count == 0)
                    {
                        MessageBox.Show("Die Liste, die Sie Drucken wollten war Leer. \n\nFügen sie zuerst Daten zum Drucken hinzu!", "Fehler!");
                        return;
                    }

                    var triplelist = new List<(string barcode, string name, string klasse)>();

                    foreach (var user in UserIDPrintList)
                    {
                        triplelist.Add((user.Model.AusweisID, user.Model.VorUndNachname, user.Model.Klassenname));
                    }

                    try
                    {
                        Utils.GenerateMultipleBarcodePDF(triplelist, true, user.VorUndNachname, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

                        MessageBox.Show("Die PDF Datei wurde unter: " + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\BoOp_PDF_Dateien\\{user.VorUndNachname}\\Ausweise gespeichert.", "Erfolg!");
                        StaticUserIDPrintList.Clear();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("PDF konnte nicht erstellt werden." + e.Message, "Fehler!");
                    }

                });

            //SortTitleCommand
            SortTitleCommand = new DelegateCommand(
                x =>
                {
                    if (!_titleFlag)
                    {
                        UpdateBooklist(Utils.SortedBookListByTitel(_currentList));
                        _titleFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByTitel(_currentList, true));
                        SetBookSortingFlagsFlase();
                    }
                });

            //SortAuthorCommand
            SortAuthorCommand = new DelegateCommand(
                x =>
                {
                    if (!_authorFlag)
                    {
                        UpdateBooklist(Utils.SortedBookListByAuthor(_currentList));
                        _authorFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByAuthor(_currentList, true));
                        SetBookSortingFlagsFlase();
                    }
                });

            //SortISBNCommand
            SortISBNCommand = new DelegateCommand(
                x =>
                {
                    if (!_isbnFlag)
                    {
                        UpdateBooklist(Utils.SortedBookListByISBN(_currentList));
                        _isbnFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByISBN(_currentList, true));
                        SetBookSortingFlagsFlase();
                    }
                });

            //Sort Vorname
            SortVornameCommand = new DelegateCommand(
                x =>
                {
                    if (!_vornameFlag)
                    {
                        UpdateUserlist(Utils.SortedUserlistByVorname(_currentUserList));
                        _vornameFlag = true;
                    }
                    else
                    {
                        UpdateUserlist(Utils.SortedUserlistByVorname(_currentUserList, true));
                        SetUserSortingFlagsFlase();
                    }
                });

            //Sort Nachname
            SortNachnameCommand = new DelegateCommand(
                x =>
                {
                    if (!_nachnameFlag)
                    {
                        UpdateUserlist(Utils.SortedUserlistByNachname(_currentUserList));
                        _nachnameFlag = true;
                    }
                    else
                    {
                        UpdateUserlist(Utils.SortedUserlistByNachname(_currentUserList, true));
                        SetUserSortingFlagsFlase();
                    }
                });

            //Sort Rechte
            SortRechteCommand = new DelegateCommand(
                x =>
                {
                    if (!_rechteFlag)
                    {
                        UpdateUserlist(Utils.SortedUserlistByRechte(_currentUserList));
                        _rechteFlag = true;
                    }
                    else
                    {
                        UpdateUserlist(Utils.SortedUserlistByRechte(_currentUserList, true));
                        SetUserSortingFlagsFlase();
                    }
                });

            SortLendedDate = new DelegateCommand(
                x =>
                {
                    if (!_lendeddateflag)
                    {
                        SortLendedListByDate(true);
                        _lendeddateflag = true;
                    }
                    else
                    {
                        SortLendedListByDate();
                        SetLendedSortingFlagsFalse();
                    }
                });

            SortLendedName = new DelegateCommand(
                x =>
                {
                    if (!_lendednameflag)
                    {
                        SortLendedListByName(true);
                        _lendednameflag = true;
                    }
                    else
                    {
                        SortLendedListByName();
                        SetLendedSortingFlagsFalse();
                    }
                });

            SearchTitleInAllBooks = new DelegateCommand(
                x =>
                {
                    UpdateBooklist(Utils.SearchForTitleInBooklist(_originalList, _allBookSearchWord));
                });

            ClearSearchInAllBooks = new DelegateCommand(
                x =>
                {
                    UpdateBooklist(_originalList);
                    AllBookSearchWord = "";
                });

            SearchVornameInUsers = new DelegateCommand(
                x =>
                {
                    UpdateUserlist(Utils.SearchVornameInUserlist(_originalUserList, _userSearchWord));
                });

            ClearSearchInUsers = new DelegateCommand(
                x =>
                {
                    UpdateUserlist(_originalUserList);
                    UserSearchWord = "";
                });

            SearchTitleInLendedBooks = new DelegateCommand(
                x =>
                {
                    UpdateLendedBooklist(Utils.SearchForTitleInBooklist(_originalList, _lendedBooksSearchWord));
                });

            ClearSearchInLendedBooks = new DelegateCommand(
                x =>
                {
                    UpdateLendedBooklist(_originalList);
                    LendedBooksSearchWord = "";
                });

            CloseApplicationCommand = new DelegateCommand(
                x =>
                {
                    if (MessageBox.Show("Wollen Sie die Anwendung Schließen?", "Schließen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Environment.Exit(0);
                    }
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });
        }

        public List<ExemplarViewModel> GetLendedBooksFromUser(PersonModel user)
        {
            var list = new List<ExemplarViewModel>();
            foreach (var book in BookList)
            {
                foreach (var exemplar in book.Model.Exemplare)
                {
                    if (exemplar.LendBy != null)
                    {
                        if (user.AusweisID == exemplar.LendBy.AusweisID)
                        {
                            list.Add(new ExemplarViewModel(_navigationService, exemplar, book.Model));
                        }
                    }
                    
                }
            }
            return list;
        }

        private void UpdateBooklist(ObservableCollection<BuchModel> booklist)
        {
            _currentList = booklist;
            BookList = new ObservableCollection<BookViewModel>();
            foreach (var book in booklist)
            {
                BookList.Add(new BookViewModel(book, _navigationService, _library, _user, this));
            }
        }

        private void UpdateUserlist(ObservableCollection<PersonModel> userlist)
        {
            _currentUserList = userlist;
            UserList = new ObservableCollection<PersonViewModel>();
            foreach (var user in userlist)
            {
                UserList.Add(new PersonViewModel(user, _navigationService, _library, _user, this));
            }
        }

        private void UpdateLendedBooklist(ObservableCollection<BuchModel> booklist)
        {
            LendedBookList = new ObservableCollection<ExemplarViewModel>();

            foreach (var book in booklist)
            {
                foreach (var exemplar in book.Exemplare)
                {
                    if (exemplar.LendBy != null)
                    {
                        LendedBookList.Add(new ExemplarViewModel(_navigationService, exemplar, book, this));
                    }
                }
            }
        }

        private void SetBookSortingFlagsFlase()
        {
            _isbnFlag = false;
            _authorFlag = false;
            _titleFlag = false; 
        }

        private void SetLendedSortingFlagsFalse()
        {
            _lendeddateflag = false; 
            _lendednameflag = false;
        }

        private void SetUserSortingFlagsFlase()
        {
            _vornameFlag = false;
            _nachnameFlag = false;
            _rechteFlag = false;
        }

        private void SortLendedListByDate(bool reverse = false)
        {
            ExemplarViewModel temp = null;

            for (int j = 0; j <= LendedBookList.Count - 2; j++)
            {
                for (int i = 0; i <= LendedBookList.Count - 2; i++)
                {
                    int comparison = DateTime.Compare(LendedBookList[i].Model.BasicInfos.AusleihDatumDateTime, LendedBookList[i + 1].Model.BasicInfos.AusleihDatumDateTime);

                    if (!reverse)
                    {
                        if (comparison > 0)
                        {
                            temp = LendedBookList[i + 1];
                            LendedBookList[i + 1] = LendedBookList[i];
                            LendedBookList[i] = temp;
                        }
                    }
                    else
                    {
                        if (comparison < 0)
                        {
                            temp = LendedBookList[i + 1];
                            LendedBookList[i + 1] = LendedBookList[i];
                            LendedBookList[i] = temp;
                        }
                    }
                }
            }
        }

        private void SortLendedListByName(bool reverse = false)
        {
            ExemplarViewModel temp = null;

            for (int j = 0; j <= LendedBookList.Count - 2; j++)
            {
                for (int i = 0; i <= LendedBookList.Count - 2; i++)
                {
                    int comparison = string.Compare(LendedBookList[i].Model.LendBy.Vorname + " " + LendedBookList[i].Model.LendBy.Nachname, LendedBookList[i + 1].Model.LendBy.Vorname + " " + LendedBookList[i].Model.LendBy.Nachname, comparisonType: StringComparison.InvariantCulture);

                    if (!reverse)
                    {
                        if (comparison > 0)
                        {
                            temp = LendedBookList[i + 1];
                            LendedBookList[i + 1] = LendedBookList[i];
                            LendedBookList[i] = temp;
                        }
                    }
                    else
                    {
                        if (comparison < 0)
                        {
                            temp = LendedBookList[i + 1];
                            LendedBookList[i + 1] = LendedBookList[i];
                            LendedBookList[i] = temp;
                        }
                    }
                }
            }
        }
    }
}