using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMWin10
{
   public  class PersonViewModel
    {
        public ObservableCollection<Person> Persons
        {
            get;set;
        }
        public Person personObject { get; set; } 
        private ICommand _savePersonCommand;
        public ICommand SavePersonCommand
        {
            get
            {
                return _savePersonCommand;
            }
            set
            {
                _savePersonCommand = value;
            }
        }
        public PersonViewModel()
        {
            ObservableCollection<Person> _Persons = new ObservableCollection<Person>();
            _Persons.Add(new Person { FirstName = "Trilok", LastName = "Chowdary" });
            _Persons.Add(new Person { FirstName = "Abhi", LastName = "Ram" });
            Persons = _Persons;
            personObject = new Person { FirstName = "Mickey", LastName = "Mouse" };
            _savePersonCommand = new DelegateCommand(SavePerson, CanSavePerson, false);
        }

        private bool CanSavePerson(object obj)
        {
           if(string.IsNullOrEmpty(personObject.FirstName)||string.IsNullOrEmpty(personObject.LastName))
            { return false; }
            else { return true; }

        }

        private void SavePerson(object obj)
        {
            Persons.Add(new Person { FirstName = personObject.FirstName, LastName = personObject.LastName });
        }

        public void LoadPersons()
        {
            ObservableCollection<Person> _Persons = new ObservableCollection<Person>();
            _Persons.Add(new Person { FirstName = "Trilok", LastName = "Chowdary" });
            _Persons.Add(new Person { FirstName = "Abhi", LastName = "Ram" });
            Persons = _Persons;
        }

    }
}
