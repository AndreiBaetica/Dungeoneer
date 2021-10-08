namespace Runemark.DarkFantasyKit.Tools
{
    using System.Collections.Generic;
    using UnityEngine;
    using Runemark.Common;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    /// <summary>
    /// Created by Adam Galla. 09.01.2019
    /// The script is freely downloaded from https://runemarkstudio.com.
    /// </summary>
    [HelpURL("https://runemarkstudio.com")]
    public class BookClusterGenerator : MonoBehaviour 
    {   
        [Header("General Settings")]    
        public Type type;
        public enum Type { Pile, Cluster }
        public List<GameObject> Books;

        [Header("Pile Settings")]
        public int BookCount;
        [Range(0,1)]public float RotationRandom = 0;

        [Header("Cluster Settings")]
        public float ClusterWidth = 5f;
        [Range(0, 1)] public float SlantedRandom = 0;
        [Range(0, 90)] public float SlantedMaxAngle = 45;
        [Range(0, 1)] public float ZPosRandom = 0;
        public bool ShowGizmo = true;

        [Header("Book Parameters")]
        [SerializeField] float _bookThickness = 0.04f;
        [SerializeField] float _bookHeight = 0.32f;
        [SerializeField] float _bookWidth = 0.22f;

        [HideInInspector][SerializeField] List<GameObject> _instantiatedBooks = new List<GameObject>();
        int _bookCount;
    
        public void Generate()
        {
            Clear();

            if (type == Type.Pile) InitPile();
            if (type == Type.Cluster) InitCluster();

            for (int i = 1; i <= _bookCount; i++)
            {
                var book = Instantiate<GameObject>(Books[Random.Range(0, Books.Count - 1)]);
                book.transform.SetParent(transform);
                book.transform.name = book.transform.name.Replace("(Clone)", "_"+i+"");
                switch (type)
                {
                    case Type.Pile: GeneratePile(i, book); break;
                    case Type.Cluster:
                        if (!GenerateClusterBook(i, book))
                        {
                            DestroyImmediate(book);
                            return;
                        }
                        break;
                }           
            }
        }

        #region Pile Generator
        void InitPile()
        {
            _bookCount = BookCount;
        }
        void GeneratePile(int cnt, GameObject book)
        {
            
            book.transform.localPosition = new Vector3(0, cnt * _bookThickness, 0);

            float angle = Random.Range(0, 360 * RotationRandom);
            book.transform.localEulerAngles = new Vector3(0, angle, 0);
            _instantiatedBooks.Add(book);
        }
        #endregion

        #region Cluster Generator
        int _lastSlantedIndex;
        float _lastSlantedAngle;
        float _lastSlantedPlace;


        int _slantedCount = 0;
        float _clusterWidth = 0;
        float _currentPosX = 0;
        

        void InitCluster()
        {
            _bookCount = Mathf.FloorToInt(ClusterWidth / _bookThickness);

            _slantedCount = 0;
            _clusterWidth = 0;

            _lastSlantedIndex = -1;
            _lastSlantedAngle = 0;        
            _lastSlantedPlace = 0;
            
            _currentPosX = ClusterWidth/2;
        }

        bool GenerateClusterBook(int cnt, GameObject book)
        {
            if (_clusterWidth + _bookThickness > ClusterWidth) 
                return false; 

            Vector3 pos = Vector3.zero;
            float angle = 90;
            float width = _bookThickness;

            // Precalculate the horizontal position of the current book.
            pos.x = _currentPosX - (cnt > 0 ? _bookThickness : 0);
        
            // Handle Slanted Books
            if (SlantedMaxAngle > 0 && cnt > 0 && _lastSlantedIndex < cnt - 1 && Random.Range(0.01f, 1) <= SlantedRandom)
            {
                float maxAngle = SlantedMaxAngle;
                
                // Cap the angle based on the remaining place for the cluster
                float remainingPlace = (ClusterWidth - _clusterWidth) / _bookHeight;
                if (remainingPlace <= 1)
                    maxAngle = Mathf.Min(SlantedMaxAngle, Mathf.Acos(remainingPlace) * Mathf.Rad2Deg);

            /* if (_lastSlantedIndex == cnt - 1)
                    maxAngle = Mathf.Min(_lastSlantedAngle, maxAngle);*/

                // Generate the Angle
                angle -= Random.Range(0,  maxAngle);

                // Calculate the delta position of the book to line up on ground
                Vector2 delta = CalculateTriangleSides(angle, _bookHeight / 2);
                delta.y = _bookHeight / 2 - delta.y;

                // Calculate the delta position based on the thickness of the book.
                Vector2 thicknessDelta = CalculateTriangleSides(90 - angle, _bookThickness/2);

                // Set the placing width of the book
                width = (delta.x + thicknessDelta.x) * 2;
                _lastSlantedPlace = width;

                // If the previous book was slanted too.
                /*if (_lastSlantedIndex == cnt - 1)
                {
                    Debug.Log(cnt + " and " + (cnt - 1) + " are slanted");
                    float desiredDistance = _bookHeight * Mathf.Sin((angle - _lastSlantedAngle)* Mathf.Deg2Rad) / Mathf.Sin((180 - angle) * Mathf.Deg2Rad);

                    Vector2 lastThicknessDelta = CalculateTriangleSides(_lastSlantedAngle, _bookThickness / 2);
                    Vector2 currentThicknessDelta = CalculateTriangleSides(angle, _bookThickness / 2);
                    desiredDistance += lastThicknessDelta.x + currentThicknessDelta.x;

                    if (desiredDistance > 0)
                    {
                        float offset = _lastSlantedPlace - desiredDistance;
                        delta.x -= offset;
                        Debug.Log(cnt + " - Desired Distance: " + desiredDistance + ", Offset: " + offset + " DeltaX: " + delta.x);
                        width = offset;
                    }
                }*/

                // Set the position offset
                pos.x -= delta.x;
                pos.y = -delta.y + thicknessDelta.y;          

                // Update variables
                _lastSlantedIndex = cnt;
                _lastSlantedAngle = angle;                       

                _slantedCount++;
                _currentPosX = pos.x - delta.x;
            }
            else
            {
                _currentPosX = pos.x;
            }

            // Z Position Random 
            if (ZPosRandom > 0)
            {
                float maxOffsetZ = _bookWidth * .5f * ZPosRandom;
                pos.z = Random.Range(0, maxOffsetZ);
            }

        
            _clusterWidth += width;
            if (_clusterWidth > ClusterWidth) return false;

            book.transform.localPosition = pos;
            book.transform.localEulerAngles = new Vector3(angle, 90, 180);       
            _instantiatedBooks.Add(book);

            return true;
        }


        /// <summary>
        /// Calculates the lengths of the triangle sides based on the hypotenuse and the angle.
        /// X = adjacent, Y = opposite side.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="hypotenuse"></param>
        /// <returns></returns>
        Vector2 CalculateTriangleSides(float angle, float hypotenuse)
        {
            Vector2 offset = Vector2.zero;

            // Calculate the delta position based on the thickness of the book.
            offset.x = Mathf.Cos(angle * Mathf.Deg2Rad) * hypotenuse;
            offset.y = Mathf.Sin(angle * Mathf.Deg2Rad) * hypotenuse;
            
            return offset;
        }



        #endregion

        void OnDrawGizmosSelected()
        {
            if (type == Type.Cluster && ShowGizmo)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

                Color c = Color.cyan;
                c.a = .5f;
                Gizmos.color = c;
                Gizmos.DrawCube(Vector3.zero, new Vector3(ClusterWidth, _bookHeight, _bookWidth));
            }
        }



        public void Clear()
        {
            foreach (var b in _instantiatedBooks)
                DestroyImmediate(b);
            _instantiatedBooks.Clear();
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(BookClusterGenerator))]
    public class BookPileGeneratorEditor : CustomInspectorBase
    {
        protected override string Title => "Book Cluster Generator"; 

        BookClusterGenerator myTarget;

        protected override void OnInit()
        {
            BookClusterGenerator myTarget = (BookClusterGenerator)target;

            AddProperty("type");
            AddProperty("Books");

            AddProperty("BookCount");
            AddProperty("RotationRandom");

            AddProperty("ClusterWidth");
            AddProperty("SlantedRandom");
            AddProperty("SlantedMaxAngle");
            AddProperty("ZPosRandom");

            AddProperty("ShowGizmo");

            AddProperty("_bookThickness");
            AddProperty("_bookHeight");
            AddProperty("_bookWidth");

            AddSpace(10);

            AddCustomField("Buttons", () => 
            {
                if (GUILayout.Button("Generate"))
                {
                    myTarget.Generate();
                    EditorUtility.SetDirty(myTarget);
                }
                if (GUILayout.Button("Clear"))
                {
                    myTarget.Clear();
                    EditorUtility.SetDirty(myTarget);
                }
            }, 0);
        }
    }
    #endif
}
