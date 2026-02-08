using FormerUrban_Afta.DataAccess.DTOs.Parvandeh;
using System.Globalization;

namespace FormerUrban_Afta.ViewComponents
{
    public class ParvandehTreeViewComponent : ViewComponent
    {
        private readonly IParvandehService _parvandehService;
        private readonly MyFunctions _myFunctions;
        

        public ParvandehTreeViewComponent(IParvandehService parvandehService, MyFunctions myFunctions)
        {
            _parvandehService = parvandehService;
            _myFunctions = myFunctions;
        }

        public IViewComponentResult Invoke(int shop, int shod = 0, int dShop = 0, int codeMarhaleh = 0)
        {
            var treeView = new List<TreeViewDTO>();
            var codeN = shop > 0 ? _myFunctions.GetCodNosazi(Convert.ToInt64(shop)) : "";
            var noeParvandeh = shod > 0 ? _myFunctions.GetStrNoeParvandeh(Convert.ToInt64(shop)) : "";
            if (!string.IsNullOrEmpty(codeN))
            {
                var strCn = codeN.Split('-');
                if (strCn.Length >= 4)
                    treeView = shod > 0 ? GetTreeItem(codeN, shop, noeParvandeh) : GetInlineData(codeN);
            }

            ViewBag.shop = shop;
            ViewBag.shod = shod;
            ViewBag.codeMarhaleh = codeMarhaleh;
            return View(treeView);
        }

        #region Parvandeh

        private List<TreeViewDTO> GetInlineData(string strCodeN)
        {
            var parvandeh = _parvandehService.GetRowForTreeViewByCodeN(strCodeN);

            if (parvandeh == null)
                return new List<TreeViewDTO>();
            var type = "";
            var name = "";
            switch (parvandeh.idparent)
            {
                case 0:
                    type = "bx bxs-home";
                    name = "ملک";
                    break;
                case 1:
                    type = "bx bxs-landmark";
                    name = "ساختمان";
                    break;
                case 2:
                    type = "bx bx-building";
                    name = "آپارتمان";
                    break;
            }
            List<TreeViewDTO> tvItems = new List<TreeViewDTO>();
            TreeViewDTO tvItemMelk = new TreeViewDTO
            {
                Text = parvandeh.codeN,
                Id = parvandeh.shop.ToString(),
                Type = type,
                IdParent = parvandeh.idparent.ToString(),
                Name = name,
                IsValid = parvandeh.IsValid,
            };

            var codeN = strCodeN.Split('-');

            var count = _parvandehService.CheckCountParvandeh(Convert.ToInt32(codeN[0]), Convert.ToInt32(codeN[1]),
                Convert.ToInt32(codeN[2]), Convert.ToInt32(codeN[3]), Convert.ToInt32(codeN[4]), Convert.ToInt32(codeN[5]),
                Convert.ToInt32(codeN[6]));
            tvItemMelk.HasChildren = count > 0;
            if (tvItemMelk.HasChildren)
            {
                GetSubItems(tvItemMelk, (int)parvandeh.shop, strCodeN);
            }

            if (tvItemMelk.Text == strCodeN)
            {
                tvItemMelk.Selected = true;
            }
            tvItems.Add(tvItemMelk);
            return tvItems;
        }

        private void GetSubItems(TreeViewDTO tvParent, int intParent, string strCodeN)
        {
            var arrCodeN = strCodeN.Split('-');
            var mantaghe = Convert.ToInt32(arrCodeN[0]);
            var hoze = Convert.ToInt32(arrCodeN[1]);
            var blok = Convert.ToInt32(arrCodeN[2]);
            var melk = Convert.ToInt32(arrCodeN[3]);

            var listParvandeh = _parvandehService.GetByCodeNAndCodeTree(strCodeN, intParent);
            if (!listParvandeh.Any())
                return;

            foreach (var parvandeh in listParvandeh)
            {
                var type = "";
                var name = "";
                switch (parvandeh.idparent)
                {
                    case 0:
                        type = "bx bxs-home";
                        name = "ملک";
                        break;
                    case 1:
                        type = "bx bxs-landmark";
                        name = "ساختمان";
                        break;
                    case 2:
                        type = "bx bx-building";
                        name = "آپارتمان";
                        break;
                    case 3:
                        type = "bx bx-store";
                        name = "صنف";
                        break;
                }
                var tv = new TreeViewDTO
                {
                    Text = parvandeh.codeN,
                    Id = parvandeh.shop.ToString(CultureInfo.InvariantCulture),
                    Type = type,
                    IdParent = parvandeh.idparent.ToString(),
                    Name = name,
                    IsValid = parvandeh.IsValid,
                };


                if (parvandeh.codeN == strCodeN)
                {
                    tv.Selected = true;
                }

                var count = _parvandehService.CheckCountParvandehByCodeTree(mantaghe, hoze, blok, melk, parvandeh.shop);
                tv.HasChildren = count > 0;
                if (tv.HasChildren)
                {
                    GetSubItems(tv, (int)parvandeh.shop, strCodeN);
                }
                tvParent.Items.Add(tv);
            }
        }

        #endregion

        #region Bazdid

        private List<TreeViewDTO> GetTreeItem(string codeNosazi, int shop, string noeParvandeh)
        {
            var isArchive = false;
            //var parvandeh = _parvandehService.GetRowForTreeViewByCodeN(codeNosazi);
            var parvandeh = _parvandehService.GetRow(shop.ToString());
            var tvItems = new List<TreeViewDTO>();

            TreeViewDTO CreateItem(string type, string name)
            {
                return new TreeViewDTO
                {
                    Text = parvandeh.codeN,
                    Id = parvandeh.shop.ToString(),
                    Selected = true,
                    Type = type,
                    Name = name,
                    IsValid = parvandeh.IsValid,
                };
            }

            switch (noeParvandeh)
            {
                case "melk":
                    tvItems.Add(CreateItem("bx bxs-home", "ملک"));
                    break;

                case "sakhteman":
                    var sakhteman = CreateItem("bx bxs-landmark", "ساختمان");
                    var melk = ParentList(shop, "bx bxs-home", "ملک");
                    melk.HasChildren = true;
                    melk.Items.Add(sakhteman);
                    tvItems.Add(melk);
                    break;

                case "aparteman":
                    var aparteman = CreateItem("bx bx-building", "آپارتمان");
                    var sakhtemanParent = ParentList(shop, "bx bxs-landmark", "ساختمان");
                    var melkParent = ParentList(Convert.ToInt32(sakhtemanParent.Id), "bx bxs-home", "ملک");

                    melkParent.HasChildren = sakhtemanParent.HasChildren = true;
                    sakhtemanParent.Items.Add(aparteman);
                    melkParent.Items.Add(sakhtemanParent);

                    tvItems.Add(melkParent);
                    break;
            }

            return tvItems;

        }

        private TreeViewDTO ParentList(int intChildShop, string type, string name)
        {
            var data = _parvandehService.GetRow(intChildShop.ToString())?.code_tree;
            var parent = _parvandehService.GetRow(data?.ToString()
                                                  ?? throw new ApplicationException("شماره پرونده والد برای این پرونده پیدا نشد"));

            return new TreeViewDTO
            {
                Id = parent.shop.ToString(),
                Text = parent.codeN,
                HasChildren = true,
                Type = type,
                Name = name,
                IsValid = parent.IsValid,
            };

        }

        #endregion
    }
}
