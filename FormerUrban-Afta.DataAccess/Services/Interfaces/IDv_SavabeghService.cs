namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IDv_SavabeghService
{
    public List<Dv_savabeghDTO> GetData(int shop, int shod);
    public void Update(Dv_savabeghDTO savabegh);
    public bool DeleteByModel(Dv_savabeghDTO savabegh);
    public bool InsertByModel(Dv_savabeghDTO savabeghDTO, string mtablename);

}
