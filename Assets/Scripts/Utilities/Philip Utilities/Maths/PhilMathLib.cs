using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Philip.Utilities
{
    namespace Math
    {
        public static class PMath
        {
            public static float EuclidianDistance(float x1, float y1, float x2, float y2)
            {
                float xDistance = Mathf.Pow(x2 - x1, 2f);
                float yDistance = Mathf.Pow(y2 - y1, 2f);
                return Mathf.Sqrt(Mathf.Abs(xDistance + yDistance));
            }
        }

        public static class PQuaternion
        {
            public static Quaternion DirectionToQuaternion(Vector2 direction)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                return rotation;
            }
        }

        public static class PQuaternionExtensions
        {
            public static Quaternion AsQuaternion(this Vector2 direction)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                return rotation;
            }
        }

        public static class PExponent
        {
            public static int DEScaling(float originalValue, float affector = 1, float exponent = 1.55f)
            {
                return Mathf.RoundToInt(originalValue * (1 + (Mathf.Pow(affector - 1, exponent) * 0.15f)));
            }
        }

        public static class PVector
        {
            public static float GetDistanceBetween(GameObject from, GameObject to)
            {
                return new Vector2(to.transform.position.x - from.transform.position.x, to.transform.position.y - from.transform.position.y).magnitude;
            }

            public static bool GetDistanceBetween(GameObject from, GameObject to, float epsilon, bool equalTo = false)
            {
                Vector2 distanceVector = new Vector2(to.transform.position.x - from.transform.position.x, to.transform.position.y - from.transform.position.y);
                return equalTo == false ? distanceVector.magnitude < epsilon : distanceVector.magnitude <= epsilon;
            }
            public static float GetDistanceBetween(Vector2 from, Vector2 to)
            {
                return new Vector2(to.x - from.x, to.y - from.y).magnitude;
            }
            public static bool GetDistanceBetween(Vector2 from, Vector2 to, float epsilon, bool equalTo = false)
            {
                Vector2 distanceVector = new Vector2(to.x - from.x, to.y - from.y);
                return equalTo == false ? distanceVector.magnitude < epsilon : distanceVector.magnitude <= epsilon;
            }

            /// <summary>
            /// Rotate Vector2 by the angle given.
            /// </summary>
            /// <param name="vector">Vector direction</param>
            /// <param name="angle">Angle</param>
            /// <returns>The new direction normalized.</returns>
            public static Vector2 RotateVectorGetDirection(Vector2 vector, float angle, bool useRadians = false)
            {
                if (!useRadians) angle *= Mathf.Deg2Rad;
                var ca = Mathf.Cos(angle);
                var sa = Mathf.Sin(angle);
                var rx = vector.x * ca - vector.y * sa;

                return new Vector2((float)rx, (float)(vector.x * sa + vector.y * ca)).normalized;
            }

            /// <summary>
            /// Get a random vector that is x away from the origin.
            /// </summary>
            /// <param name="origin">Vector origin</param>
            /// <param name="distance">Distance from origin</param>
            /// <returns>A new vector x away from origin.</returns>
            public static Vector2 RandomVectorWithinDistance(Vector2 origin, float distance)
            {
                return new Vector2(Random.Range(origin.x - distance, origin.x + distance), Random.Range(origin.y - distance, origin.y + distance));
            }
        }

        public static class PVectorExtensions
        {
            public static Vector2 GetDirectionTo(this Vector2 origin, Vector2 to)
            {
                return new Vector2(to.x - origin.x, to.y - origin.y).normalized;
            }

            /// <summary>
            /// Rotate Vector2 by the angle given.
            /// </summary>
            /// <param name="vector">Vector direction</param>
            /// <param name="angle">Angle</param>
            /// <returns>The new direction normalized.</returns>
            public static Vector2 RotateVectorGetDirection(this Vector2 vector, float angle, bool useRadians = false)
            {
                if (!useRadians) angle *= Mathf.Deg2Rad;
                var ca = Mathf.Cos(angle);
                var sa = Mathf.Sin(angle);
                var rx = vector.x * ca - vector.y * sa;

                return new Vector2((float)rx, (float)(vector.x * sa + vector.y * ca)).normalized;
            }

            /// <summary>
            /// Get a vector that is x away from target with the direction towards the origin.
            /// </summary>
            /// <param name="origin">Vector origin.</param>
            /// <param name="target">Vector target.</param>
            /// <param name="distance">Distance from origin - If using distanceAsPercentage then it uses the original distance and gets that percentage.</param>
            /// <param name="distanceAsPercentage">Use distance as percentage between origin and target.</param>
            /// <returns>A new vector x away from the target in the original direction.</returns>
            public static Vector2 MirrorDirection(this Vector2 origin, Vector2 target, float distance, bool distanceAsPercentage = false)
            {
                Vector2 dir = origin.GetDirectionTo(target);

                return target + (dir * (distanceAsPercentage ? -(dir.magnitude * (distance / 100)) : -distance));
            }

            /// <summary>
            /// Get a vector that is x away from target with the direction towards the origin.
            /// </summary>
            /// <param name="origin">Vector origin.</param>
            /// <param name="target">Vector target.</param>
            /// <param name="distance">Distance from origin - If using distanceAsPercentage then it uses the original distance and gets that percentage.</param>
            /// <param name="distanceAsPercentage">Use distance as percentage between origin and target.</param>
            /// <returns>A new vector x away from the target in the original direction.</returns>
            public static Vector2 MirrorDirection(this Vector3 origin, Vector2 target, float distance, bool distanceAsPercentage = false)
            {
                Vector2 _origin = (Vector2)origin;
                Vector2 dir = _origin.GetDirectionTo(target);

                return target + (dir * (distanceAsPercentage ? -(dir.magnitude * (distance / 100)) : -distance));
            }

            /// <summary>
            /// Get a vector that is x away from target with the direction towards the origin with an offset.
            /// </summary>
            /// <param name="origin">Vector origin.</param>
            /// <param name="target">Vector target.</param>
            /// <param name="distance">Distance from origin - If using distanceAsPercentage then it uses the original distance and gets that percentage.</param>
            /// <param name="angle">Offset from the original direction.</param>
            /// <param name="distanceAsPercentage">Use distance as percentage between origin and target.</param>
            /// <returns>A new vector x away from the target in the original direction with an offset.</returns>
            public static Vector2 MirrorDirectionWithOffset(this Vector3 origin, Vector2 target, float distance, float angle, bool distanceAsPercentage = false)
            {
                Vector2 _origin = (Vector2)origin;
                Vector2 dir = _origin.GetDirectionTo(target).RotateVectorGetDirection(angle);

                return target + (dir * (distanceAsPercentage ? -(dir.magnitude * (distance / 100)) : -distance));
            }

            /// <summary>
            /// Get a vector that is x away from target with the direction towards the origin with an offset.
            /// </summary>
            /// <param name="origin">Vector origin.</param>
            /// <param name="target">Vector target.</param>
            /// <param name="distance">Distance from origin - If using distanceAsPercentage then it uses the original distance and gets that percentage.</param>
            /// <param name="angle">Offset from the original direction.</param>
            /// <param name="distanceAsPercentage">Use distance as percentage between origin and target.</param>
            /// <returns>A new vector x away from the target in the original direction with an offset.</returns>
            public static Vector2 MirrorDirectionWithOffset(this Vector2 origin, Vector2 target, float distance, float angle, bool distanceAsPercentage = false)
            {
                Vector2 dir = origin.GetDirectionTo(target).RotateVectorGetDirection(angle);

                return target + (dir * (distanceAsPercentage ? -(dir.magnitude * (distance / 100)) : -distance));
            }
        }

        public static class PVector3
        {
            public static Vector3 Oscillate(float oscillateHeight, float oscillateSpeed, float oscillateRange, float oscillateOffset)
            {
                return (Vector3.up * (oscillateHeight + Mathf.Cos(Time.time * oscillateSpeed) * oscillateRange)) + new Vector3(0f, oscillateOffset, 0f);
            }
        }

        public static class PVector3Extensions
        {
            public static Vector2 GetDirectionTo(this Vector3 origin, Vector2 to)
            {
                return new Vector2(to.x - origin.x, to.y - origin.y).normalized;
            }
        }
    }
}
