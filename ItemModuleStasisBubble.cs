using BS;

namespace StasisRifle
{
    // This create an item module that can be referenced in the item JSON
    public class ItemModuleStasisBubble : ItemModule
    {

        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<ItemStasisBubble>();
        }
    }
}
