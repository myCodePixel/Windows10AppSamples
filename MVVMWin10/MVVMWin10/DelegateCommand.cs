using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMWin10
{
  public  class DelegateCommand:ICommand 
    {
        private readonly Action<object> _executeMethod = null;
        private readonly Predicate<object> _canExecuteMethod = null;
        private bool _isAutomaticRequeryDisabled = false;
        public DelegateCommand (Action<object> executeMethod,Predicate<object> canExecuteMethod,bool isAutomaticRequeryDisabled)
        { 
            if(executeMethod==null)
            {
                throw new ArgumentNullException("executeMethod is Null .. Plz set ExecuteMethod");

            }
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            _isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod == null ? true : _canExecuteMethod(parameter);
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            if(_executeMethod!=null)
            {
                _executeMethod(parameter);
            }
        }

        public void RaisecaznExecuteChanged()
        {
            if(CanExecuteChanged!=null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
