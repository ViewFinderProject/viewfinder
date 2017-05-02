using System;

namespace APIforEOSDIS
{
    /*
    Структура, котрая будет хранить координаты
    левой нижней (left lower - ll) и правой верхней (right upper - ru) точек прямоугольника,
    который используется в WorldviewLNK.DataforQuery для построения запроса.
    */

    public struct RectArea
    {
        private double x_ll, y_ll, x_ru, y_ru;
        private bool x_ll_sig, y_ll_sig, x_ru_sig, y_ru_sig;

        static private bool IsInRange(double val)
        {
            if (Math.Abs(val) >= 0 && Math.Abs(val) <= 790)
                return true;
            return false;
        }

        static private void SetVal(double val, out double num, out bool sig)
        {
            if (!IsInRange(val))
            {
                sig = false;
                num = 0;
                return;
            }

            sig = (val >= 0) ? false : true;
            num = Math.Abs(val);
        }

        static private bool IsValid(string val)
        {
            if (val == "")
                return false;

            bool WasPoint = false;

            for (int i = 0; i < val.Length - 1; i++)
            {
                if ((val[i] == '.') && (i != 0) && !WasPoint)
                    WasPoint = true;
                else if (val[i] < '0' || val[i] > '9')
                    return false;
            }

            byte last_val_ind = (byte)(val.Length - 1);

            if (!((val[last_val_ind] == 'n' || val[last_val_ind] == 's' || val[last_val_ind] == 'w' || val[last_val_ind] == 'e')
                || (val[last_val_ind] == 'N' || val[last_val_ind] == 'S' || val[last_val_ind] == 'W' || val[last_val_ind] == 'E'))
                || (val[last_val_ind - 1] < '0' || val[last_val_ind - 1] > '9'))
                return false;

            return true;
        }

        static private void SetVal(string val, out double num, out bool sig)
        {
            if (IsValid(val))
            {
                byte last_val_ind = (byte)(val.Length - 1);

                sig = (val[last_val_ind] == 'w' || val[last_val_ind] == 'W'
                    || val[last_val_ind] == 's' || val[last_val_ind] == 'S') ?
                    true : false;

                val = val.Remove(last_val_ind);
                num = Math.Abs(Convert.ToDouble(val));
                return;
            }

            sig = false;
            num = 0;
        }

        public double X_ll
        {
            get { return (x_ll_sig) ? -this.x_ll : this.x_ll; }
            set { SetVal(value, out x_ll, out x_ll_sig); }
        }

        public double Y_ll
        {
            get { return (y_ll_sig) ? -this.y_ll : this.y_ll; }
            set { SetVal(value, out y_ll, out y_ll_sig); }
        }

        public double X_ru
        {
            get { return (x_ru_sig) ? -this.x_ru : this.x_ru; }
            set { SetVal(value, out x_ru, out x_ru_sig); }
        }

        public double Y_ru
        {
            get { return (y_ru_sig) ? -this.y_ru : this.y_ru; }
            set { SetVal(value, out y_ru, out y_ru_sig); }
        }

        public RectArea(double x_ll_val, double y_ll_val, double x_ru_val, double y_ru_val)
        {
            SetVal(x_ll_val, out x_ll, out x_ll_sig);
            SetVal(y_ll_val, out y_ll, out y_ll_sig);
            SetVal(x_ru_val, out x_ru, out x_ru_sig);
            SetVal(y_ru_val, out y_ru, out y_ru_sig);
        }

        public RectArea(string x_ll_val, string y_ll_val, string x_ru_val, string y_ru_val)
        {
            SetVal(x_ll_val, out x_ll, out x_ll_sig);
            SetVal(y_ll_val, out y_ll, out y_ll_sig);
            SetVal(x_ru_val, out x_ru, out x_ru_sig);
            SetVal(y_ru_val, out y_ru, out y_ru_sig);
        }

        public void InitializeCoord(double x_ll_val, double y_ll_val, double x_ru_val, double y_ru_val)
        {
            this = new RectArea(x_ll_val, y_ll_val, x_ru_val, y_ru_val);
        }

        public void InitializeCoord(string x_ll_val, string y_ll_val, string x_ru_val, string y_ru_val)
        {
            this = new RectArea(x_ll_val, y_ll_val, x_ru_val, y_ru_val);
        }

        public override string ToString()
        {
            return X_ll.ToString() + "," + Y_ll.ToString() + "," + Y_ru.ToString() + "," + X_ru.ToString();
        }
    }
}
