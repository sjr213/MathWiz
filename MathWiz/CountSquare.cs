namespace MathWiz
{
    public class CountSquare : ICountShape
    {
        private string _shapeColor = "Red";
        public string ShapeColor
        {
            get { return _shapeColor; }
            set { _shapeColor = value; }
        }
    }
}
