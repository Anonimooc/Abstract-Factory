using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        private IShapeFactory _currentFactory;
        private IShape _currentShape;

        public MainForm()
        {
            InitializeComponent();
            comboBoxShapes.SelectedIndexChanged += ComboBoxShapes_SelectedIndexChanged;
            comboBoxColors.SelectedIndexChanged += ComboBoxColors_SelectedIndexChanged;

            // Инициализация
            comboBoxColors.Items.Add("Red");
            comboBoxColors.Items.Add("Blue");
            comboBoxShapes.Items.Add("Circle");
            comboBoxShapes.Items.Add("Square");
            comboBoxShapes.Items.Add("Triangle");

            comboBoxColors.SelectedIndex = 0; // по умолчанию красный
            comboBoxShapes.SelectedIndex = 0; // по умолчанию круг

            UpdateFactory();
            CreateShape();
        }

        private void ComboBoxColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFactory();
            CreateShape();
            Invalidate(); // перерисовать форму
        }

        private void ComboBoxShapes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateShape();
            Invalidate(); // перерисовать форму
        }

        private void UpdateFactory()
        {
            string selectedColor = comboBoxColors.SelectedItem.ToString();
            if (selectedColor == "Red")
            {
                _currentFactory = new RedFactory();
            }
            else
            {
                _currentFactory = new BlueFactory();
            }
        }

        private void CreateShape()
        {
            // проверка, что выбран элемент в comboBoxShapes
            if (comboBoxShapes.SelectedItem == null)
            {
                _currentShape = null;
                return;
            }

            string selectedShape = comboBoxShapes.SelectedItem.ToString();

            // проверка, что _currentFactory не равен null
            if (_currentFactory == null)
            {
                _currentShape = null;
                return;
            }

            switch (selectedShape)
            {
                case "Circle":
                    _currentShape = _currentFactory.CreateCircle();
                    break;
                case "Square":
                    _currentShape = _currentFactory.CreateSquare();
                    break;
                case "Triangle":
                    _currentShape = _currentFactory.CreateTriangle();
                    break;
                default:
                    _currentShape = null;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _currentShape?.Draw(e.Graphics); // рисуем текущую фигуру
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public interface IShape
    {
        void Draw(Graphics g);
    }

    public class Circle : IShape
    {
        public Color Color { get; set; }

        public Circle(Color color)
        {
            Color = color;
        }

        public void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(Color))
            {
                g.FillEllipse(brush, 150, 150, 100, 100);
            }
        }
    }

    public class Square : IShape
    {
        public Color Color { get; set; }

        public Square(Color color)
        {
            Color = color;
        }

        public void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(Color))
            {
                g.FillRectangle(brush, 150, 150, 100, 100);
            }
        }
    }

    public class Triangle : IShape
    {
        public Color Color { get; set; }

        public Triangle(Color color)
        {
            Color = color;
        }

        public void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(Color))
            {
                Point[] points = { new Point(120, 250), new Point(180, 150), new Point(240, 250) };
                g.FillPolygon(brush, points);
            }
        }
    }

    public interface IShapeFactory
    {
        IShape CreateCircle();
        IShape CreateSquare();
        IShape CreateTriangle();
    }

    public class RedFactory : IShapeFactory
    {
        public IShape CreateCircle() => new Circle(Color.Red);
        public IShape CreateSquare() => new Square(Color.Red);
        public IShape CreateTriangle() => new Triangle(Color.Red);
    }

    public class BlueFactory : IShapeFactory
    {
        public IShape CreateCircle() => new Circle(Color.Blue);
        public IShape CreateSquare() => new Square(Color.Blue);
        public IShape CreateTriangle() => new Triangle(Color.Blue);
    }
}
