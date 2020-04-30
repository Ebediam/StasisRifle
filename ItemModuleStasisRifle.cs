using BS;

namespace StasisRifle
{


    // This create an item module that can be referenced in the item JSON
    public class ItemModuleStasisRifle : ItemModule
    {
        public float minDuration = 6.5f;
        public float maxDuration = 30f;

        public float fullCharge = 3f;
        public float minCharge = 1f;

        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<ItemStasisRifle>();
        }
    }
}
