using Philip.Utilities.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Philip.Utilities
{
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