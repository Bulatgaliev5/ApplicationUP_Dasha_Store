using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUP.Template
{
    public class Reviews : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
        private string _nameUser;
        public string NameUser
        {
            get => _nameUser;
            set => _nameUser = value;
        }
        private string _comment;
        public string Comment
        {
            get => _comment;
            set => _comment = value;
        }
        private DateTime _Date_insert;
        public DateTime Date_insert
        {
            get => _Date_insert;
            set => _Date_insert = value;
        }

        private int _ozenka;
        public int ozenka
        {
            get => _ozenka;
            set => _ozenka = value;
        }
    }
}
