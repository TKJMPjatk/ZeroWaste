using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace ZeroWaste.Tests.Helpers;

public static class ITempDataDictionaryProvider
{
    public static ITempDataDictionary GetTempDataDictionary()
    {
        ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
        TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
        ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());
        return tempData;
    }
}