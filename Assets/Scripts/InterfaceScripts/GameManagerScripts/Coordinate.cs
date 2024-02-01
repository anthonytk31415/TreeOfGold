// represents coordinates for the board
using System; 

public class Coordinate
{
    public int x; 
    public int y; 

    public Coordinate(int x, int y)
    {    
        this.x = x;
        this.y = y;
    }

    public int GetX()
    {
        return x; 
    }

    public int GetY() 
    {
        return y; 
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Coordinate coordObj = (Coordinate)obj;
        return this.GetX() == coordObj.GetX() && this.GetY() == coordObj.GetY();
    }
    // review the mechanics of this Value
    public int Value { get; set; }
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return string.Format("Coordinate with x: {0}, y: {1}", x, y);
    }

}
