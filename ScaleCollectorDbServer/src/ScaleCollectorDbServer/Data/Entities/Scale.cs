namespace ScaleCollectorDbServer.Data.Entities
{
    public class Scale : TenantEntity
    {
        public int RatioFrom { get; set; }

        public int RatioTo { get; set; }

        public string RatioText { get; set; }

        public virtual ICollection<ModelKit> Models { get; set; }

        public static implicit operator Scale(string text)
        {
            var parts = text.Split(':');
            if (parts.Length < 2)
                throw new ArgumentException(nameof(text));
            int from = int.Parse(parts[0]);
            int to = int.Parse(parts[1]);

            return new Scale(from, to);
        }

        public Scale(int ratioFrom, int ratioTo)
        {
            RatioFrom = ratioFrom;
            RatioTo = ratioTo;
            RatioText = ToString();
        }

        public override string ToString()
        {
            return $"{RatioFrom}:{RatioTo}";
        }
    }
}
