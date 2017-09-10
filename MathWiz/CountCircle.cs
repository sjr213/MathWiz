namespace MathWiz
{
    public class CountCircle : ICountShape
    {
        private string _shapeColor = "Blue";
        public string ShapeColor
        {
            get { return _shapeColor; }
            set { _shapeColor = value;  }
        }
    }
}
