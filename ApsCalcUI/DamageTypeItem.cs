namespace ApsCalcUI
{
    class DamageTypeItem
    {
        public DamageTypeItem(DamageType iD, string text)
        {
            ID = iD;
            Text = text;
        }

        public DamageType ID { get; }
        public string Text { get; }
    }
}
