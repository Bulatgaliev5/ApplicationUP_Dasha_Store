using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUP.Template
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }

        private string _login;
        private string _parol;
        private string _numaber_tel;
        private int _role_id;
        private string _name;

        public int Role_id
        {
            get => _role_id;
            set => _role_id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Numaber_tel
        {
            get => _numaber_tel;
            set => _numaber_tel = value;
        }
        public string Parol
        {
            get => _parol;
            set => _parol = value;
        }
        public string Login
        {
            get => _login;
            set => _login = value;
        }
    }
}
