using UnityEngine;
using UnityEngine.UIElements;

namespace GardenDefense
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private VisualElement root;

        private Label woodAmountLabel;
        private Label oreAmountLabel;
        private Label essenceAmountLabel;

        private static HUDManager _instance;

        public static HUDManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<HUDManager>();

                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("HUDManager");
                        _instance = obj.AddComponent<HUDManager>();
                        _instance.uiDocument = obj.AddComponent<UIDocument>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            if (uiDocument == null)
            {
                uiDocument = GetComponent<UIDocument>();
            }

            if (uiDocument == null)
            {
                Debug.LogError("HUDManager: UIDocument component not found!");
                return;
            }

            root = uiDocument.rootVisualElement;

            woodAmountLabel = root.Q<Label>("ResourceWoodAmount");
            oreAmountLabel = root.Q<Label>("ResourceOreAmount");
            essenceAmountLabel = root.Q<Label>("ResourceEssenceAmount");
        }

        private void Start()
        {
            UpdateAllResources();
        }
        public void UpdateResource(string resourceName)
        {
            switch (resourceName)
            {
                case "Wood":
                    if (woodAmountLabel != null)
                        woodAmountLabel.text = ItemsManager.Instance.GetItemCount("Wood").ToString();
                    break;
                case "Ore":
                    if (oreAmountLabel != null)
                        oreAmountLabel.text = ItemsManager.Instance.GetItemCount("Stone").ToString();
                    break;
                case "Essence":
                    if (essenceAmountLabel != null)
                        essenceAmountLabel.text = ItemsManager.Instance.GetItemCount("Essence").ToString();
                    break;
                default:
                    Debug.LogError("No label found for resource: " + resourceName);
                    break;
                    // Добавьте другие ресурсы по необходимости
            }
        }
        public void UpdateAllResources()
        {
            UpdateResource("Wood");
            UpdateResource("Ore");
            UpdateResource("Essence");
        }
    }
}