public class Party : MyMonoBehaviour
{
    public Character[] GroupParty = new Character[1];
    public int ActiveChar;

    public int GroupLength()
    {
        return GroupParty != null ? GroupParty.Length : 0;
    }

    public void SetParty(Character[] newParty)
    {
        GroupParty = newParty;
    }

    public Character GetCharacterInParty(int index)
    {
        if (index >= 0 && index < GroupParty.Length)
        {
            return GroupParty[index];
        }
        return null;
    }

    public void ChangeActiveCharacter()
    {
        do
        {
            ActiveChar = ActiveChar < GroupParty.Length - 1 ? ActiveChar + 1 : 0;
        } while (!GroupParty[ActiveChar].InParty && GroupParty != null);
    }
}