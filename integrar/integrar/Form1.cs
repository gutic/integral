using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace derive
{

    public partial class Form1 : Form
    {
        public double A; 
        public double B; 
        public double funcionA;
        public double funcionB;
        public double resultado;
        public string result;
        public string funcion;
        public int cuanto;
        public int contador = 0;
        public int TotalMono = 0;
        public char[] grado = { 'x', '^' };
        public string submono;
        public string [] parte;
        public int TotalParte;
        public int MaxGrado;
        public int multi;
        
        public Form1()
        {
            InitializeComponent();
            panel1.BackColor = Color.Black;
        }
        System.Drawing.Pen Lapiz = new System.Drawing.Pen(System.Drawing.Color.Blue);

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics formGraphics = panel1.CreateGraphics();
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text) ||  string.Format(textBox1.Text) == "Ingrese f(x) aquí") {
                MessageBox.Show("Ingrese una funcion! \n");
            }
            else
            {
                funcion = (textBox1.Text);
                if (textBox5.Text == "A" || textBox6.Text == "B")
                {
                    MessageBox.Show("Ingrese valores para A y B!");
                }
                else { 
                    A = Convert.ToDouble(textBox5.Text);
                    B = Convert.ToDouble(textBox6.Text);
                    if (A == B)
                    {
                        textBox2.Text = Convert.ToString("0");
                    }
                    else
                    {
                        textBox2.Text = Convert.ToString(integrar(funcion));
                        resultado = funcionB - funcionA;
                        result =  Convert.ToString(resultado);
                        textBox10.Text=Convert.ToString(result);
                        funcionA = 0;
                        funcionB = 0;
                    }
                }

            }

        }

        public string integrar(string funcion)
        {
            string salida = "";
            string[] terminos;
            string dx;
            bool EsPositivo = true;
            double potencia = 0, coeficiente = 0;
            funcion = formato(funcion); //separo -  +
            terminos = funcion.Split();
            foreach (string termino in terminos)
            {
                    separar(termino,ref coeficiente,ref potencia,ref EsPositivo);
                    dx = integrarTermino(coeficiente, potencia, EsPositivo);
                    salida += dx;
            }
            if  (!(salida.Trim().Length > 0))
            {
                return "x";
            }
            if (salida[1] == '+')
            {
                return salida.Substring(3);
            }
            if (salida[1] == '-')
            {
                return Convert.ToString(salida[1]) + salida.Substring(3);
            }
            if (salida[0] == ' ')
            {
                return salida.Substring(1);
            }
            return salida;
        }
        public string formato(string funcion)//separo estilo  1 +2 -6
        {
            string salir = "";
            int j = 0;
            for (int i = 0; i < funcion.Length - 1; i++)
            {
                if ((funcion[i] == '-' || funcion[i] == '+') && i > 0)
                {
                    if (funcion[i - 1] != '^')
                    {
                        salir += funcion.Substring(j, i - j) + " "; //concateno y agrego espacio antes del + o -
                        j = i;
                    }
                }
            }
            salir += funcion.Substring(j, funcion.Length - j);
            return salir;
        }
        private void separar(string termino,ref double coeficiente,ref double potencia,ref bool EsPositivo)
        {
            int fincoeficiente = 0;
            int inicio;
            EsPositivo = true;
            if (termino[0] == '-')
            {
                EsPositivo = false;
                inicio = 1;
            } 
            else if (termino[0] == '+')
            {
                EsPositivo = true;
                inicio = 1;
            }
            else
            {
                inicio = 0;
            }
            if(termino == Convert.ToString('x'))
            {
                coeficiente = 1; //cuento numero de coeficientes
            }
            else
            {
                for(int i=0;i< termino.Length -1; i++)
                {
                    if (!(char.IsNumber(termino[i])))
                    {
                        fincoeficiente = i - 1; //veo donde termina el coeficiente
                        break;
                    }
                }
                string c;
                if (fincoeficiente <= 0)
                {
                    c = termino.Substring(inicio, 1);
                    
                }
                else if(inicio == 0)
                {
                    c = termino.Substring(inicio, fincoeficiente + 1);
                    
                }
                else
                {
                    c = termino.Substring(inicio, fincoeficiente);
                    
                }
                if (c == Convert.ToString('x') || c.Length == 0)
                {
                    coeficiente = 1;
                }
                else
                {
                    if (Convert.ToString(c) != string.Empty)
                    {
                        //MessageBox.Show(c);
                        double valor;
                        double.TryParse(c, out valor);
                        //MessageBox.Show(Convert.ToString(valor));
                        coeficiente = valor;
                    }
                }
            }
            //saco la potencia
            if (termino.IndexOf("^") >= 0 && termino.IndexOf("(") < 0)
            {
                potencia = Convert.ToInt32(termino.Substring(termino.IndexOf("^") + 1));
            }
            else if(termino.IndexOf("x") >=0 && termino.IndexOf("^") < 0)
            {
                potencia = 1;
            }
            //fraccion no funciona, retorno//
            else if (termino.IndexOf(")") >= 0) 
            {
                string fraccion;
                inicio = termino.IndexOf("(") + 1;
                fraccion = termino.Substring(inicio, (termino.Length - inicio) - 1);
                potencia = fraccionADecimal(fraccion);
            }
            else
            {
                potencia = 0;
            }
        }
        private Single fraccionADecimal(string fraccion)
        {
            int primer, segundo;
            primer = Convert.ToInt32(fraccion.Substring(0, fraccion.IndexOf("/")));
            segundo = Convert.ToInt32(fraccion.Substring(fraccion.IndexOf("/") + 1));
            //MessageBox.Show(Convert.ToString(primer / segundo));
            return primer / segundo;
        }
        private string integrarTermino(double coeficiente, double potencia, bool EsPositivo)
        {
            string salida = "";
            double termino;
            if (!(EsPositivo))
            {
                if (potencia < 0)
                {
                    salida += " + ";
                }
                else
                {
                    salida += " - ";
                }
            }
            else if (potencia >= 0)
            {
                salida += " + ";
            }
            else
            {
                salida += " - ";
            }
            termino = Math.Abs(coeficiente);
            Convert.ToDouble(A);
            Convert.ToDouble(potencia);
            funcionA += (coeficiente * (Math.Pow(Convert.ToDouble(A), Convert.ToDouble(potencia + 1)))) / (potencia + 1);
            funcionB += (coeficiente * (Math.Pow(Convert.ToDouble(B), Convert.ToDouble(potencia+1)))) / (potencia+1);
            
            switch (Convert.ToInt32(potencia))
            {
                case 1:
                    return "(" + salida + termino + "x^" + (potencia + 1) + ")" + "/" + "(" + (potencia + 1) + ")";
                    
                case 2:
                    return "(" + salida + termino + "x^" + (potencia + 1) + ")" + "/" + "(" + (potencia + 1) + ")";
                case 0:
                    return salida + "x";

                default:
                    if (termino != 1)
                    {
                        return "(" + salida + termino + "x^" + (potencia + 1) + ")" + "/" + "(" + (potencia + 1) + ")"; ;
                    }
                    else
                    {
                        return salida + "x^" + (potencia - 1);
                    }
            }
        }









        //BARRA SEPARADORA DE BAJO PRESUPUESTO BARRA SEPARADORA DE BAJO PRESUPUESTO BARRA SEPARADORA DE BAJO PRESUPUESTO BARRA SEPARADORA DE BAJO PRESUPUESTO 
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)      //dibujo el grafico + para la funcion
        {
            //int xcentro = panel1.Width / 2;
            //int ycentro = panel1.Height / 2;
            //e.Graphics.TranslateTransform(xcentro, ycentro);
            //e.Graphics.ScaleTransform(1, - 1);
            //e.Graphics.DrawLine(Lapiz, xcentro * -1, 0, xcentro * 2, 0); //ejex
            //e.Graphics.DrawLine(Lapiz, 0, ycentro, 0, ycentro * -1); // ejey
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
