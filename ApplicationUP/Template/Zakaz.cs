using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApplicationUP.Template
{
    public class Zakaz
    {
        private int _count;
        public int count
        {
            get => _count;
            set => _count = value;
        }
        private string _number_documenta;
        public string number_documenta
        {   
            get => _number_documenta;
            set => _number_documenta = value;
        }
        private int _id_zakaza;
        public int id_zakaza
        {
            get => _id_zakaza;
            set => _id_zakaza = value;
        }


        private string _status;
       

        public string status
        {
            get => _status;
            set => _status = value;
        }
        private string _loginuser;


        public string loginuser
        {
            get => _loginuser;
            set => _loginuser = value;
        }

        private string _name_tovara;

        public string name_tovara
        {
            get => _name_tovara;
            set => _name_tovara = value;
        }
        private string _img_tavara;

        public string img_tavara
        {
            get => _img_tavara;
            set => _img_tavara = value;
        }

        private float _price_tovara;

        public float price_tovara
        {
            get => _price_tovara;
            set => _price_tovara = value;
        }
        private float _itogovaya_summa;

        public float itogovaya_summa
        {
            get => _itogovaya_summa;
            set => _itogovaya_summa = value;
        }

        private DateTime _date;

        public DateTime date
        {
            get => _date;
            set => _date = value;
        }
    }
}
