using System;

namespace BAMCIS.GeoJSON
{
    public interface ITouch<T>
    {
        bool Touches(T other, double eps = double.MinValue * 100);
    }

    public interface IIntersects<T>
    {
        bool Intersects(T other, double eps = double.MinValue * 100);
    }

    public interface IWithin<T>
    {
        bool Within(T other, double eps = double.MinValue * 100);
    }

    public interface IContains<T>
    {
        bool Contains(T other, double eps = double.MinValue * 100);
    }

    public interface IAdimTopology<T>: IEquatable<T>, ITouch<T>, IWithin<T> 
    {
       
  
    }

    public interface ILine<T>: IEquatable<T>, IIntersects<T>, ITouch<T>, IWithin<T>
    {
        
    }

    

    public interface ITopology<T>: ILine<T>, IContains<T>
    {

    }

    public interface IPolygon<T> : ITouch<LineString>, IContains<LineString>, IIntersects<LineString> { }
}