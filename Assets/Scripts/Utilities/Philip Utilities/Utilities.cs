using JetBrains.Annotations;
using Philip.Utilities.Maths;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Philip.Utilities
{
    namespace Maths
    {
        public static class PInt
        {
            public static int Unsigned(this int number)
            {
                if (number >= 0)
                {
                    return number;
                }
                else
                {
                    return (number * -1);
                }
            }
        }

        public static class PFloat
        {
            public static float Unsigned(this float number)
            {
                if (number >= 0f)
                {
                    return number;
                }
                else
                {
                    return (number * -1f);
                }
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

    namespace UI
    {
        public static class PEvent
        {
            public static void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
            {
                EventTrigger trigger = obj.GetComponent<EventTrigger>();
                var eventTrigger = new EventTrigger.Entry();
                eventTrigger.eventID = type;
                eventTrigger.callback.AddListener(action);
                trigger.triggers.Add(eventTrigger);
            }

            public static void RemoveEvent(GameObject obj, EventTriggerType type)
            {
                EventTrigger trigger = obj.GetComponent<EventTrigger>();
                EventTrigger.Entry _eventTrigger = Array.Find(trigger.triggers.ToArray(), eventTrigger => eventTrigger.eventID == type);
                trigger.triggers.Remove(_eventTrigger);
            }
        }


        public static class PWorldDebug
        {
            public static TextMesh CreateWorldText(string text, Transform parent=null, Vector3 localPosition = default, Vector3 localScale = default, int fontSize=40,  Color colour=default, TextAnchor textAnchor= TextAnchor.UpperLeft, TextAlignment textAlignment=TextAlignment.Left, int sortingOrder=0) 
            {
                if (colour == null) colour = Color.white;
                return CreateWorldText(parent, text, localPosition, fontSize, colour, textAnchor, textAlignment, sortingOrder, localScale);
            }

            public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder, Vector3 localScale) 
            {
                GameObject gameObject = new GameObject("text_world", typeof(TextMesh));
                Transform transform = gameObject.transform;
                transform.SetParent(parent, false);
                transform.localPosition = localPosition;
                transform.localScale = localScale;
                TextMesh textMesh = gameObject.GetComponent<TextMesh>();
                textMesh.anchor = textAnchor;
                textMesh.alignment = textAlignment;
                textMesh.text = text;
                textMesh.fontSize = fontSize;
                textMesh.color = color;
                textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
                return textMesh;
            }

            public static TextMesh CreateWorldTextWithWorldPosition(string text, Transform parent = null, Vector3 worldPosition = default, Vector3 localScale = default, int fontSize = 40, Color colour = default, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 0)
            {
                if (colour == null) colour = Color.white;
                return CreateWorldTextWithWorldPosition(parent, text, worldPosition, fontSize, colour, textAnchor, textAlignment, sortingOrder, localScale);
            }

            public static TextMesh CreateWorldTextWithWorldPosition(Transform parent, string text, Vector3 worldPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder, Vector3 localScale)
            {
                GameObject gameObject = new GameObject("text_world", typeof(TextMesh));
                Transform transform = gameObject.transform;
                transform.SetParent(parent, false);
                transform.position = worldPosition;
                transform.localScale = localScale;
                TextMesh textMesh = gameObject.GetComponent<TextMesh>();
                textMesh.anchor = textAnchor;
                textMesh.alignment = textAlignment;
                textMesh.text = text;
                textMesh.fontSize = fontSize;
                textMesh.color = color;
                textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
                return textMesh;
            }
        }
    }

    namespace GameObjects
    {

        public static class PSprite
        {
            // Take in a sprite renderer, and fade the colour from 1 to 0 in given time
            public static IEnumerator FadeOutSprite(this SpriteRenderer sprite, float time, float minimumAlpha = 0, bool useFixedDeltaTime = false)
            {
                float i = 0.0f;
                while (i < 1.0f)
                {
                    i += Time.deltaTime / time;
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Lerp(1, minimumAlpha, i));
                    yield return useFixedDeltaTime ? new WaitForFixedUpdate() : new WaitForEndOfFrame();
                }
            }
        }

        public static class PUnityComponent
        {
            public static bool TagContains(this Component component, string substring)
            {
                return component.tag.Contains(substring);
            }
        }

        public static class PTransformExtensions
        {
            public static Quaternion SetRotationQuaternion(this Transform _transform, Vector3 targetPosition, float speed)
            {
                Vector2 direction = _transform.position.GetDirectionTo(targetPosition);
                return Quaternion.Slerp(_transform.rotation, direction.AsQuaternion(), speed * Time.deltaTime);
            }
        }

        public static class PGameObjectExtensions
        {
            public static GameObject GetChildByName(this GameObject gameObject, string nameOfGameObject)
            {
                foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
                {
                    if (child.gameObject.name == nameOfGameObject)
                        return child.gameObject;
                }

                return null;
            }

            public static Transform[] GetAllChildren(this GameObject gameObject)
            {
                List<Transform> transforms = new List<Transform>();
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    transforms.Add(gameObject.transform.GetChild(i));
                }
                return transforms.ToArray();
            }
        }
    }

    namespace Extras
    {
        public static class PMouse
        {
            public static Vector3 GetScreenMouseWorldPosition(this UnityEngine.Camera camera)
            {
                return camera.ScreenToWorldPoint(Input.mousePosition);
            }

            public static Vector3 GetViewportMouseWorldPosition(this UnityEngine.Camera camera)
            {
                return camera.ViewportToWorldPoint(Input.mousePosition);
            }

        }

    }
}

[System.Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}]", x, y, z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}