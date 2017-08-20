using System;
using System.Windows.Media;

namespace DrawWiz
{
    [Serializable]
    public class ColorWrapper
    {
        public ColorWrapper()
        {}

        public ColorWrapper(Color color)
        {
            StdColor = color;
        }

        private float _a = 255.0f;
        private float _r = 0.0f;
        private float _g = 0.0f;
        private float _b = 0.0f;

        public Color StdColor
        {
            get 
            {
                Color c = new Color();
                c.ScA = _a;
                c.ScR = _r;
                c.ScG = _g;
                c.ScB = _b;
                return c; 
            }
            set 
            {
                _a = value.ScA;
                _r = value.ScR;
                _g = value.ScG;
                _b = value.ScB;
            }
        }

        public float Alpha
        {
            get { return _a; }
            set { _a = value;  }
        }

        public float Red
        {
            get { return _r; }
            set { _r = value; }
        }

        public float Green
        {
            get { return _g; }
            set { _g = value; }
        }

        public float Blue
        {
            get { return _b; }
            set { _b = value; }
        }
    }
}
