namespace FormerUrban_Afta.DataAccess.Model.BaseEntity
{
    interface IBaseFile
    {
        public int shop { get; set; }
        public Nullable<long> sh_Darkhast { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
