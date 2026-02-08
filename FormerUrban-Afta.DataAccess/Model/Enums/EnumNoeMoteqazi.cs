namespace FormerUrban_Afta.DataAccess.Model.Enums;

public enum EnumNoeMoteqazi
{
    [Display(Name = " ")]
    None,  //0

    [Display(Name = "مالک")]
    owner, //1

    [Display(Name = "وکیل مالک")]
    malekLawyer, //2

    [Display(Name = "مستاجر")]
    tenant, //3

    [Display(Name = "خریدار")]
    Buyer, //4

    [Display(Name = "یکی از مالکین")]
    oneoftheowners, //5

    [Display(Name = "شرکا")]
    Partner, //6
}

