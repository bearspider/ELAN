using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EQAudioTriggers.Command
{
    public class OnDemandCommand : ICommand
    {
        private Action<object> executeOnDemandLoading;
        private Func<object, bool> canExecuteOnDemandLoading;

        public OnDemandCommand(Action<object> executeOnDemandLoading, Func<object, bool> canExecuteOnDemandLoading)
        {
            this.executeOnDemandLoading = executeOnDemandLoading;
            this.canExecuteOnDemandLoading = canExecuteOnDemandLoading;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.canExecuteOnDemandLoading(parameter);
        }

        public void Execute(object parameter)
        {
            this.executeOnDemandLoading(parameter);
        }
    }
}
