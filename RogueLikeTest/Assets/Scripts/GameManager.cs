using Menus;

namespace DefaultNamespace
{
    public static class GameManager
    {
        public static bool InMenu;

        private static int m_money;
        public static int Money
        {
            set
            {
                m_money = value;
                MenuManager.instance.UpdateMoney(Money);
            }
            get => m_money;
        }
    }
}