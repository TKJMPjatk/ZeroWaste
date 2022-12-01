using ZeroWaste.Data.Enums;
using ZeroWaste.Data.ViewModels.RecipeSearch;

namespace ZeroWaste.Data.Static;

public class SortTypeSupplementer
{
    public static List<SortTypeDisplayVm> SupplementSortTypeVm()
    {
        List<SortTypeDisplayVm> typeDisplayVmsList = new List<SortTypeDisplayVm>();
        foreach (var type in Enum.GetValues<SortTypes>())
        {
            switch (type)
            {
                case SortTypes.FromAtoZ:
                    typeDisplayVmsList.Add(new SortTypeDisplayVm(){SearchType = type, DisplayText = "Od A do Z"});
                    break;
                case SortTypes.FromZToA:
                    typeDisplayVmsList.Add(new SortTypeDisplayVm(){SearchType = type, DisplayText = "Od Z do A"});
                    break;
                case SortTypes.ByDifficultyLevel:
                    typeDisplayVmsList.Add(new SortTypeDisplayVm(){SearchType = type, DisplayText = "Od najłatwiejszych"});
                    break;
                case SortTypes.ByTime:
                    typeDisplayVmsList.Add(new SortTypeDisplayVm(){SearchType = type, DisplayText = "Od najkrótszych"});
                    break;
                case SortTypes.ByGradesDesc:
                    typeDisplayVmsList.Add(new SortTypeDisplayVm(){SearchType = type, DisplayText = "Oceny malejąco"});
                    break;
                case SortTypes.ByGradesAsc:
                    typeDisplayVmsList.Add(new SortTypeDisplayVm(){SearchType = type, DisplayText = "Oceny rosnąco"});
                    break;
            }
        }
        return typeDisplayVmsList;
    }
}