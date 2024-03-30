using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApplicationUP.Template
{
    public class Goods
    {


        private int _count;
        public int count
        {
            get => _count;
            set
            {
                if (value <= 0)
                    value = 1;

                _count = value;
            }
        }
        // Поле ID
        private int _id_tovar;
        private string _name;
        private string _Desc;
        private float _Cost;
        private int _Discount;
        private string _Img;
        private int _V_nalichii;
        private bool _CanEdit;
        private Visibility _CanVisible;
        private bool _CanZakaz;
        private Visibility _CanZakazVisible;


        /// <summary>
        /// Свойство ID
        /// </summary>
        public int ID
        {
            get => _id_tovar;
            set => _id_tovar = value;
        }

        /// <summary>
        /// Свойство Названия товара
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Desc
        {
            get => _Desc;
            set => _Desc = value;
        }

        public float Cost
        {
            get => _Cost;
            set
            {
                if (value < 0)
                    value = 0;

                _Cost = value;
            }
        }

        public int Discount
        {
            get => _Discount;
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 100)

                _Discount = value;
            }
        }

        public string IMG
        {
            get => _Img;
            set => _Img = value;
        }

        public int CountInContainer
        {
            get => _V_nalichii;
            set => _V_nalichii = value;
        }
        public bool CanEdit
        {
            get => _CanEdit;
            set => _CanEdit = value;
        }
        public Visibility CanVisible
        {
            get => _CanVisible;
            set => _CanVisible = value;
        }

        public bool CanZakaz
        {
            get => _CanZakaz;
            set => _CanZakaz = value;
        }
        public Visibility CanZakazVisible
        {
            get => _CanZakazVisible;
            set => _CanZakazVisible = value;
        }
    }
}
