using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App22.Model
{
    public class Remainder : INotifyPropertyChanged
    {
        private string _RemainderName;
        public string RemainderName
        {
            get
            {
                return _RemainderName;
            }
            set
            {
                if (_RemainderName != value)
                {
                    _RemainderName = value;
                    OnPropetyChanged("RemainderName");
                }

            }
        }
        private DateTimeOffset _RemainderDate;
        public DateTimeOffset RemainderDate
        {
            get
            {
                return _RemainderDate;
            }
            set
            {
                _RemainderDate = value;
                OnPropetyChanged("RemainderDate");
            }
        }
        public User ReminaderUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropetyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}
