using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMWin10
{
    public class Person:INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if(_firstName!=value)
                {
                    _firstName = value;
                    OnPropertyChanged("FirstN ame");
                }
            }
        }
        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if(_lastName!=value)
                {
                    _lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private void OnPropertyChanged(string property)
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
