namespace FormerUrban_Afta.DataAccess.Model.Enums;
public enum EnumJahatType
{
    [Display(Name = " ")]
    None,  //0

    [Display(Name = "شمال")]
    north, //1

    [Display(Name = "جنوب")]
    south, //2

    [Display(Name = "غرب")]
    west, //3

    [Display(Name = "شرق")]
    east, //4

    [Display(Name = "شمال شرقی")]
    northeast, //5

    [Display(Name = "جنوب غربی")]
    southwest, //6

    [Display(Name = "شمال غربی")]
    northwest, //7

    [Display(Name = "جنوب شرقی")]
    southeast, //8
}
