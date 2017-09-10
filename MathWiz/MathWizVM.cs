using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MyMvvmLib;

namespace MathWiz
{
    public class ResultParams
    {
        string _strResult = "";
        int _seconds = 0;

        public string StrResult
        {
            get { return _strResult; }
            set { _strResult = value;  }
        }

        public int Seconds
        {
            get { return _seconds; }
            set { _seconds = value;  }
        }
    }

    class MathWizVM : ViewModelBase<MathWizModel>
    {
        public MathWizVM(MathWizModel model)
        {
            Model = model;
            UpdateLeftShapes();
            UpdateRightShapes();
            GetCorrectResult();
            ClownType = clownType.none;
        }

        // Next Problem command
        private ICommand _nextProblemCommand = null;

        private void NextProblem()
        {
            enumSign theSign = enumSign.addition;
            if (Sign == "-")
                theSign = enumSign.subtraction;
            else if (Sign == "x")
                theSign = enumSign.multiplication;
            else if (Sign == "÷")
                theSign = enumSign.division;

            ProblemArguments args = Model.GetProblemArguments(_minValue, _maxValue, theSign);

            LeftValue = args.left.ToString();
            RightValue = args.right.ToString();
            GetCorrectResult();
            ResetResult();
        }

        public ICommand NextProblemCommand
        {
            get
            {
                return _nextProblemCommand ?? (_nextProblemCommand =
                    new RelayCommand(() =>
                    {
                        NextProblem();
                    }));
            }
        }

        // MinValue
        int _minValue = 1;

        public string MinValue
        {
            get { return _minValue.ToString(); }
            set
            {
                try
                {
                    int newValue = Convert.ToInt32(value);
                    SetProperty(ref _minValue, newValue, "MinValue"  );
                }
                catch (System.Exception )
                {
                    SetProperty(ref _minValue, _minValue, "MinValue" );
                }

                ResetResult();
            }
        }

        // MaxValue
        int _maxValue = 10;

        public string MaxValue
        {
            get { return _maxValue.ToString(); }
            set
            {
                try
                {
                    int newValue = Convert.ToInt32(value);
                    SetProperty(ref _maxValue, newValue, "MaxValue");
                }
                catch (System.Exception)
                {
                    SetProperty(ref _maxValue, _maxValue, "MaxValue");
                }

                ResetResult();
            }
        }

        // LeftValue
        int _leftValue = 1;

        public string LeftValue
        {
            get
            {
                return _leftValue.ToString();
            }
            set
            {
                try
                {
                    int newValue = Convert.ToInt32(value);
                    SetProperty(ref _leftValue, newValue, "LeftValue");
                }
                catch (System.Exception)
                {
                    SetProperty(ref _leftValue, _leftValue, "LeftValue");
                }

                GetCorrectResult();
                UpdateLeftShapes();
                ResetResult();
            }
        }

        // RightValue
        int _rightValue = 2;

        public string RightValue
        {
            get
            {
                return _rightValue.ToString();
            }

            set
            {
                try
                {
                    int newValue = Convert.ToInt32(value);
                    if(Sign == "÷" && newValue == 0)
                        SetProperty(ref _rightValue, _rightValue, "RightValue");
                    else
                        SetProperty(ref _rightValue, newValue, "RightValue");
                }
                catch (System.Exception)
                {
                    SetProperty(ref _rightValue, _rightValue, "RightValue");
                }

                GetCorrectResult();
                UpdateRightShapes();
                ResetResult();
            }
        }

        // Sign
        string _sign = "+";

        public string Sign
        {
            get { return _sign; }
            set
            {
                SetProperty(ref _sign, value, () => Sign);
                NextProblem();
            }
        }

        // Result
        int _correctResult = 0;
        string _result = "";

        public string Result
        {
            get
            {
                return _result.ToString();
            }

            set
            {
                try
                {
                    int newValue = Convert.ToInt32(value);
                    SetProperty(ref _result, newValue.ToString(), ()=> Result);
                }
                catch (System.Exception)
                {
                    SetProperty(ref _result, "", "Result");
                }

                UpdateResultShapes();
                IsWrong = (_correctResult.ToString() != _result);
                ClownType = IsWrong ? clownType.sad : clownType.happy;
            }
        }

        // We don't want to set correct/incorrect
        private void ResetResult()
        {
            SetProperty(ref _result, "", () => Result);
            UpdateResultShapes();
            IsWrong = true;
            ClownType = clownType.none;
        }

        bool _isWrong = true;

        public bool IsWrong
        {
            get
            {
                return _isWrong;
            }

            set
            {
                SetProperty(ref _isWrong, value, ()=> IsWrong);

                ClownType = _isWrong ? clownType.sad : clownType.none;

                if (_result.Count() > 0)
                {
                    if (_isWrong)
                        ++_incorrect;
                    else
                        ++_correct;

                    SetScore();
                }
            }
        }

        clownType _clownType = clownType.none;

        public clownType ClownType
        {
            get
            {
                return _clownType;
            }

            set
            {
                SetProperty(ref _clownType, value, () => ClownType);
            }
        }

        // Equals Command
        private ICommand _equalsCommand = null;

        public ICommand EqualsCommand
        {
            get
            {
                return _equalsCommand ?? (_equalsCommand =
                    new RelayCommand(() =>
                    {
                        GetCorrectResult();       
                        SetProperty(ref _result, _correctResult.ToString(), () => Result);
                        UpdateResultShapes();
                    }));
            }
        }


        void GetCorrectResult()
        {
            enumSign theSign = enumSign.addition;
            if (Sign == "-")
                theSign = enumSign.subtraction;
            else if (Sign == "x")
                theSign = enumSign.multiplication;
            else if (Sign == "÷")
                theSign = enumSign.division;

            _correctResult = Model.GetProblemResult(_leftValue, _rightValue, theSign);
        }

