﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XEntity.InventoryItemSystem
{
    //This script is attached to the UI representation of the item container. An example item container UI prefab is provided with the asset.
    public class ItemContainer : MonoBehaviour
    {
        [Header("Do NOT assign interactor on external containers.")]
        //The carrier is the interactor this ItemContainer is assigned to. For a player it is the general player inventory.
         public Interactor containerInteractor;
        [SerializeField]
        protected string containerTitle;
        [SerializeField]
        protected KeyCode UIToggleKey = KeyCode.I;
        [SerializeField]
        private SlotOptions[] customOptionsMenuConfig;
        //If this is true, when items are removed from this container, they will be dropped in front of the container.
        [Tooltip("If true when items are removed the corresponding prefab will be instantiated in the scene near the carrier")]
        public bool dropItemGameObjects = true;

        public static System.Action<bool> OnInventoryToggle;

        public GameObject zoomPanel;          // Panel z dużym obrazkiem
        public UnityEngine.UI.Image zoomImage;

        //The array of slots this container holds. The slots are assigned through code based on the number of children the slot holder Transform contains.
        protected ItemSlot[] slots;

        //The UI of the container, a containerUI template prefab is provided with this asset. All container UI mus tbe set up exactly in that same way.
        //To modify the number of slots, modifiy the number of children the slot holder inside the containerUI has.
        protected Transform mainContainerUI;

        //The options UI that pops up when a slot is clicked. A prefab template for this UI is provided with this asset.
        protected GameObject itemInfoPanel;
        protected GameObject slotOptionsMenu;
        protected bool isContainerUIOpen = false;
        protected bool isUIInitialized;
        protected Transform containerPanel;

        private List<SlotOptionButtonInfo> slotOptionButtonInfoList;

        [Space]

        [SerializeField] private bool interactWithRaycast;
        [SerializeField] private LayerMask interactiveLayer;

        public ContainerType containerType = ContainerType.Inventory;

        protected virtual void OnEnable()
        {
            ItemSlotUIEvents.OnSlotDrag += CloseSlotOptionsMenu;
            ContainerPanelDragger.OnContainerPanelDrag += CloseSlotOptionsMenu;
        }

        protected virtual void OnDisable()
        {
            ItemSlotUIEvents.OnSlotDrag -= CloseSlotOptionsMenu;
            ContainerPanelDragger.OnContainerPanelDrag -= CloseSlotOptionsMenu;
        }

        private static ItemContainer instance;


        protected virtual void Awake()
        {
            if (zoomPanel == null)
                zoomPanel = transform.Find("ZoomPanel")?.gameObject;

            if (zoomImage == null && zoomPanel != null)
                zoomImage = zoomPanel.GetComponentInChildren<Image>();

            isUIInitialized = false;

            //The container is initilized on awake.
            InitializeContainer();

            //Initialize all the slots in this container.
            foreach (ItemSlot slot in slots)
            {
                slot.Initialize();
            }

            AddDefaultItemsToChest();
        }

        private void AddDefaultItemsToChest()
        {
            if (containerType == ContainerType.Chest_1)
            {
                Item Item1 = ItemManager.Instance.GetItemByName("Key");
                Item Item2 = ItemManager.Instance.GetItemByName("Sztabka");
                if (Item1 != null && Item2 != null)
                {
                    slots[0].SetData(Item1, 1);
                    slots[1].SetData(Item2, 3);
                }
                else
                {
                    Debug.LogWarning("Nie ma przedmiotów.");
                }
            }

            else if (containerType == ContainerType.Chest_2)
            {
                Item Item1 = ItemManager.Instance.GetItemByName("Stone");
                Item Item2 = ItemManager.Instance.GetItemByName("Serce");
                if (Item1 != null && Item2 != null)
                {
                    slots[0].SetData(Item1, 1);
                    slots[1].SetData(Item2, 2);
                }
                else
                {
                    Debug.LogWarning("Nie ma przedmiotów.");
                }

            }

            else if (containerType == ContainerType.Chest_3)
            {
                Item Item1 = ItemManager.Instance.GetItemByName("Bomba");
                Item Item2 = ItemManager.Instance.GetItemByName("Serce");
                if (Item1 != null && Item2 != null)
                {
                    slots[0].SetData(Item1, 1);
                    slots[1].SetData(Item2, 3);
                }
                else
                {
                    Debug.LogWarning("Nie ma przedmiotów.");
                }

            }
        }

        protected virtual void Update()
        {
            if (isUIInitialized == false) return;

            CheckForUIToggleInput();

            if (containerInteractor == null)
            {
                containerInteractor = FindAnyObjectByType<Interactor>();
            }
        }

        //All the container variables are assigned here based.
        protected virtual void InitializeContainer()
        {
            IntialzieMainUI(transform);

            if (containerInteractor == null)
            {
                containerInteractor = FindAnyObjectByType<Interactor>();
            }

            switch (containerType)
            {
                case ContainerType.Inventory:
                    CreateSlotOptionsMenu(InteractionSettings.Current.internalSlotOptions, containerInteractor);
                    break;
                case ContainerType.Chest_1:
                    CreateSlotOptionsMenu(InteractionSettings.Current.chest_1_SlotOptions, containerInteractor);
                    break;
                case ContainerType.Chest_2:
                    CreateSlotOptionsMenu(InteractionSettings.Current.chest_2_SlotOptions, containerInteractor);
                    break;
                case ContainerType.Chest_3:
                    CreateSlotOptionsMenu(InteractionSettings.Current.chest_3_SlotOptions, containerInteractor);
                    break;
            }

            isUIInitialized = true;
        }

        protected void IntialzieMainUI(Transform containerPanel)
        {
            Debug.Log("InitializeMainUI() wywołane dla: " + containerPanel.name);
            this.containerPanel = containerPanel;
            mainContainerUI = this.containerPanel.Find("Main UI");
            mainContainerUI.Find("Title").GetComponentInChildren<Text>().text = containerTitle;
            slotOptionsMenu = this.containerPanel.Find("Slot Options").gameObject;

            Button containerCloseButton = mainContainerUI.transform.Find("Close Button").GetComponent<Button>();
            containerCloseButton.onClick.AddListener(() => ToggleUI());

            Transform slotHolder = mainContainerUI.Find("Slot Holder");
            slots = new ItemSlot[slotHolder.childCount];
            for (int i = 0; i < slots.Length; i++)
            {
                ItemSlot slot = slotHolder.GetChild(i).GetComponent<ItemSlot>();
                slots[i] = slot;
                Button slotButton = slot.GetComponent<Button>();
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(delegate { OnSlotClicked(slot, containerInteractor); });
            }
            mainContainerUI.gameObject.SetActive(false);
        }

        protected virtual void CreateSlotOptionsMenu(SlotOptions[] config, Interactor interactor)
        {
            //If valid custom configuration eixsts, use that instead of the default one.
            if (customOptionsMenuConfig != null && customOptionsMenuConfig.Length > 0)
            {
                config = customOptionsMenuConfig;
            }

            itemInfoPanel = slotOptionsMenu.transform.Find("Info Panel").gameObject;

            GameObject buttonPrefab = InteractionSettings.Current.optionsMenuButtonPrefab;
            slotOptionButtonInfoList = new List<SlotOptionButtonInfo>();

            foreach (Transform child in slotOptionsMenu.transform)
            {
                if (child.GetComponent<Button>()) Destroy(child.gameObject);
            }

            if (config != null)
            {
                foreach (SlotOptions option in config)
                {
                    Button button = Instantiate(buttonPrefab, slotOptionsMenu.transform).GetComponent<Button>();
                    string buttonTitle = option.ToString();
                    System.Action<ItemSlot, Interactor> onButtonClicked = null;

                    //Add custom slot options here ###################################################################################################
                    switch (option)
                    {
                        case SlotOptions.View:
                            buttonTitle = "Powiększ";
                            onButtonClicked = OnViewItemClicked;
                            break;
                        case SlotOptions.ItemInfo:
                            buttonTitle = "Info.";
                            onButtonClicked = OnItemInfoClicked;
                            break;
                        case SlotOptions.Remove:
                            buttonTitle = "Wyrzuć";
                            onButtonClicked = OnRemoveItemClicked;
                            break;
                        case SlotOptions.BulkRemove:
                            buttonTitle = "Wszystko wyrzuć";
                            onButtonClicked = OnBulkRemoveItemClicked;
                            break;
                        case SlotOptions.TransferToInventory:
                            buttonTitle = "Przenieś";
                            onButtonClicked = OnTransferToInventoryClicked;
                            break;
                    }

                    button.GetComponentInChildren<Text>().text = buttonTitle;
                    SlotOptionButtonInfo buttonInfo = new SlotOptionButtonInfo(button, onButtonClicked, OnSlotButtonEventFinished);
                    slotOptionButtonInfoList.Add(buttonInfo);
                }
            }

            CloseSlotOptionsMenu();
        }

        private void OnViewItemClicked(ItemSlot slot, Interactor interactor)
        {
            if (slot == null || slot.slotItem == null || slot.slotItem.icon == null)
            {
                Debug.LogWarning("Brakuje przedmiotu lub ikony.");
                return;
            }

            zoomImage.sprite = slot.slotItem.icon;
            zoomImage.preserveAspect = true;

            zoomPanel.SetActive(true);
        }

        public void CloseZoomPanel()
        {
            zoomPanel.SetActive(false);
        }


        private void OnTransferToInventoryClicked(ItemSlot slot, Interactor interactor)
        {
            Utils.TransferItemQuantity(slot, interactor.inventory, slot.itemCount);
        }

        //This method is called when a slot is clicked.
        //The listeners on the slot option buttons are cleared and re-assigned based on the selected slot.
        protected void OnSlotClicked(ItemSlot slot, Interactor interactor)
        {
            if (slot.IsEmpty) return;

            if (!slotOptionsMenu.activeSelf)
            {
                foreach (SlotOptionButtonInfo buttonInfo in slotOptionButtonInfoList)
                {
                    buttonInfo.UpdateInfo(slot, interactor);
                }

                OpenSlotOptionsMenu();
            }
            else
            {
                CloseSlotOptionsMenu();
            }
        }

        private void OpenSlotOptionsMenu()
        {
            slotOptionsMenu.SetActive(false);
            slotOptionsMenu.transform.position = Input.mousePosition;
            StartCoroutine(Utils.TweenScaleIn(slotOptionsMenu, 10, Vector3.one));
        }

        private void CloseSlotOptionsMenu()
        {
            if (slotOptionsMenu != null)
                slotOptionsMenu.SetActive(false);
            else
                Debug.LogWarning("slotOptionsMenu is NULL");

            if (itemInfoPanel != null)
                itemInfoPanel.SetActive(false);
            else
                Debug.LogWarning("itemInfoPanel is NULL");
        }

        private void OnRemoveItemClicked(ItemSlot slot, Interactor interactor)
        {
            if (dropItemGameObjects) slot.RemoveAndDrop(1, containerInteractor.ItemDropPosition);
            else slot.Remove(1);
        }

        private void OnBulkRemoveItemClicked(ItemSlot slot, Interactor interactor)
        {
            if (dropItemGameObjects) slot.RemoveAndDrop(slot.itemCount, interactor.ItemDropPosition);
            else slot.Remove(slot.itemCount);
        }

        private void OnUseItemClicked(ItemSlot slot, Interactor interactor)
        {
            ItemManager.Instance.UseItem(slot);
        }

        private void OnItemInfoClicked(ItemSlot slot, Interactor interactor)
        {
            itemInfoPanel.GetComponentInChildren<Text>().text = slot.slotItem.itemInformation;
            itemInfoPanel.SetActive(!itemInfoPanel.activeSelf);
        }

        private void OnSlotButtonEventFinished(ItemSlot slot)
        {
            if (slot.IsEmpty)
            {
                CloseSlotOptionsMenu();
            }
        }

        //Checks for user inputs and updates the toggle state of the UI accordingly.
        
        

        protected void CheckForUIToggleInput()
        {
            if (Input.GetKeyDown(UIToggleKey))
            {
                if (interactWithRaycast)
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, InteractionSettings.Current.interactionRange, interactiveLayer))
                    {
                        ItemContainer interactable = hit.transform.GetComponentInChildren<ItemContainer>();

                        if (interactable != null)
                        {
                            ToggleUI();
                        }
                    }
                }
                else
                {
                    ToggleUI();
                }
            }
        }

        //Returns true if it's able to add the item to the container.
        public bool AddItem(Item item)
        {
            for (int i = 0; i < slots.Length; i++) if (slots[i].Add(item)) return true;
            return false;
        }

        //Returns true if the container contains the passed in item.
        public bool ContainsItem(Item item)
        {
            for (int i = 0; i < slots.Length; i++)
                if (slots[i].slotItem == item) return true;
            return false;
        }

        //Returns true if the container contains the passed in amount of item.
        public bool ContainsItemQuantity(Item item, int amount)
        {
            int count = 0;
            foreach (ItemSlot slot in slots)
            {
                if (slot.slotItem == item) count += slot.itemCount;
                if (count >= amount) return true;
            }
            return false;
        }

        //Updates the UI toggle state.
        protected void ToggleUI()
        {
            CloseSlotOptionsMenu();

            bool willOpen;
            if (mainContainerUI.gameObject.activeSelf && isContainerUIOpen)
            {

                isContainerUIOpen = false;
                StartCoroutine(Utils.TweenScaleOut(mainContainerUI.gameObject, 10, false));
                willOpen = false;
            }
            else
            {
                isContainerUIOpen = true;
                StartCoroutine(Utils.TweenScaleIn(mainContainerUI.gameObject, 10, Vector3.one));
                willOpen = true;
            }


            if (willOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            OnInventoryToggle?.Invoke(willOpen);
        }

        //Below is the code for saving/loading/deleting container data using JSON utility.
        #region Saving & Loading Data

        //This method saves the container data on an unique file path that is aquired based on the passed in id.
        //This id should be unique for different saves.
        //If a save already exists with the id, the data will be overwritten.
        public void SaveData(string id)
        {
            //An unique file path is aquired here based on the passed in id. 
            string dataPath = GetIDPath(id);

            if (System.IO.File.Exists(dataPath))
            {
                System.IO.File.Delete(dataPath);
                Debug.Log("Exisiting data with id: " + id + "  is overwritten.");
            }

            try
            {
                Transform slotHolder = mainContainerUI.Find("Slot Holder");
                SlotInfo info = new SlotInfo();
                for (int i = 0; i < slotHolder.childCount; i++)
                {
                    ItemSlot slot = slotHolder.GetChild(i).GetComponent<ItemSlot>();
                    if (!slot.IsEmpty)
                    {
                        info.AddInfo(i, ItemManager.Instance.GetItemIndex(slot.slotItem), slot.itemCount);
                    }
                }
                //Convert slot info object to json string and write it to a local file
                string jsonData = JsonUtility.ToJson(info);
                System.IO.File.WriteAllText(dataPath, jsonData);
                Debug.Log("<color=green>Data succesfully saved! </color>");
            }
            catch
            {
                Debug.LogError("Could not save container data! Make sure you have entered a valid id and all the item scriptable objects are added to the ItemManager item list");
            }
        }

        //Loads container data that was saved with the passed in id.
        //NOTE: A save file must exist first with the id in order for it to be loaded.
        public void LoadData(string id)
        {
            string dataPath = GetIDPath(id);

            if (!System.IO.File.Exists(dataPath))
            {
                Debug.LogWarning("No saved data exists for the provided id: " + id);
                return;
            }

            try
            {
                //Read and parse json string to slot info object and load all data accordingly.
                string jsonData = System.IO.File.ReadAllText(dataPath);
                SlotInfo info = JsonUtility.FromJson<SlotInfo>(jsonData);

                Transform slotHolder = mainContainerUI.Find("Slot Holder");
                for (int i = 0; i < info.slotIndexs.Count; i++)
                {
                    Item item = ItemManager.Instance.GetItemByIndex(info.itemIndexs[i]);
                    slotHolder.GetChild(info.slotIndexs[i]).GetComponent<ItemSlot>().SetData(item, info.itemCounts[i]);
                }
                Debug.Log("<color=green>Data succesfully loaded! </color>");
            }
            catch
            {
                Debug.LogError("Could not load container data! Make sure you have entered a valid id and all the item scriptable objects are added to the ItemManager item list.");
            }
        }

        //Deletes the save with the passed in id, if one exists.
        public void DeleteData(string id)
        {
            string path = GetIDPath(id);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                Debug.Log("Data with id: " + id + " is deleted.");
            }
        }

        //Returns a unique path based on the id.
        protected virtual string GetIDPath(string id)
        {
            return Application.persistentDataPath + $"/{id}.dat";
        }

        //This struct contains the data for the container slots; used for saving/loading the container slot data.
        //NOTE: JsonUtility wasn't serializing nested information properly. So i resorted to using seperate list of indexes for each slot.
        public class SlotInfo
        {
            public List<int> slotIndexs;
            public List<int> itemIndexs;
            public List<int> itemCounts;

            public SlotInfo()
            {
                slotIndexs = new List<int>();
                itemIndexs = new List<int>();
                itemCounts = new List<int>();
            }

            public void AddInfo(int slotInex, int itemIndex, int itemCount)
            {
                slotIndexs.Add(slotInex);
                itemIndexs.Add(itemIndex);
                itemCounts.Add(itemCount);
            }

        }
        #endregion

        private class SlotOptionButtonInfo
        {
            internal Button optionButton;
            internal System.Action<ItemSlot, Interactor> onButtonClicked;
            internal System.Action<ItemSlot> onButtonEventFinished;

            internal SlotOptionButtonInfo(Button optionButton, System.Action<ItemSlot, Interactor> onButtonClicked, System.Action<ItemSlot> onButtonEventFinished)
            {
                this.optionButton = optionButton;
                this.onButtonClicked = onButtonClicked;
                this.onButtonEventFinished = onButtonEventFinished;
            }

            //Upadtes info based on the currently selected slot
            internal void UpdateInfo(ItemSlot slot, Interactor interactor)
            {
                optionButton.onClick.RemoveAllListeners();
                optionButton.onClick.AddListener(
                    delegate
                    {
                        onButtonClicked?.Invoke(slot, interactor);
                        onButtonEventFinished?.Invoke(slot);
                    });
            }
        }
    }
}

public enum ContainerType
{
    Inventory,
    Chest_1,
    Chest_2,
    Chest_3
}