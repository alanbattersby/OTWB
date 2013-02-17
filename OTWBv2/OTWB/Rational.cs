using Geometric_Chuck.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometric_Chuck
{
    public class Rational : BindableBase
    {
        int _num;
        public int Numerator
        {
            get { return _num; }
            set { SetProperty(ref _num, value, "Numerator"); }
        }

        int _den;
        public int Denominator
        {
            get { return _den; }
            set { SetProperty(ref _den, value, "Denominator"); }
        }

        public Rational(int n, int d)
        {
            Numerator = n;
            Denominator = d;
            simplyfy();
        }

        public Rational(int n) : this(n, 1) { }
        public Rational() : this(1, 1) { }

        public Rational(double d, int dp)
        {
            int den = (int)Math.Pow(10, dp);
            int num = (int)(Math.Round(d, dp) * den) ;
            int hcf = BasicLib.HCF(num, den);
            Numerator = num / hcf;
            Denominator = den / hcf;
        }

        void simplyfy()
        {
            int hcf = BasicLib.HCF(Numerator, Denominator);
            Numerator /= hcf;
            Denominator /= hcf;
        }

        public double ToDouble
        {
            get { return Numerator * 1.0d / Denominator; }
        }

        // LCM of fractions = LCM of numerators/HCF of denominators
        // HCF of fractions = HCF of numerators/LCM of denominators

        public static Rational LCM(Rational r1, Rational r2)
        {
            int n = BasicLib.LCM(r1.Numerator, r2.Numerator);         
            int d = BasicLib.HCF(r1.Denominator, r2.Denominator);
            return new Rational(n,d);
        }

        public static Rational HCF(Rational r1, Rational r2)
        {
            int n = BasicLib.HCF(r1.Numerator, r2.Numerator);
            int d = BasicLib.LCM(r1.Denominator, r2.Denominator);
            return new Rational(n, d);
        }

        public static Rational Simplyfy(Rational r)
        {
            int hcf = BasicLib.HCF(r.Numerator, r.Denominator);
            return new Rational(r.Numerator / hcf, r.Denominator / hcf);
        }

        public override string ToString()
        {
            return string.Format("{0} / {1}", Numerator, Denominator);
        }

        public override bool Equals(Object o)
        {
            if (o is Rational)
            {
                Rational r = o as Rational;
                return (Numerator == r.Numerator) && (Denominator == r.Denominator);
            }
            return false;
        }

        public static Rational operator *(Rational r1, Rational r2)
        {
            return new Rational(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator);
        }

        public static Rational operator /(Rational r1, Rational r2)
        {
            return new Rational(r1.Numerator * r2.Denominator, r1.Denominator * r2.Numerator);
        }

        public static Rational operator +(Rational r1, Rational r2)
        {
            int n = r1.Numerator * r2.Denominator + r2.Numerator * r1.Denominator;
            int d = r1.Denominator * r2.Denominator;
            return new Rational(n, d);
        }

        public static Rational operator -(Rational r1, Rational r2)
        {
            int n = r1.Numerator * r2.Denominator - r2.Numerator * r1.Denominator;
            int d = r1.Denominator * r2.Denominator;
            return new Rational(n, d);
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + Numerator.GetHashCode();
            hash = hash * 23 + Denominator.GetHashCode();
            return hash;
        }
    }
}
