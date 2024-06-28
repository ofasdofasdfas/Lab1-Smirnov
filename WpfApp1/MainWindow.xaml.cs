using System;
using System.Windows;

namespace PairExample
{

    public abstract class Pair
    {
        public virtual Pair Add(Pair other)
        {
            throw new NotImplementedException("Метод Add не реализован в базовом классе Pair.");
        }

        public virtual Pair Subtract(Pair other)
        {
            throw new NotImplementedException("Метод Subtract не реализован в базовом классе Pair.");
        }

        public virtual Pair Multiply(Pair other)
        {
            throw new NotImplementedException("Метод Multiply не реализован в базовом классе Pair.");
        }

        public virtual Pair Divide(Pair other)
        {
            throw new NotImplementedException("Метод Divide не реализован в базовом классе Pair.");
        }

        public virtual void Print()
        {
            Console.WriteLine("Неизвестный тип пары.");
        }
    }

    public class FazzyNumber : Pair
    {
        public double Value { get; set; }
        public double Error { get; set; }

        public FazzyNumber(double value, double error)
        {
            Value = value;
            Error = error;
        }

        public override Pair Add(Pair other)
        {
            if (other is FazzyNumber fn)
            {
                return new FazzyNumber(Value + fn.Value, Math.Sqrt(Error * Error + fn.Error * fn.Error));
            }
            else
            {
                throw new ArgumentException("Неверный тип для операции сложения.");
            }
        }

        public override Pair Subtract(Pair other)
        {
            if (other is FazzyNumber fn)
            {
                return new FazzyNumber(Value - fn.Value, Math.Sqrt(Error * Error + fn.Error * fn.Error));
            }
            else
            {
                throw new ArgumentException("Неверный тип для операции вычитания.");
            }
        }

        public override Pair Multiply(Pair other)
        {
            if (other is FazzyNumber fn)
            {
                return new FazzyNumber(Value * fn.Value, Math.Abs(Value * fn.Error + Error * fn.Value));
            }
            else
            {
                throw new ArgumentException("Неверный тип для операции умножения.");
            }
        }

        public override Pair Divide(Pair other)
        {
            if (other is FazzyNumber fn)
            {
                if (fn.Value == 0)
                {
                    throw new DivideByZeroException("Деление на ноль.");
                }
                return new FazzyNumber(Value / fn.Value, Math.Abs(Value * fn.Error + Error * fn.Value) / (fn.Value * fn.Value));
            }
            else
            {
                throw new ArgumentException("Неверный тип для операции деления.");
            }
        }

        public override void Print()
        {
            Console.WriteLine($"Нечеткое число: {Value} ± {Error}");
        }
    }

    public class Complex : Pair
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public Complex(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public override Pair Add(Pair other)
        {
            if (other is Complex c)
            {
                return new Complex(Real + c.Real, Imaginary + c.Imaginary);
            }
            else
            {
                throw new ArgumentException("Неверный тип для операции сложения.");
            }
        }

        public override Pair Subtract(Pair other)
        {
            if (other is Complex c)
            {
                return new Complex(Real - c.Real, Imaginary - c.Imaginary);
            }
            else
            {
                throw new
ArgumentException("Неверный тип для операции вычитания.");
            }
        }

        public override Pair Multiply(Pair other)
        {
            if (other is Complex c)
            {
                return new Complex(Real * c.Real - Imaginary * c.Imaginary, Real * c.Imaginary + Imaginary * c.Real);
            }
            else
            {
                throw new ArgumentException("Неверный тип для операции умножения.");
            }
        }

        public override Pair Divide(Pair other)
        {
            if (other is Complex c)
            {
                if (c.Real == 0 && c.Imaginary == 0)
                {
                    throw new DivideByZeroException("Деление на ноль.");
                }
                double denominator = c.Real * c.Real + c.Imaginary * c.Imaginary;
                return new Complex((Real * c.Real + Imaginary * c.Imaginary) / denominator, (Imaginary * c.Real - Real * c.Imaginary) / denominator);
            }
            else
            {
                throw new ArgumentException("Неверный тип для операции деления.");
            }
        }

        public override void Print()
        {
            Console.WriteLine($"Комплексное число: {Real} + {Imaginary}i");
        }
    }
}

namespace PairExample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            PerformOperation(PairOperation.Add);
        }

        private void Button_Click_Subtract(object sender, RoutedEventArgs e)
        {
            PerformOperation(PairOperation.Subtract);
        }

        private void Button_Click_Multiply(object sender, RoutedEventArgs e)
        {
            PerformOperation(PairOperation.Multiply);
        }

        private void Button_Click_Divide(object sender, RoutedEventArgs e)
        {
            PerformOperation(PairOperation.Divide);
        }

        private void PerformOperation(PairOperation operation)
        {
            try
            {
                // Получение значений из TextBox
                double fn1Value = double.Parse(FazzyNumber1Value.Text);
                double fn1Error = double.Parse(FazzyNumber1Error.Text);
                double fn2Value = double.Parse(FazzyNumber2Value.Text);
                double fn2Error = double.Parse(FazzyNumber2Error.Text);

                double complex1Real = double.Parse(Complex1Real.Text);
                double complex1Imaginary = double.Parse(Complex1Imaginary.Text);
                double complex2Real = double.Parse(Complex2Real.Text);
                double complex2Imaginary = double.Parse(Complex2Imaginary.Text);

                // Создание объектов FazzyNumber и Complex
                FazzyNumber fn1 = new FazzyNumber(fn1Value, fn1Error);
                FazzyNumber fn2 = new FazzyNumber(fn2Value, fn2Error);
                Complex c1 = new Complex(complex1Real, complex1Imaginary);
                Complex c2 = new Complex(complex2Real, complex2Imaginary);

                // Выполнение операции
                Pair result = null;
                switch (operation)
                {
                    case PairOperation.Add:
                        result = fn1.Add(fn2);
                        break;
                    case PairOperation.Subtract:
                        result = fn1.Subtract(fn2);
                        break;
                    case PairOperation.Multiply:
                        result = fn1.Multiply(fn2);
                        break;
                    case PairOperation.Divide:
                        result = fn1.Divide(fn2);
                        break;
                }

                if (result is FazzyNumber fn)
                {
                    ResultText.Text = $"Результат: {fn.Value} ± {fn.Error}";
                }
                else if (result is Complex c)
                {
                    ResultText.Text = $"Результат: {c.Real} + {c.Imaginary}i";
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = $"Ошибка: {ex.Message}";
            }
        }

        private enum PairOperation
        {
            Add,
            Subtract,
            Multiply,
            Divide
        }
    }
}
