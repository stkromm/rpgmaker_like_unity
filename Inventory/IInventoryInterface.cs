public interface IInventoryInterface
{
    bool ContainsItem(int itemId);

    bool AddAmountofItem(int itemId, int amount);

    bool AddAmountofMoney(int money);

    int GetItemCount(int itemId);
}