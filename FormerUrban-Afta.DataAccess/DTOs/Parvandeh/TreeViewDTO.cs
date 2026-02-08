namespace FormerUrban_Afta.DataAccess.DTOs.Parvandeh
{
    public class TreeViewDTO
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string IdParent { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Expanded { get; set; }
        public bool Encoded { get; set; } = true;
        public bool Selected { get; set; }
        public string SpriteCssClass { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool HasChildren { get; set; }
        public bool Checked { get; set; }
        public bool IsValid { get; set; }
        public List<TreeViewDTO> Items { get; set; } = new List<TreeViewDTO>();
        public Dictionary<string, string> HtmlAttributes { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ImageHtmlAttributes { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> LinkHtmlAttributes { get; set; } = new Dictionary<string, string>();
    }
}
