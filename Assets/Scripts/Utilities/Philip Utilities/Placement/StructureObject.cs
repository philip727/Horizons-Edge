using UnityEngine;

namespace Philip.Building 
{
    public abstract class StructureObject : MonoBehaviour, IBuildable
    {
        [field: SerializeField] public GameObject BuiltObject { private set; get; }

        [field: SerializeField] public StructureObjectSettings StructureObjectSettings { private set; get; }

        public bool IsBuilt { private set; get; } = false;

        protected virtual void Awake()
        {
        
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            SetupStructureNodes();
        }

        protected virtual void Update()
        {

        }

        public void SetupStructureNodes()
        {
            Vector2Int coordinates = Placement<IBuildable>.Instance.GetGrid().GetCoordinate(BuiltObject.transform.position);

            Placement<IBuildable>.Instance.PlaceObjectInNode(this, coordinates);
        }

        public virtual bool HasBuildRequirements()
        {
            return true;
        }

        public virtual void OnBuilt()
        {
            SetIsBuilt(true);
        }

        public void SetIsBuilt(bool value)
        {
            IsBuilt = value;
        }
    }
}

