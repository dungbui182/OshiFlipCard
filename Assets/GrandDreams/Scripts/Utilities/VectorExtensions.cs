using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GrandDreams.Core.Utilities
{
    public static class VectorExtensions
    {
        public static Vector2 Half = new Vector2(0.5f, 0.5f);
        public static Vector3 Half3D = new Vector3(0.5f, 0.5f, 0.5f);

        public static Vector2 SetX(this Vector2 vector, float valueX)
        {
            vector.x = valueX;
            return vector;
        }

        public static Vector2 SetY(this Vector2 vector, float valueY)
        {
            vector.y = valueY;
            return vector;
        }

        public static Vector3 SetX(this Vector3 vector, float valueX)
        {
            vector.x = valueX;
            return vector;
        }

        public static Vector3 SetY(this Vector3 vector, float valueY)
        {
            vector.y = valueY;
            return vector;
        }

        public static Vector3 SetZ(this Vector3 vector, float valueZ)
        {
            vector.z = valueZ;
            return vector;
        }

        public static Vector2 AddX(this Vector2 vector, float valueX)
        {
            vector.x += valueX;
            return vector;
        }

        public static Vector2 AddY(this Vector2 vector, float valueY)
        {
            vector.y += valueY;
            return vector;
        }

        public static Vector3 AddX(this Vector3 vector, float valueX)
        {
            vector.x += valueX;
            return vector;
        }

        public static Vector3 AddY(this Vector3 vector, float valueY)
        {
            vector.y += valueY;
            return vector;
        }

        public static Vector3 AddZ(this Vector3 vector, float valueZ)
        {
            vector.z += valueZ;
            return vector;
        }

        public static Vector2 MulX(this Vector2 vector, float valueX)
        {
            vector.x *= valueX;
            return vector;
        }

        public static Vector2 MulY(this Vector2 vector, float valueY)
        {
            vector.y *= valueY;
            return vector;
        }

        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y);
        }

        public static Vector2Int ToVector2Int(this Vector2 vector)
        {
            return new Vector2Int((int)vector.x, (int)vector.y);
        }

        public static Vector2 GetIntersectionPoint(this Vector2 calculatingPoint, Vector2 pointInLine1, Vector2 pointInLine2, ref bool isCrossLine)
        {
            float dX = pointInLine2.x - pointInLine1.x;
            float dY = pointInLine2.y - pointInLine1.y;
            float dtX = calculatingPoint.x - pointInLine1.x;
            float dtY = calculatingPoint.y - pointInLine1.y;

            isCrossLine = (dX * dtY - dY * dtX) >= 0;

            float u = Mathf.Lerp(0, 1, (dtX * dX + dtY * dY) / (dX * dX + dY * dY));

            return new Vector2(pointInLine1.x + (u * dX), pointInLine1.y + (u * dY));
        }

        public static bool CrossLine(this Vector2 calculatingPoint, Vector2 pointInLine1, Vector2 pointInLine2)
        {
            float dX = pointInLine2.x - pointInLine1.x;
            float dY = pointInLine2.y - pointInLine1.y;
            float dtX = calculatingPoint.x - pointInLine1.x;
            float dtY = calculatingPoint.y - pointInLine1.y;

            return (dY * dtX - dX * dtY) > 0;
        }

        public static Vector2 GetCentroid(params Vector2[] corners)
        {
            int counter = 0;
            Vector2 sum = Vector2.zero;

            for (int index = 0; index < corners.Length; index++)
            {
                sum += corners[index];
                counter++;
            }

            return sum / counter;
        }

        public static Vector2 AddRandomVector2(this Vector2 vector, Vector2 parameterRandomDistance, Vector2? boundingRectSize = null)
        {
            Vector2 addingRandomDistance = new Vector2(UnityEngine.Random.Range(-parameterRandomDistance.x, parameterRandomDistance.x), UnityEngine.Random.Range(-parameterRandomDistance.y, parameterRandomDistance.y));
            Vector2 resultPos = vector + addingRandomDistance;

            if (boundingRectSize.HasValue)
            {
                Vector2 bounding = boundingRectSize.Value;
                while (!(resultPos.x > -bounding.x && resultPos.x < bounding.x && resultPos.y > -bounding.y && resultPos.y < bounding.y))
                {
                    addingRandomDistance = new Vector2(UnityEngine.Random.Range(-parameterRandomDistance.x, parameterRandomDistance.x), UnityEngine.Random.Range(-parameterRandomDistance.y, parameterRandomDistance.y));
                    resultPos = vector + addingRandomDistance;
                }
            }

            return resultPos;
        }

        public static bool IsInsideRectangle(this Vector2 checkingPoint, Vector2 rectanglePosition, Vector2 size)
        {
            if(checkingPoint.x <= rectanglePosition.x + size.x && checkingPoint.x >= rectanglePosition.x - size.x && checkingPoint.y <= rectanglePosition.y + size.y && checkingPoint.y >= rectanglePosition.y - size.y)
            {
                return true;
            }
            return false;
        }

        public static bool IsInsideEllipse(this Vector2 checkingPoint, Vector2 ellipsePosition, Vector2 size)
        {
            if (size.x == size.y)
            {
                return Vector2.Distance(checkingPoint, ellipsePosition) <= size.x;
            }
            else
            {
                float val = (Mathf.Pow((checkingPoint.x - ellipsePosition.x), 2) / Mathf.Pow(size.x, 2)) + (Mathf.Pow((checkingPoint.y - ellipsePosition.y), 2) / Mathf.Pow(size.y, 2));
                return val <= 1;
            }
        }

        //Checking point inside polygon
        private const int INF = 100000;
        public static bool IsInsidePolygon(this Vector2 checkingPoint, Vector2[] polygon)
        {
            int n = polygon.Length;

            if (n < 3)
            {
                return false;
            }

            Vector2 extreme = new Vector2(INF, checkingPoint.y);

            int count = 0, i = 0;
            do
            {
                int next = (i + 1) % n;

                // Check if the line segment from 'p' to
                // 'extreme' intersects with the line
                // segment from 'polygon[i]' to 'polygon[next]'
                if (DoIntersect(polygon[i], polygon[next], checkingPoint, extreme))
                {
                    // If the point 'p' is collinear with line
                    // segment 'i-next', then check if it lies
                    // on segment. If it lies, return true, otherwise false
                    if (Orientation(polygon[i], checkingPoint, polygon[next]) == 0)
                    {
                        return OnSegment(polygon[i], checkingPoint, polygon[next]);
                    }
                    count++;
                }
                i = next;
            } while (i != 0);

            // Return true if count is odd, false otherwise
            return (count % 2 == 1); // Same as (count%2 == 1)
        }

        private static bool OnSegment(Vector2 startPoint, Vector2 middlePoint, Vector2 endPoint)
        {
            if (middlePoint.x <= Math.Max(startPoint.x, endPoint.x) &&
                middlePoint.x >= Math.Min(startPoint.x, endPoint.x) &&
                middlePoint.y <= Math.Max(startPoint.y, endPoint.y) &&
                middlePoint.y >= Math.Min(startPoint.y, endPoint.y))
            {
                return true;
            }
            return false;
        }

        private static int Orientation(Vector2 startPoint, Vector2 middlePoint, Vector2 endPoint)
        {
            float val = (middlePoint.y - startPoint.y) * (endPoint.x - middlePoint.x) - (middlePoint.x - startPoint.x) * (endPoint.y - middlePoint.y);

            if (val == 0)
            {
                return 0; // collinear
            }
            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        //private static bool Orientation(Vector2 startPoint, Vector2 middlePoint, Vector2 endPoint)
        //{
        //    return Vector3.Cross(endPoint - startPoint, middlePoint - startPoint) == Vector3.zero;
        //}

        private static bool DoIntersect(Vector2 currentVertex, Vector2 nextVertex, Vector2 checkingPoint, Vector2 extreme)
        {
            // Find the four orientations needed for
            // general and special cases
            int o1 = Orientation(currentVertex, nextVertex, checkingPoint);
            int o2 = Orientation(currentVertex, nextVertex, extreme);
            int o3 = Orientation(checkingPoint, extreme, currentVertex);
            int o4 = Orientation(checkingPoint, extreme, nextVertex);

            // General case
            if (o1 != o2 && o3 != o4)
            {
                return true;
            }

            // Special Cases
            // p1, q1 and p2 are collinear and
            // p2 lies on segment p1q1
            if (o1 == 0 && OnSegment(currentVertex, checkingPoint, nextVertex))
            {
                return true;
            }

            // p1, q1 and p2 are collinear and
            // q2 lies on segment p1q1
            if (o2 == 0 && OnSegment(currentVertex, extreme, nextVertex))
            {
                return true;
            }

            // p2, q2 and p1 are collinear and
            // p1 lies on segment p2q2
            if (o3 == 0 && OnSegment(checkingPoint, currentVertex, extreme))
            {
                return true;
            }

            // p2, q2 and q1 are collinear and
            // q1 lies on segment p2q2
            if (o4 == 0 && OnSegment(checkingPoint, nextVertex, extreme))
            {
                return true;
            }

            // Doesn't fall in any of the above cases
            return false;
        }
    }
}