        private ObservableCollection<ICountShape> _theLeftShapes;

        public ObservableCollection<ICountShape> LeftShapes
        {
            get
            {
                return _theLeftShapes;
            }

            set
            {
                SetProperty(ref _theLeftShapes, value, () => LeftShapes);
            }
        }

        void UpdateLeftShapes()
        {
            ObservableCollection<ICountShape> leftShapes = new ObservableCollection<ICountShape>();

            for(int i = 0; i < _leftValue; ++i)
                leftShapes.Add(new CountCircle());

            LeftShapes= leftShapes;
        }


        private ObservableCollection<ICountShape> _theRightShapes;

        public ObservableCollection<ICountShape> RightShapes
        {
            get
            {
                return _theRightShapes;
            }

            set
            {
                SetProperty(ref _theRightShapes, value, () => RightShapes);
            }
        }

        void UpdateRightShapes()
        {
            ObservableCollection<ICountShape> rightShapes = new ObservableCollection<ICountShape>();

            for (int i = 0; i < _rightValue; ++i)
                rightShapes.Add(new CountSquare());

            RightShapes = rightShapes;
        }

        private ObservableCollection<ICountShape> _theResultShapes;

        public ObservableCollection<ICountShape> ResultShapes
        {
            get
            {
                return _theResultShapes;
            }

            set
            {
                SetProperty(ref _theResultShapes, value, () => ResultShapes);
            }
        }

        void UpdateResultShapes()
        {
            ObservableCollection<ICountShape> resultShapes = new ObservableCollection<ICountShape>();

            int result = 0;
            try
            {
                result = Convert.ToInt32(_result);
            }
            catch (System.Exception)
            { }

            if (result != _correctResult)
                result = 0;

            if (Sign == "-")
            {
                for (int i = 0; i < result; ++i)
                    resultShapes.Add(new CountCircle() { ShapeColor = "Green" });
            }
            else if(Sign == "x")
            {
                for (int i = 0; i < result; ++i)
                    resultShapes.Add(new CountCircle() { ShapeColor = "Green" });
            }
            else if (Sign == "÷")
            {
                for (int i = 0; i < result; ++i)
                    resultShapes.Add(new CountCircle() { ShapeColor = "Green" });
            }
            else
            {
                if (result != 0)
                {
                    for (int i = 0; i < _leftValue; ++i)
                        resultShapes.Add(new CountCircle());

                    for (int i = 0; i < _rightValue; ++i)
                        resultShapes.Add(new CountSquare());
                }
            }

            ResultShapes = resultShapes;
        }

        int _correct = 0;
        int _incorrect = 0;
        int _score = 0;

        public int Score
        {
            get
            {
                return _score;
            }

            set
            {
                SetProperty(ref _score, value, () => Score);
            }
        }

        int _problemCount = 0;
        public int ProblemCount
        {
            get
            {
                return _problemCount;
            }

            set
            {
                SetProperty(ref _problemCount, value, () => ProblemCount);
            }
        }

        void SetScore()
        {
            int sum = _correct + _incorrect;
            if (sum == 0)
                Score = 0;
            else
                Score = (int) (100.0 * ((double)_correct) / sum + 0.4999);

            ProblemCount = sum;
        }

        // Equals Command
        private ICommand _resetCommand = null;

        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand ?? (_resetCommand =
                    new RelayCommand(() =>
                    {
                        _incorrect = 0;
                        _correct = 0;
                        SetScore();
                    }));
            }
        }

        // All of the following is used to result to typing in the result box

        // Sent when a user types in the result box
        private ICommand _resultTypedCommand = null;

        public ICommand ResultTypedCommand
        {
            get
            {
                return _resultTypedCommand ?? (_resultTypedCommand =
                    new RelayCommand<ResultParams>((resultParams) =>
                {
                    if (resultParams == null)
                        return;

                    bool bTimerWasRunning = (_resultTimer != null);

                    // if timer - kill it
                    if (_resultTimer != null)
                        KillTimer();

                    Equation lastEquation = new Equation(LeftValue, Sign, RightValue, resultParams.StrResult);

                    if (resultParams.Seconds == 0)
                    {
                        if (lastEquation == _equation && ! bTimerWasRunning)
                        {
                            if (NextProblemCommand.CanExecute(null))
                                NextProblemCommand.Execute(null);
                            
                            return;
                        }

                        _equation = lastEquation;
                        Result = resultParams.StrResult;
                        return;
                    }

                    _equation = lastEquation;

                    // set the new result field
                    _textResult = resultParams.StrResult;

                    // start the timer
                    // make timer
                    _resultTimer = new System.Windows.Threading.DispatcherTimer();

                    // start
                    _resultTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                    _resultTimer.Interval = new TimeSpan(0, 0, 0, resultParams.Seconds, 0);
                    _KillingTimer = false;

                    _resultTimer.Start();
                }));
            }
        }

        // temp result field
        private string _textResult;

        private bool _KillingTimer = false;

        private Equation _equation = new Equation();

        // timer
        private System.Windows.Threading.DispatcherTimer _resultTimer = null;

        // timer handler
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (_KillingTimer)
            {
                _KillingTimer = false;
                return;
            }

            KillTimer();

            if (_textResult != null && _textResult.Count() > 0)
                Result = _textResult;

            _textResult = "";
        }

        void KillTimer()
        {
            if (_resultTimer != null)
            {
                _resultTimer.Stop();
                _resultTimer = null;
                _KillingTimer = true;
            }
        }

    }
}
