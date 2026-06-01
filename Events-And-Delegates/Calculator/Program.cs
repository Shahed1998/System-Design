class Program
{

    delegate int MathOp(int x, int y);

    static int Add(int x, int y) => x + y;
    static int Sub(int x, int y) => x - y;
    static int Mul(int x, int y) => x * y;
    static int Div(int x, int y) => x / y;

    public static void Main()
    {
        MathOp op;

        op = Add;
        Console.WriteLine("Addition of 2 and 3 =" + op(2,3));
        
        op = Sub;
        Console.WriteLine("Subtration of 2 and 3 =" + op(2,3));
        
        op = Mul;
        Console.WriteLine("Multiplication of 2 and 3 =" + op(2,3));
        
        op = Div;
        Console.WriteLine("Division of 2 and 3 =" + op(2,3));
    }
}
