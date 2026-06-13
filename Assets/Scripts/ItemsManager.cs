using System.Collections.Generic;
using UnityEngine;

namespace GardenDefense
{
    public class ItemsManager : MonoBehaviour
    {
        [SerializeField]
        private List<ItemEntry> _itemList = new List<ItemEntry>(); // Для отображения в инспекторе
        
        // Словарь для быстрого доступа из кода (не виден в инспекторе)
        private Dictionary<string, int> _items = new Dictionary<string, int>();
        
        private static ItemsManager _instance;
        
        public static ItemsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Ищем существующий экземпляр в сцене
                    _instance = FindObjectOfType<ItemsManager>();
                    
                    // Если не нашли, создаем новый
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("ItemsManager");
                        _instance = obj.AddComponent<ItemsManager>();
                        DontDestroyOnLoad(obj); // Опционально: не уничтожать при загрузке сцен
                    }
                }
                return _instance;
            }
        }
        
        // Для отображения словаря в инспекторе
        [System.Serializable]
        public struct ItemEntry
        {
            public string itemName;
            public int quantity;
        }
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject); // Опционально
            
            // Конвертируем List в Dictionary для быстрого доступа
            ConvertListToDictionary();
        }
        
        private void ConvertListToDictionary()
        {
            _items.Clear();
            foreach (var entry in _itemList)
            {
                if (!string.IsNullOrEmpty(entry.itemName))
                {
                    _items[entry.itemName] = entry.quantity;
                }
            }
        }
        
        public void AddItem(string itemName, int quantity)
        {
            if (_items.ContainsKey(itemName))
            {
                _items[itemName] += quantity;
            }
            else
            {
                _items[itemName] = quantity;
            }
            
            // Синхронизируем с List (для отображения в инспекторе во время игры)
            SyncItemList();
            HUDManager.Instance.UpdateResource(itemName); // Обновляем HUD
            Debug.Log($"Added {quantity} of {itemName}. Total: {_items[itemName]}");
        }
        
        public bool RemoveItem(string itemName, int quantity)
        {
            if (!_items.ContainsKey(itemName) || _items[itemName] < quantity)
                return false;
                
            _items[itemName] -= quantity;
            
            if (_items[itemName] <= 0)
                _items.Remove(itemName);

            HUDManager.Instance.UpdateResource(itemName); // Обновляем HUD
            SyncItemList();
            Debug.Log($"Removed {quantity} of {itemName}. Total: {_items[itemName]}");
            return true;
        }
        
        public int GetItemCount(string itemName)
        {
            return _items.ContainsKey(itemName) ? _items[itemName] : 0;
        }
        
        public bool HasItem(string itemName, int quantity = 0)
        {
            return GetItemCount(itemName) >= quantity;
        }
        
        private void SyncItemList()
        {
            _itemList.Clear();
            foreach (var kvp in _items)
            {
                _itemList.Add(new ItemEntry { itemName = kvp.Key, quantity = kvp.Value });
            }
        }
    }
}