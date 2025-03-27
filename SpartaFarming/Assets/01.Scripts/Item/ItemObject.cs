public class ItemObject
{
    public ItemInfo ItemInfo { get; }

    public float currentDurability;

    public ItemObject(ItemInfo itemInfo)
    {
        ItemInfo = itemInfo;
        currentDurability = ItemInfo.durability;
    }
}
