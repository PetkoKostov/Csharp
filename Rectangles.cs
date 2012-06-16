using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

//F.N. 71246 Petko Kostov

namespace HomeWork_3_1_FMI
{
    class Program
    {
        class Point
        {
            internal double x;
            internal double y;

            public Point()
            {
                this.x = 0;
                this.y = 0;
            }

            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        class Rectangle
        {
            public Point a, b, c, d; // a=upper left, b= upper right, c=under a, d=under c

            public Rectangle()
            {
                this.a = new Point();
                this.b = new Point();
                this.c = new Point();
                this.d = new Point();
            }

            //Constructor for the streamreading
            public Rectangle(double ax, double ay, double bx, double by, double cx, double cy, double dx, double dy)
            {
                //lower left corner
                this.a = new Point(ax, ay);
                //upper right corner
                this.d = new Point(dx, dy);
                //upper left corner
                this.c = new Point(cx, cy);
                //lower right corner
                this.b = new Point(bx, by);

            }

            public Rectangle(Point c, double width, double height)
            {
                this.c = c;
                this.d = new Point(this.c.x + width, this.c.y);
                this.a = new Point(this.c.x, this.c.y - height);
                this.b = new Point(this.a.x + width, this.a.y);
                //This was not very smart , we can define only two points since it`s rectangle :)
            }

            public static Rectangle operator *(Rectangle r, int n)// r*2 <=> operator*(r,2)
            {
                Rectangle tmp = new Rectangle(r.c, (r.a.x) * n, (r.a.y) * n);
                return tmp;
            }

            public static Rectangle operator +(Rectangle r, Point p)// r+p <=> operator+(r,p)
            {
                double width = Math.Abs(r.b.x - r.a.x);
                double height = Math.Abs(r.c.y - r.a.y);
                Rectangle tmp = new Rectangle(p, width, height);
                return tmp;
            }

            public static Rectangle operator +(Rectangle r1, Rectangle r2)// r+p <=> operator+(r,p)
            {
                Point c = new Point();
                if (r1.c.y > r2.c.y) c = r1.c; //define upper left point
                else c = r2.c;
                Point b = new Point();
                if (r1.b.y < r2.b.y) b = r1.b;// define bottom right point
                else b = r2.b;
                double width = Math.Abs(c.x - b.x);
                double height = Math.Abs(c.y - b.y);
                Rectangle tmp = new Rectangle(c, width, height);
                return tmp;
            }
            public static void showRec(Rectangle r)
            {
                Console.WriteLine("The points of the rectangle are:A({0} , {1}), B({2} , {3}), C({4} , {5}), D({6} , {7})",
                  r.a.x, r.a.y, r.b.x, r.b.y, r.c.x, r.c.y, r.d.x, r.d.y);
            }
            public static double findArea(Rectangle r)
            {
                double width = Math.Abs(r.b.x - r.a.x);
                double height = Math.Abs(r.c.y - r.a.y);
                return width * height;
            }

            public static bool operator >(Rectangle r1, Rectangle r2)
            {
                return findArea(r1) > findArea(r2);
            }

            public static bool operator ==(Rectangle r1, Rectangle r2)
            {
                return findArea(r1) == findArea(r2);
            }

            public static bool operator !=(Rectangle r1, Rectangle r2)
            {
                return findArea(r1) != findArea(r2);
            }

            public static bool operator <(Rectangle r1, Rectangle r2)
            {
                return findArea(r1) < findArea(r2);
            }

            public Point this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0: return this.c;
                        case 1: return this.a;
                        case 2: return this.b;
                        case 3: return this.d;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                }
                set
                {
                    // I will comment the under section b/c in our homework we just want TO GET the point.
                    //switch (index)
                    //{
                    //    case 0: this.c = value; break;
                    //    case 1: this.a = value; break;
                    //    case 2: this.b = value; break;
                    //    case 3: this.d = value; break;
                    //    default:
                    //        throw new IndexOutOfRangeException();
                    //}
                }
            }


            public double this[int index1, int index2]
            {
                get
                {
                    switch (index1)
                    {
                        case 0: if (index2 == 0) return this.c.x; else return this.c.y;
                        case 1: if (index2 == 0) return this.a.x; else return this.a.y;
                        case 2: if (index2 == 0) return this.b.x; else return this.b.y;
                        case 3: if (index2 == 0) return this.d.x; else return this.d.y;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                }
                set
                {
                }
            }

        }



        static void Main()
        {
            Point p = new Point(2, 6);
            Rectangle r = new Rectangle(p, 3, 3);
            Rectangle r1 = r * 2;
            Rectangle.showRec(r);
            Console.WriteLine();
            Rectangle.showRec(r1);
            Point p1 = new Point(1, 8);
            Rectangle r2 = r + p1;
            Console.WriteLine();
            Rectangle.showRec(r2);
            Point p3 = new Point(3, 5);
            Rectangle r3 = new Rectangle(p3, 3, 7);
            Rectangle r4 = r1 + r3;
            Console.WriteLine();
            Rectangle.showRec(r4);
            Console.WriteLine();
            Console.WriteLine(r[0, 0]);

            // Create a StreamWriter instance
            StreamWriter writer = new StreamWriter("eto.txt");
            // Ensure the writer will be closed when no longer used
            using (writer)
            {
                Rectangle[] recArray = { r, r1, r2};
                // Loop through the rectangles and write them
                foreach (Rectangle rec in recArray)
                {
                    string tmp = Convert.ToString(rec.a.x) +" "+ Convert.ToString(rec.a.y)
                                    +" "+ Convert.ToString(rec.b.x) +" "+ Convert.ToString(rec.b.y)
                                    + " " + Convert.ToString(rec.c.x) + " " + Convert.ToString(rec.c.y)
                                    + " " + Convert.ToString(rec.d.x) + " " + Convert.ToString(rec.d.y);
                    writer.WriteLine(tmp);
                }
            }
            //And now the reading :}
            StreamReader sr = new StreamReader("eto.txt");
            using (sr)
            {
                double ax = sr.Read() - 48; sr.Read();// <- to ignore the white space
                double ay = sr.Read() - 48; sr.Read();
                double bx = sr.Read() - 48; sr.Read();
                double by = sr.Read() - 48; sr.Read();
                double cx = sr.Read() - 48; sr.Read();
                double cy = sr.Read() - 48; sr.Read();
                double dx = sr.Read() - 48; sr.Read();
                double dy = sr.Read() - 48; sr.Read();
                Rectangle r5 = new Rectangle(ax,ay, bx,by,cx, cy, dx, dy);
                Console.WriteLine("And the rectangle constructed from the .text file");
                Rectangle.showRec(r5);
            }
        }
    }
}